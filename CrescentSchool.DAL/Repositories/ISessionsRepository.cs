using CrescentSchool.DAL.Dtos;

namespace CrescentSchool.DAL.Repositories;

public interface ISessionsRepository
{
    Task<List<GetSessionDto>> GetSessionsAsync(CancellationToken cancellationToken);
    Task<Guid?> CreateSession(CreateSessionDto sessionDto, CancellationToken cancellationToken);
    Task<List<GetSessionDto>> GetSessionsByInstructorIdAndDateAsync(Guid id, DateTimeOffset date, CancellationToken cancellationToken);
    Task<List<GetSessionDto>> GetSessionsByInstructorIdAsync(Guid instructorId, CancellationToken cancellationToken);
    Task<List<GetSessionDto>> GetSessionsByStudentIdAndDateAsync(Guid studentId, DateTimeOffset date, CancellationToken cancellationToken);
    Task<List<GetSessionDto>> GetSessionsByStudentIdAsync(Guid studentId, CancellationToken cancellationToken = default);
    Task UpdateSession(Session session, CancellationToken cancellationToken);
    Task<Session?> GetSessionByIdAsync(Guid id, CancellationToken cancellationToken);
}
