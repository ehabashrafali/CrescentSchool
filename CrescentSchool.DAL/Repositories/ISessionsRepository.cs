using CrescentSchool.DAL.Dtos;

namespace CrescentSchool.DAL.Repositories;

public interface ISessionsRepository
{
    Task<Guid?> CreateSession(SessionDto sessionDto, CancellationToken cancellationToken);
    Task<List<SessionDto>> GetSessionsByInstructorIdAndDateRangeAsync(Guid id, DateTimeOffset date, CancellationToken cancellationToken);
    Task<List<SessionDto>> GetSessionsByInstructorIdAsync(Guid instructorId, CancellationToken cancellationToken);
    Task<List<SessionDto>> GetSessionsByStudentIdAndDateRangeAsync(Guid studentId, DateTimeOffset date, CancellationToken cancellationToken);
    Task<List<SessionDto>> GetSessionsByStudentIdAsync(Guid studentId, CancellationToken cancellationToken = default);
}
