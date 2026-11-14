using CrescentSchool.DAL.Entities;

namespace CrescentSchool.DAL.Repositories;

public sealed record ZoomSessionParticipants(string? StudentEmail, string? InstructorEmail, string? InstructorZoomUserId);

public interface IZoomWebhookInboxRepository
{
    Task<bool> InsertIfNewAsync(string eventName, long meetingId, string meetingUuid, byte[] bodyHash, string rawBody, CancellationToken cancellationToken);
    Task<Dictionary<long, List<ZoomWebhookInboxMessage>>> GetPendingMessagesAsync(CancellationToken cancellationToken);
    Task<ZoomWebhookInboxMessage?> GetPendingMessageAsync(Guid id, CancellationToken cancellationToken);
    Task<UpCommingSessions?> GetSessionByMeetingIdAsync(long meetingId, CancellationToken cancellationToken);
    Task<ZoomSessionParticipants?> GetSessionParticipantsAsync(Guid studentId, Guid instructorId, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
    void ResetContext();
}