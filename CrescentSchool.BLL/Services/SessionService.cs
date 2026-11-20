using CrescentSchool.BLL.Interfaces;
using CrescentSchool.BLL.Validation;
using CrescentSchool.Core.Extensions;
using CrescentSchool.Core.Exceptions;
using CrescentSchool.DAL.Dtos;
using CrescentSchool.DAL.Entities;
using CrescentSchool.DAL.Repositories;
using CrescentSchool.Models.Enums;

namespace CrescentSchool.BLL.Services;

public class SessionService(ISessionsRepository sessionsRepository, RescheduleSessionValidator rescheduleSessionValidator, CancelSessionValidator cancelSessionValidator) : ISessionService
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
        var session = await sessionsRepository.GetSessionByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException($"Session with id {id} not found.");

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
        var sessions = role switch
        {
            Role.Student => await sessionsRepository.GetStudentUpComingSessions(id, cancellationToken),
            Role.Instructor => await sessionsRepository.GetInstructorUpComingSessions(id, cancellationToken),
            _ => throw new ArgumentOutOfRangeException(nameof(role), role, "Unsupported role.")
        };

        return [.. sessions.Select(session => new TimeTableSessionDto
            {
                SessionDateTime = session.SessionDateTime,
                ZoomMeeting = session.ZoomMeeting,
                StudentName = session.StudentName,
                InstructorName = session.InstructorName,
                ZoomMeetingId = session.ZoomMeetingId,
                Canceled = session.Canceled
            })];
    }

    public async Task RescheduleSessionAsync(long meetingId, DateTimeOffset newDateTime, CancellationToken cancellationToken = default)
    {
        var session = await sessionsRepository.GetUpComingSessionByIdAsync(meetingId, cancellationToken)
            ?? throw new NotFoundException($"Session with meeting Id {meetingId} not found.");

        var validation = await rescheduleSessionValidator.ValidateAsync(session, cancellationToken);
        if (!validation.IsValid)
        {
            throw new BusinessValidationException(
                validation.Errors[0].ErrorMessage);
        }

        session.SessionDateTime = newDateTime.UtcDateTime;
        session.IsRescheduled = true;
        await sessionsRepository.UpdateUpComingSessions(session, cancellationToken);
    }

    public async Task CancelUpCommingSessionAsync(long meetingId, Role role, CancellationToken cancellationToken = default)
    {
        var session = await sessionsRepository.GetSessionByMeetingIdAsync(meetingId, cancellationToken) ?? throw new InvalidOperationException($"Session with id {meetingId} not found.");
        var validation = await cancelSessionValidator.ValidateAsync(
            new CancelSessionValidation(session.SessionDateTime, role), cancellationToken);
        if (!validation.IsValid)
        {
            throw new BusinessValidationException(
                validation.Errors[0].ErrorMessage);
        }

        session.Canceled = role == Role.Student ? StudentAttendanceStatus.Cancelled : StudentAttendanceStatus.CancelledByInstructor;
        await sessionsRepository.UpdateUpComingSessions(session, cancellationToken);
    }
}