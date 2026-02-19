using CrescentSchool.BLL.Enums;
using CrescentSchool.DAL.Dtos;

namespace CrescentSchool.BLL.Interfaces;

public interface ISessionService
{
    Task<List<SessionDto>> GetSessionsByStudentIdAsync(Guid studentId, CancellationToken cancellationToken = default);
    Task<List<SessionDto>> GetSessionsByInstructorIdAsync(Guid instructorId, CancellationToken cancellationToken = default);
    Task<Guid?> CreateSession(SessionDto sessionDto, CancellationToken cancellationToken);
    Task<List<SessionDto>> GetSessionsOfCurrentMonthAndYear(Guid id, Roles role, DateTimeOffset date, CancellationToken cancellationToken = default);
}
