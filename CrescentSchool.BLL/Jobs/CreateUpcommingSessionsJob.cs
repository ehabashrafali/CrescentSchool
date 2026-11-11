using CrescentSchool.BLL.Services;
using CrescentSchool.DAL.Dtos;
using CrescentSchool.DAL.Repositories;
using System.Globalization;

namespace CrescentSchool.BLL.Jobs;

public class CreateUpcommingSessionsJob(IStudentsRepository studentsRepository, ISessionsRepository sessionsRepository, IZoomService zoomService)
{
    public const string Name = "CreateUpcommingSessionsJob";

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
        var cairoTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Africa/Cairo");

        var now = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, cairoTimeZone);

        var endDate = now.AddDays(7);

        var students = await studentsRepository.GetStudentsByIdsAsync([], cancellationToken);


        var instructorZoomUsers = students
            .Where(student => student.Instructor is not null)
            .Select(student => student.Instructor!)
            .DistinctBy(instructor => instructor.Id)
            .ToDictionary(
                instructor => instructor.Id,
                instructor => instructor.ZoomUserId);

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

                for (; sessionDateTime < endDate; sessionDateTime = sessionDateTime.AddDays(7))
                {
                    upcomingSessions.Add(new TimeTableSessionDto
                    {
                        SessionDateTime = sessionDateTime,
                        ZoomMeeting = string.Empty,
                        StudentName = studentName,
                        InstructorName = instructorName,
                        StudentId = student.Id,
                        InstructorId = student.Instructor?.Id ?? Guid.Empty
                    });
                }
            }
        }

        foreach (var session in upcomingSessions)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var zoomUserId = instructorZoomUsers[session.InstructorId]!;

            if (string.IsNullOrWhiteSpace(zoomUserId))
                continue;

            var topic = $"Lesson - {session.StudentName} - {session.InstructorName} - {session.SessionDateTime:yyyy-MM-dd HH:mm}";

            var meeting = await zoomService.CreateMeetingAsync(hostUserId: zoomUserId, topic: topic, startTime: session.SessionDateTime, cancellationToken: cancellationToken);

            session.ZoomMeeting = meeting.JoinUrl;

            session.ZoomMeetingId = meeting.Id;
            session.Uuid = meeting.Uuid;

            await sessionsRepository.CreateUpCommingSessions([session], cancellationToken);
        }
    }
}