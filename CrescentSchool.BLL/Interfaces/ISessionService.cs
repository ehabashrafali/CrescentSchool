using CrescentSchool.BLL.Enums;
using CrescentSchool.DAL.Dtos;

namespace CrescentSchool.BLL.Interfaces;

public interface ISessionService
{
    Task<List<GetSessionDto>> GetSessionsAsync(CancellationToken cancellationToken = default);
    Task<List<GetSessionDto>> GetSessionsByStudentIdAsync(Guid studentId, CancellationToken cancellationToken = default);
    Task<List<GetSessionDto>> GetSessionsByInstructorIdAsync(Guid instructorId, CancellationToken cancellationToken = default);
    Task<Guid?> CreateSession(CreateSessionDto sessionDto, CancellationToken cancellationToken);
    Task<List<GetSessionDto>> GetSessionsByIdAndDate(Guid id, Roles role, DateTimeOffset date, CancellationToken cancellationToken = default);
    Task DeleteSessionAsync(Guid id, CancellationToken cancellationToken);
}
