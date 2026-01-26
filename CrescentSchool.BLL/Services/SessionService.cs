using CrescentSchool.BLL.Enums;
using CrescentSchool.BLL.Interfaces;
using CrescentSchool.DAL.Repositories;
using CrescentSchool.Models.Dtos;

namespace CrescentSchool.BLL.Services;

public class SessionService(ISessionsRepository sessionsRepository) : ISessionService
{
    public Task<List<SessionDto>> GetSessionsByStudentIdAsync(Guid studentId, CancellationToken cancellationToken = default)
        => sessionsRepository.GetSessionsByStudentIdAsync(studentId, cancellationToken);
    public async Task<Guid?> CreateSession(SessionDto sessionDto, CancellationToken cancellationToken)
        => await sessionsRepository.CreateSession(sessionDto, cancellationToken);

    public async Task<List<SessionDto>> GetSessionsOfCurrentMonthAndYear(Guid id, Roles role, DateTimeOffset date, CancellationToken cancellationToken = default)
    {
        if (role == Roles.Student)
            return await sessionsRepository.GetSessionsByStudentIdAndDateRangeAsync(id, date, cancellationToken);
        else if (role == Roles.Instructor)
            return await sessionsRepository.GetSessionsByInstructorIdAndDateRangeAsync(id, date, cancellationToken);
        else
            return [];
    }

    public Task<List<SessionDto>> GetSessionsByInstructorIdAsync(Guid instructorId, CancellationToken cancellationToken = default)
           => sessionsRepository.GetSessionsByInstructorIdAsync(instructorId, cancellationToken);

}
