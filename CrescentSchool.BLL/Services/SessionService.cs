using CrescentSchool.BLL.Interfaces;
using CrescentSchool.DAL.Dtos;
using CrescentSchool.DAL.Entities;
using CrescentSchool.DAL.Repositories;
using CrescentSchool.Models.Enums;
using System.Globalization;

namespace CrescentSchool.BLL.Services;

public class SessionService(ISessionsRepository sessionsRepository, IStudentsRepository studentsRepository, IInstructorsRepository instructorsRepository) : ISessionService
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

    private async Task<List<TimeTableSessionDto>> GetStudentTimeTableAsync(Guid studentId, CancellationToken cancellationToken = default)
    {
        var student = await studentsRepository.GetStudentByIdAsync(studentId, cancellationToken) ?? throw new KeyNotFoundException($"Student with ID {studentId} was not found.");

        var appointments = student.WeeklyAppointments.Select(appointment => (appointment.Day, appointment.Time));

        return GenerateTimeTable(appointments, sessionDateTime => new TimeTableSessionDto
        {
            SessionDateTime = sessionDateTime,
            ParticipantName = student.Instructor?.User.FullName
        });
    }

    private async Task<List<TimeTableSessionDto>> GetInstructorTimeTableAsync(Guid instructorId, CancellationToken cancellationToken = default)
    {
        var instructor = await instructorsRepository.GetByIdAsync(instructorId, cancellationToken) ?? throw new KeyNotFoundException($"Instructor with ID {instructorId} was not found.");

        var appointments = instructor.Students.SelectMany(student => student.WeeklyAppointments)
                                               .Select(appointment => (appointment.Day, appointment.Time));

        return GenerateTimeTable(appointments, sessionDateTime => new TimeTableSessionDto
        {
            SessionDateTime = sessionDateTime,
            StudentName = instructor.Students.Where(s => s.WeeklyAppointments.Any(w =>
                            w.Day == sessionDateTime.DayOfWeek.ToString()))
                        .Select(s => s.User.FullName)
                        .First()
        });
    }

    private static List<TimeTableSessionDto> GenerateTimeTable(IEnumerable<(string Day, string Time)> appointments, Func<DateTime, TimeTableSessionDto> createSession)
    {
        var now = DateTime.Now;
        var lastDayOfMonth = new DateTime(now.Year, now.Month, DateTime.DaysInMonth(now.Year, now.Month));

        var result = new List<TimeTableSessionDto>();

        foreach (var appointment in appointments)
        {
            if (!Enum.TryParse<DayOfWeek>(appointment.Day, ignoreCase: true, out var dayOfWeek))
                continue;

            if (!TimeSpan.TryParseExact(appointment.Time?.Trim(), [@"hh\.mm", @"h\.mm", @"hh\:mm", @"h\:mm", @"%h", @"hh"], CultureInfo.InvariantCulture, out var time))
                continue;

            var daysUntilAppointment = ((int)dayOfWeek - (int)now.DayOfWeek + 7) % 7;

            var sessionDateTime = now.Date.AddDays(daysUntilAppointment);

            if (sessionDateTime < now)
                sessionDateTime = sessionDateTime.AddDays(7);

            while (sessionDateTime.Date <= lastDayOfMonth)
            {
                result.Add(createSession(sessionDateTime));
                sessionDateTime = sessionDateTime.AddDays(7);
            }
        }

        return [.. result.OrderBy(session => session.SessionDateTime)];
    }
}