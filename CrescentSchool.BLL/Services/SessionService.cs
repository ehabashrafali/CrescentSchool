using CrescentSchool.BLL.Interfaces;
using CrescentSchool.DAL.Dtos;
using CrescentSchool.DAL.Entities;
using CrescentSchool.DAL.Repositories;
using CrescentSchool.Models.Enums;

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
        return await sessionsRepository.CreateSession(sessionDto, cancellationToken);
    }

    public async Task<List<GetSessionDto>> GetSessionsByUserIdAndDate(Guid id, Role role, DateTimeOffset date, CancellationToken cancellationToken = default)
    {
        if (role == Role.Student)
        {
            var sessions = await sessionsRepository.GetSessionsByStudentIdAndDateAsync(id, date, cancellationToken);
            return [.. sessions.Select(ToGetSessionDto)];
        }
        else if (role == Role.Instructor)
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

    public async Task<List<TimeTableSessionDto>> GetTimeTableSessionsDtoAsync(Guid id, Role role, CancellationToken cancellationToken = default)
    {
        return role switch
        {
            Role.Student =>
                await GetStudentTimeTableAsync(id, cancellationToken),

            Role.Instructor =>
                await GetInstructorTimeTableAsync(id, cancellationToken),

            _ => throw new ArgumentOutOfRangeException(nameof(role), role, "Unsupported role.")
        };
    }

    private async Task<List<TimeTableSessionDto>> GetStudentTimeTableAsync(Guid id, CancellationToken cancellationToken)
    {
        return [.. (await sessionsRepository.GetStudentUpCommingSessions(id, cancellationToken))
            .Select(session => new TimeTableSessionDto
            {
                SessionDateTime = session.SessionDateTime,
                ZoomMeeting = session.ZoomMeeting,
                StudentName = session.StudentName,
                InstructorName = session.InstructorName,
                ZoomMeetingId = session.ZoomMeetingId,
                IsCanceled = session.IsCancelled
            })];
    }

    private async Task<List<TimeTableSessionDto>> GetInstructorTimeTableAsync(Guid id, CancellationToken cancellationToken)
    {
        return [.. (await sessionsRepository.GetInstructorUpCommingSessions(id, cancellationToken))
            .Select(session => new TimeTableSessionDto
            {
                SessionDateTime = session.SessionDateTime,
                ZoomMeeting = session.ZoomMeeting,
                StudentName = session.StudentName,
                InstructorName = session.InstructorName,
                ZoomMeetingId = session.ZoomMeetingId,
                IsCanceled = session.IsCancelled
            })];
    }

    public async Task CancelUpCommingSessionAsync(long meetingId, CancellationToken cancellationToken = default)
    {
        var session = await sessionsRepository.GetSessionByMeetingIdAsync(meetingId, cancellationToken) ?? throw new InvalidOperationException($"Session with id {meetingId} not found.");
        session.IsCancelled = true;
        await sessionsRepository.UpdateUpCommingSessions(session, cancellationToken);
    }
}