using CrescentSchool.BLL.Enums;
using CrescentSchool.BLL.Interfaces;
using CrescentSchool.DAL.Dtos;
using CrescentSchool.DAL.Repositories;

namespace CrescentSchool.BLL.Services;

public class SessionService(ISessionsRepository sessionsRepository) : ISessionService
{
    public Task<List<GetSessionDto>> GetSessionsByStudentIdAsync(Guid studentId, CancellationToken cancellationToken = default)
        => sessionsRepository.GetSessionsByStudentIdAsync(studentId, cancellationToken);
    public async Task<Guid?> CreateSession(CreateSessionDto sessionDto, CancellationToken cancellationToken)
    {
        try
        {
            return await sessionsRepository.CreateSession(sessionDto, cancellationToken);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<List<GetSessionDto>> GetSessionsByIdAndDate(Guid id, Roles role, DateTimeOffset date, CancellationToken cancellationToken = default)
    {
        if (role == Roles.Student)
            return await sessionsRepository.GetSessionsByStudentIdAndDateAsync(id, date, cancellationToken);
        else if (role == Roles.Instructor)
            return await sessionsRepository.GetSessionsByInstructorIdAndDateAsync(id, date, cancellationToken);
        else
            return [];
    }
    public Task<List<GetSessionDto>> GetSessionsByInstructorIdAsync(Guid instructorId, CancellationToken cancellationToken = default)
           => sessionsRepository.GetSessionsByInstructorIdAsync(instructorId, cancellationToken);

    public Task<List<GetSessionDto>> GetSessionsAsync(CancellationToken cancellationToken = default)
        => sessionsRepository.GetSessionsAsync(cancellationToken);

    public async Task DeleteSessionAsync(Guid id, CancellationToken cancellationToken)
    {
        var session = await sessionsRepository.GetSessionByIdAsync(id, cancellationToken);
        if (session is not null)
        {
            await sessionsRepository.DeleteSessionAsync(session, cancellationToken);
        }
        else
            throw new Exception("Session not found");
    }
}
