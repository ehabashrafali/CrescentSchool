using CrescentSchool.DAL.Entities;

namespace CrescentSchool.DAL.Repositories;

public sealed record ZoomSessionParticipants(string? StudentEmail, string? InstructorEmail, string? InstructorZoomUserId);

public interface IZoomWebhookInboxRepository
{
    Task<bool> InsertIfNewAsync(string eventName, string meetingUuid, byte[] bodyHash, string rawBody, CancellationToken cancellationToken);
    Task<List<Guid>> GetPendingMessageIdsAsync(int take, CancellationToken cancellationToken);
    Task<ZoomWebhookInboxMessage?> GetPendingMessageAsync(Guid id, CancellationToken cancellationToken);
    Task<List<UpCommingSessions>> GetSessionsByMeetingUuidAsync(string meetingUuid, CancellationToken cancellationToken);
    Task<ZoomSessionParticipants?> GetSessionParticipantsAsync(Guid studentId, Guid instructorId, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
    void ResetContext();
}