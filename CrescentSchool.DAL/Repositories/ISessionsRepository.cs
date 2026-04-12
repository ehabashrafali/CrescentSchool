using CrescentSchool.DAL.Dtos;

namespace CrescentSchool.DAL.Repositories;

public interface ISessionsRepository
{
    Task<List<Session>> GetSessionsAsync(CancellationToken cancellationToken);
    Task<Guid?> CreateSession(CreateSessionDto sessionDto, CancellationToken cancellationToken);
    Task<List<Session>> GetSessionsByInstructorIdAndDateAsync(Guid id, DateTimeOffset date, CancellationToken cancellationToken);
    Task<List<Session>> GetSessionsByInstructorIdAsync(Guid instructorId, CancellationToken cancellationToken);
    Task<List<Session>> GetSessionsByStudentIdAndDateAsync(Guid studentId, DateTimeOffset date, CancellationToken cancellationToken);
    Task<List<Session>> GetSessionsByStudentIdAsync(Guid studentId, CancellationToken cancellationToken = default);
    Task UpdateSession(Session session, CancellationToken cancellationToken);
    Task<Session?> GetSessionByIdAsync(Guid id, CancellationToken cancellationToken);
    Task DeleteSessionAsync(Guid sessionId, CancellationToken cancellation);
}
