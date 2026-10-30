using CrescentSchool.BLL.Services;
using CrescentSchool.Core.Options;
using CrescentSchool.DAL.Dtos;
using CrescentSchool.DAL.Repositories;
using Microsoft.Extensions.Options;
using System.Globalization;

namespace CrescentSchool.BLL.Jobs;

public class CreateMonthlyUpcommingSessionsJob(IStudentsRepository studentsRepository, ISessionsRepository sessionsRepository, IZoomService zoomService, IOptions<ZoomOptions> zoomOptions)
{
    public const string Name = "CreateMonthlyUpcommingSessionsJob";

    private static readonly string[] TimeFormats =
    [
        @"hh\.mm",
        @"h\.mm",
        @"hh\:mm",
        @"h\:mm",
        @"%h",
        @"hh"
    ];

    public async Task Execute(object? state, CancellationToken cancellationToken = default)
    {
        var now = DateTime.Now;
        var nextMonth = new DateTime(now.Year, now.Month, 1).AddMonths(1);

        var students = await studentsRepository.GetStudentsByIdsAsync([]);
        var upcomingSessions = new List<TimeTableSessionDto>();

        foreach (var student in students)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var studentName = student.User.FullName;
            var instructorName = student.Instructor?.User.FullName ?? string.Empty;

            foreach (var appointment in student.WeeklyAppointments)
            {
                if (!Enum.TryParse<DayOfWeek>(appointment.Day, ignoreCase: true, out var dayOfWeek) ||
                    !Enum.IsDefined(typeof(DayOfWeek), dayOfWeek))
                    continue;

                if (!TimeSpan.TryParseExact(appointment.Time?.Trim(), TimeFormats, CultureInfo.InvariantCulture, out var time) ||
                    time < TimeSpan.Zero || time >= TimeSpan.FromDays(1))
                    continue;

                var daysUntilAppointment = ((int)dayOfWeek - (int)now.DayOfWeek + 7) % 7;

                var sessionDateTime = now.Date.AddDays(daysUntilAppointment).Add(time);

                if (sessionDateTime < now)
                    sessionDateTime = sessionDateTime.AddDays(7);

                for (; sessionDateTime < nextMonth; sessionDateTime = sessionDateTime.AddDays(7))
                {
                    upcomingSessions.Add(new TimeTableSessionDto
                    {
                        SessionDateTime = sessionDateTime,
                        ZoomMeeting = string.Empty,
                        StudentName = studentName,
                        InstructorName = instructorName
                    });
                }
            }
        }
        foreach (var session in upcomingSessions)
        {
            cancellationToken.ThrowIfCancellationRequested();

            session.ZoomMeeting = await zoomService.CreateMeetingAsync(
                topic: $"Lesson - {session.StudentName} - {session.InstructorName} - {session.SessionDateTime.ToString()}",
                startTime: session.SessionDateTime,
                cancellationToken: cancellationToken);
        }

        if (upcomingSessions.Count > 0)
            await sessionsRepository.CreateUpCommingSessions(upcomingSessions, cancellationToken);
    }

    private async Task<string> CreateZoomMeetingLink()
    {
        throw new NotImplementedException();
    }
}