using CrescentSchool.Models.Enums;
using CrescentSchool.DAL.Entities;
using CrescentSchool.DAL.Repositories;
using Hangfire;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace CrescentSchool.BLL.Jobs;

public class ProcessZoomWebhookInboxJob(IZoomWebhookInboxRepository inboxRepository, ILogger<ProcessZoomWebhookInboxJob> logger, ISessionsRepository sessionsRepository)
{
    public const string Name = "ProcessZoomWebhookInboxJob";
    private const string ParticipantJoined = "meeting.participant_joined";
    private const string ParticipantLeft = "meeting.participant_left";

    [DisableConcurrentExecution(timeoutInSeconds: 50)]
    [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
    public async Task Execute(CancellationToken cancellationToken = default)
    {
        var messages = await inboxRepository.GetPendingMessagesAsync(cancellationToken);
        foreach (var item in messages)
        {
            try
            {
                await ProcessMessageAsync(item, cancellationToken);
            }
            catch (Exception exception) when (exception is not OperationCanceledException)
            {
                logger.LogError(exception, "Failed to process Zoom webhook messages for meeting Id: {Id}", item.Key);
                inboxRepository.ResetContext();
            }
        }
    }

    private async Task ProcessMessageAsync(KeyValuePair<long, List<ZoomWebhookInboxMessage>> item, CancellationToken cancellationToken)
    {
        var upComing = await sessionsRepository.GetSessionByMeetingIdAsync(item.Key, cancellationToken);

        if (upComing is null)
            logger.LogWarning("No upcoming session found for meeting Id: {MeetingId}", item.Key);

        else
        {
            foreach (var message in item.Value.OrderBy(message => message.ReceivedAt))
            {
                if (message.EventName != ParticipantJoined)
                {
                    switch (message.EventName)
                    {
                        case ParticipantLeft:
                        {
                            var leftAt = GetEventTime(message, "leave_time");

                            if (upComing.LastLeftAt is null || leftAt > upComing.LastLeftAt)
                                upComing.LastLeftAt = leftAt;
                            break;
                        }
                    }
                }
                else
                {
                    var joinedAt = GetEventTime(message, "join_time");

                    if (upComing.FirstJoinedAt is null || joinedAt < upComing.FirstJoinedAt)
                        upComing.FirstJoinedAt = joinedAt;
                }
            }

            upComing.IsJoined = true;

            if (upComing.FirstJoinedAt is not null && upComing.LastLeftAt > upComing.FirstJoinedAt)
                upComing.TotalDuration = (SessionDuration)(int)(upComing.LastLeftAt.Value - upComing.FirstJoinedAt.Value).TotalMinutes;
        }

        var processedAt = DateTime.UtcNow;
        foreach (var message in item.Value)
            message.ProcessedAt = processedAt;

        await inboxRepository.SaveChangesAsync(cancellationToken);
    }

    private static DateTime GetEventTime(ZoomWebhookInboxMessage message, string propertyName)
    {

        using var document = JsonDocument.Parse(message.Payload);

        if (document.RootElement.TryGetProperty("payload", out var payload) &&
            payload.TryGetProperty("object", out var meeting) && meeting.TryGetProperty("participant", out var participant) && participant.TryGetProperty(propertyName, out var time) &&
            time.ValueKind == JsonValueKind.String && time.TryGetDateTime(out var value))
            return value.Kind == DateTimeKind.Utc ? value : DateTime.SpecifyKind(value.ToUniversalTime(), DateTimeKind.Utc);

        return DateTime.SpecifyKind(message.ReceivedAt, DateTimeKind.Utc);
    }
}
