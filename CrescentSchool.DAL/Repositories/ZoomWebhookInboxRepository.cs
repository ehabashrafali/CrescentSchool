using CrescentSchool.DAL.DbContext;
using CrescentSchool.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace CrescentSchool.DAL.Repositories;

public sealed class ZoomWebhookInboxRepository(ApplicationDbContext dbContext)
    : IZoomWebhookInboxRepository
{
    public async Task<bool> InsertIfNewAsync(string eventName, string meetingUuid, byte[] bodyHash, string rawBody, CancellationToken cancellationToken)
    {
        var message = new ZoomWebhookInboxMessage
        {
            Id = Guid.NewGuid(),
            EventName = eventName,
            MeetingUUID = meetingUuid,
            Payload = rawBody,
            BodySha256 = bodyHash,
            ReceivedAt = DateTime.UtcNow
        };

        var entry = dbContext.Set<ZoomWebhookInboxMessage>().Add(message);

        try
        {
            await dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }
        catch (DbUpdateException exception) when (exception.InnerException is PostgresException
        {
            SqlState: PostgresErrorCodes.UniqueViolation,
            ConstraintName: "uq_zoom_webhook_inbox_body_sha256"
        })
        {
            entry.State = EntityState.Detached;
            return false;
        }
    }

    public Task<List<Guid>> GetPendingMessageIdsAsync(int take, CancellationToken cancellationToken) =>
        dbContext.ZoomWebhookInboxMessages
            .AsNoTracking()
            .Where(message => message.ProcessedAt == null)
            .OrderBy(message => message.ReceivedAt)
            .ThenBy(message => message.Id)
            .Select(message => message.Id)
            .Take(take)
            .ToListAsync(cancellationToken);

    public Task<ZoomWebhookInboxMessage?> GetPendingMessageAsync(Guid id, CancellationToken cancellationToken) =>
        dbContext.ZoomWebhookInboxMessages
            .FirstOrDefaultAsync(message => message.Id == id && message.ProcessedAt == null, cancellationToken);

    public Task<List<UpCommingSessions>> GetSessionsByMeetingUuidAsync(string meetingUuid, CancellationToken cancellationToken) =>
        dbContext.UpCommingSessions
            .Where(session => session.ZoomMeetingUuid == meetingUuid)
            .Take(2)
            .ToListAsync(cancellationToken);

    public async Task<ZoomSessionParticipants?> GetSessionParticipantsAsync(Guid studentId, Guid instructorId, CancellationToken cancellationToken)
    {
        var studentEmail = await dbContext.Students
            .AsNoTracking()
            .Where(student => student.Id == studentId)
            .Select(student => student.User.Email)
            .FirstOrDefaultAsync(cancellationToken);

        var instructor = await dbContext.Instructors
            .AsNoTracking()
            .Where(instructor => instructor.Id == instructorId)
            .Select(instructor => new { instructor.User.Email, instructor.ZoomUserId })
            .FirstOrDefaultAsync(cancellationToken);

        if (studentEmail is null && instructor is null)
            return null;

        return new ZoomSessionParticipants(studentEmail, instructor?.Email, instructor?.ZoomUserId);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken) =>
        dbContext.SaveChangesAsync(cancellationToken);

    public void ResetContext() => dbContext.ChangeTracker.Clear();
}