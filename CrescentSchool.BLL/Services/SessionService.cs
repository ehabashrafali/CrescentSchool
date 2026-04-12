using CrescentSchool.BLL.Enums;
using CrescentSchool.BLL.Interfaces;
using CrescentSchool.DAL.Dtos;
using CrescentSchool.DAL.Repositories;

namespace CrescentSchool.BLL.Services;

public class SessionService(ISessionsRepository sessionsRepository) : ISessionService
{

    public async Task<List<GetSessionDto>> GetSessionsByStudentIdAsync(Guid studentId, CancellationToken cancellationToken = default)
    {
        var sessions = await sessionsRepository.GetSessionsByStudentIdAsync(studentId, cancellationToken);
        return [.. sessions.Select(ToGetSessionDto)];
    }

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

    public async Task<List<GetSessionDto>> GetSessionsByUserIdAndDate(Guid id, Roles role, DateTimeOffset date, CancellationToken cancellationToken = default)
    {
        if (role == Roles.Student)
        {
            var sessions = await sessionsRepository.GetSessionsByStudentIdAndDateAsync(id, date, cancellationToken);
            return [.. sessions.Select(ToGetSessionDto)];
        }
        else if (role == Roles.Instructor)
        {
            var sessions = await sessionsRepository.GetSessionsByInstructorIdAndDateAsync(id, date, cancellationToken);
            return [.. sessions.Select(ToGetSessionDto)];
        }

        return [];
    }

    public async Task<List<GetSessionDto>> GetSessionsByInstructorIdAsync(Guid instructorId, CancellationToken cancellationToken = default)
    {
        var sessions = await sessionsRepository.GetSessionsByInstructorIdAsync(instructorId, cancellationToken);
        return [.. sessions.Select(ToGetSessionDto)];
    }

    public async Task<GetSessionsResult> GetSessionsAsync(int? pageNumber, int? pageSize, CancellationToken cancellationToken = default)
    {
        var sessions = await sessionsRepository.GetSessionsAsync(pageNumber, pageSize, cancellationToken);

        return new GetSessionsResult
        {
            Sessions = sessions.sessions.Select(ToGetSessionDto).ToList(),
            TotalCount = sessions.totalCount
        };
    }

    public async Task DeleteSessionAsync(Guid id, CancellationToken cancellationToken)
        => await sessionsRepository.DeleteSessionAsync(id, cancellationToken);

    public async Task<GetSessionDto?> GetSessionByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var session = await sessionsRepository.GetSessionByIdAsync(id, cancellationToken);
        return session is null ? null : ToGetSessionDto(session);
    }

    public async Task<Guid> UpdateSessionAsync(Guid id, UpdateSessionDto sessionDto, CancellationToken cancellationToken)
    {
        var session = await sessionsRepository.GetSessionByIdAsync(id, cancellationToken);

        session.UpdateSession(
            sessionDto.StudentId,
            sessionDto.InstructorId,
            sessionDto.StudentSessionStatus,
            sessionDto.InstructorSessionStatus,
            sessionDto.Duration);

        await sessionsRepository.UpdateSession(session, cancellationToken);
        return session.Id;
    }
    private static GetSessionDto ToGetSessionDto(Session session)
    {
        return new GetSessionDto
        {
            Id = session.Id,
            Date = session.Date,
            StudentId = session.StudentId,
            InstructorId = session.InstructorId,
            StudentSessionStatus = session.StudentStatus,
            InstructorSessionStatus = session.InstructorStatus,
            Duration = session.Duration,
            StudentName = session.Student.User.FullName,
            InstructorName = session.Instructor.User.FullName
        };
    }

}