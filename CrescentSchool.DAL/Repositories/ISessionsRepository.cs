using CrescentSchool.DAL.Dtos;
using CrescentSchool.DAL.Entities;

namespace CrescentSchool.DAL.Repositories;

public interface ISessionsRepository
{
    Task<(List<Session> sessions, int totalCount)> GetSessionsAsync(int? pageNumber, int? pageSize, CancellationToken cancellationToken);
    Task<Guid?> CreateSession(CreateSessionDto sessionDto, CancellationToken cancellationToken);
    Task<List<Session>> GetSessionsByInstructorIdAndDateAsync(Guid id, DateTimeOffset date, CancellationToken cancellationToken);
    Task<List<Session>> GetSessionsByInstructorIdAsync(Guid instructorId, CancellationToken cancellationToken);
    Task<List<Session>> GetSessionsByStudentIdAndDateAsync(Guid studentId, DateTimeOffset date, CancellationToken cancellationToken);
    Task<List<Session>> GetSessionsByStudentIdAsync(Guid studentId, CancellationToken cancellationToken = default);
    Task UpdateSession(Session session, CancellationToken cancellationToken);
    Task<Session?> GetSessionByIdAsync(Guid id, CancellationToken cancellationToken);
    Task DeleteSessionAsync(Guid sessionId, CancellationToken cancellation);
    Task CreateUpCommingSessions(List<TimeTableSessionDto> upcomingSessions, CancellationToken cancellationToken);
    Task<List<UpCommingSessions>> GetStudentUpCommingSessions(Guid studentId, CancellationToken cancellationToken);
    Task<List<UpCommingSessions>> GetInstructorUpCommingSessions(Guid instructorId, CancellationToken cancellationToken);
    Task MarkJoinedAsync(string zoomMeetingUuid, DateTime joinedAtUtc, CancellationToken cancellationToken);
    Task MarkLeftAsync(string zoomMeetingUuid, DateTime leftAtUtc, CancellationToken cancellationToken);
    Task<UpCommingSessions?> GetSessionByMeetingIdAsync(long meetingId, CancellationToken cancellationToken);
    Task UpdateUpCommingSessions(UpCommingSessions session, CancellationToken cancellationToken);
    Task<List<UpCommingSessions>> GetUpCommingSessionsToSettleAsync(CancellationToken cancellationToken);
    Task AddSessions(List<Session> sessions, CancellationToken cancellationToken);
}
