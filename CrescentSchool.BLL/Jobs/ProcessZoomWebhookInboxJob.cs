using CrescentSchool.DAL.Entities;
using CrescentSchool.DAL.Repositories;
using Hangfire;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Text.Json;

namespace CrescentSchool.BLL.Jobs;

public class ProcessZoomWebhookInboxJob(IZoomWebhookInboxRepository inboxRepository, ILogger<ProcessZoomWebhookInboxJob> logger)
{
    public const string Name = "ProcessZoomWebhookInboxJob";
    public const int BatchSize = 100;

    private const string ParticipantJoined = "meeting.participant_joined";
    private const string ParticipantLeft = "meeting.participant_left";

    private enum ParticipantRole
    {
        Student,
        Instructor
    }

    private sealed record ParsedEvent(string? Email, string? ParticipantId, string? UserId, DateTime Timestamp);

    [DisableConcurrentExecution(timeoutInSeconds: 50)]
    [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
    public async Task Execute(CancellationToken cancellationToken = default)
    {
        var messageIds = await inboxRepository.GetPendingMessageIdsAsync(BatchSize, cancellationToken);

        foreach (var messageId in messageIds)
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                await ProcessMessageAsync(messageId, cancellationToken);
            }
            catch (Exception exception) when (exception is not OperationCanceledException)
            {
                logger.LogError(exception, "Failed to process Zoom webhook inbox message {MessageId}. It remains pending for retry.", messageId);
            }
            finally
            {
                inboxRepository.ResetContext();
            }
        }
    }

    private async Task ProcessMessageAsync(Guid messageId, CancellationToken cancellationToken)
    {
        var message = await inboxRepository.GetPendingMessageAsync(messageId, cancellationToken);

        if (message is null)
            return;

        if (message.EventName is not (ParticipantJoined or ParticipantLeft))
        {
            await CompleteAsync(message, "unsupported event", cancellationToken);
            return;
        }

        if (!TryParse(message, out var parsed, out var parseError))
        {
            await CompleteAsync(message, parseError, cancellationToken);
            return;
        }

        var sessions = await inboxRepository.GetSessionsByMeetingUuidAsync(message.MeetingUUID, cancellationToken);

        if (sessions.Count != 1)
        {
            await CompleteAsync(message, sessions.Count == 0 ? "no session matches meeting UUID" : "multiple sessions match meeting UUID", cancellationToken);
            return;
        }

        var session = sessions[0];
        var participants = await inboxRepository.GetSessionParticipantsAsync(session.StudentId, session.InstructorId, cancellationToken);

        var role = ResolveRole(parsed, participants, out var identityError);

        if (role is null)
        {
            await CompleteAsync(message, $"participant identity unresolved: {identityError}", cancellationToken);
            return;
        }

        if (role == ParticipantRole.Instructor)
        {
            await CompleteAsync(message, "instructor event; student attendance not changed", cancellationToken);
            return;
        }

        if (message.EventName == ParticipantJoined)
        {
            if (session.FirstJoinedAt is null || parsed.Timestamp < session.FirstJoinedAt.Value)
                session.FirstJoinedAt = parsed.Timestamp;
        }
        else
        {
            if (session.LastLeftAt is null || parsed.Timestamp > session.LastLeftAt.Value)
                session.LastLeftAt = parsed.Timestamp;
        }

        await CompleteAsync(message, null, cancellationToken);
    }

    private async Task CompleteAsync(ZoomWebhookInboxMessage message, string? skipReason, CancellationToken cancellationToken)
    {
        if (skipReason is not null)
            logger.LogWarning("Zoom webhook inbox message {MessageId} ({EventName}, meeting {MeetingUuid}) skipped: {Reason}.", message.Id, message.EventName, message.MeetingUUID, skipReason);

        message.ProcessedAt = DateTime.UtcNow;

        await inboxRepository.SaveChangesAsync(cancellationToken);
    }

    private static ParticipantRole? ResolveRole(ParsedEvent parsed, ZoomSessionParticipants? participants, out string error)
    {
        error = string.Empty;

        if (participants is null)
        {
            error = "session student and instructor not found";
            return null;
        }

        var isInstructorById = !string.IsNullOrWhiteSpace(participants.InstructorZoomUserId) &&
            (string.Equals(parsed.ParticipantId, participants.InstructorZoomUserId, StringComparison.Ordinal) ||
             string.Equals(parsed.UserId, participants.InstructorZoomUserId, StringComparison.Ordinal));

        var isInstructorByEmail = EmailEquals(parsed.Email, participants.InstructorEmail);
        var isStudentByEmail = EmailEquals(parsed.Email, participants.StudentEmail);
        var isInstructor = isInstructorById || isInstructorByEmail;

        if (isInstructor && isStudentByEmail)
        {
            error = "participant matches both student and instructor";
            return null;
        }

        if (isInstructor)
            return ParticipantRole.Instructor;

        if (isStudentByEmail)
            return ParticipantRole.Student;

        error = string.IsNullOrWhiteSpace(parsed.Email)
            ? "participant email missing from payload"
            : "participant email does not match student or instructor";

        return null;
    }

    private static bool EmailEquals(string? left, string? right) =>
        !string.IsNullOrWhiteSpace(left) && !string.IsNullOrWhiteSpace(right) &&
        string.Equals(left.Trim(), right.Trim(), StringComparison.OrdinalIgnoreCase);

    private static bool TryParse(ZoomWebhookInboxMessage message, out ParsedEvent parsed, out string error)
    {
        parsed = null!;
        error = string.Empty;

        try
        {
            using var document = JsonDocument.Parse(message.Payload);

            if (!document.RootElement.TryGetProperty("payload", out var payload) || payload.ValueKind != JsonValueKind.Object ||
                !payload.TryGetProperty("object", out var meeting) || meeting.ValueKind != JsonValueKind.Object ||
                !meeting.TryGetProperty("participant", out var participant) || participant.ValueKind != JsonValueKind.Object)
            {
                error = "payload.object.participant missing";
                return false;
            }

            DateTime? timestamp = message.EventName == ParticipantJoined
                ? ReadUtc(participant, "join_time") ?? ReadUtc(participant, "date_time") ?? ReadUtc(meeting, "date_time")
                : ReadUtc(participant, "leave_time");

            if (timestamp is null)
            {
                error = message.EventName == ParticipantJoined
                    ? "participant join_time/date_time missing or invalid"
                    : "participant leave_time missing or invalid";
                return false;
            }

            parsed = new ParsedEvent(
                ReadString(participant, "email"),
                ReadString(participant, "id"),
                ReadString(participant, "user_id"),
                timestamp.Value);

            return true;
        }
        catch (JsonException)
        {
            error = "payload is not valid JSON";
            return false;
        }
    }

    private static string? ReadString(JsonElement element, string name) =>
        element.TryGetProperty(name, out var value) && value.ValueKind == JsonValueKind.String ? value.GetString() : null;

    private static DateTime? ReadUtc(JsonElement element, string name)
    {
        var value = ReadString(element, name);

        if (string.IsNullOrWhiteSpace(value) ||
            !DateTimeOffset.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal, out var parsed))
            return null;

        return parsed.UtcDateTime;
    }
}
