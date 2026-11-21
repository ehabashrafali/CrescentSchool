using CrescentSchool.DAL.Entities;
using CrescentSchool.DAL.Repositories;
using CrescentSchool.Models.Enums;

namespace CrescentSchool.BLL.Jobs;

public class SettleUpcomingSessionsJob(ISessionsRepository sessionsRepository, TimeProvider timeProvider)
{
    public static string Name => "SettleUpcomingSessionsJob";

    public async Task Execute(object? state, CancellationToken cancellationToken = default)
    {
        var utcNow = timeProvider.GetUtcNow().UtcDateTime;
        var noAttendanceCutoff = utcNow.AddMinutes(-40);
        var lateThreshold = TimeSpan.FromMinutes(5);

        var upcomingSessions = await sessionsRepository.GetUpComingSessionsToSettleAsync(utcNow, cancellationToken);

        var sessions = upcomingSessions.Select(s =>
        {
            var (studentStatus, instructorStatus) = s.Canceled switch
            {
                StudentAttendanceStatus.CancelledByInstructor =>
                    (StudentAttendanceStatus.CancelledByInstructor,
                     InstructorAttendanceStatus.Cancelled),

                StudentAttendanceStatus.Cancelled =>
                    (StudentAttendanceStatus.Cancelled,
                     InstructorAttendanceStatus.Attend),

                _ when s.SessionDateTime <= noAttendanceCutoff
                    && s.FirstJoinedAt is null
                    && s.LastLeftAt is null =>
                    (StudentAttendanceStatus.CancelledByInstructor,
                     InstructorAttendanceStatus.Absent),

                _ when s.FirstJoinedAt is { } firstJoinedAt && firstJoinedAt - s.SessionDateTime > lateThreshold => (StudentAttendanceStatus.Attend, InstructorAttendanceStatus.Late),

                _ => (StudentAttendanceStatus.Attend, InstructorAttendanceStatus.Attend)
            };

            return new Session(instructorId: s.InstructorId, studentId: s.StudentId, sessionDuration: s.TotalDuration,
                                date: s.SessionDateTime,
                                studentSessionStatus: studentStatus,
                                instructorSessionStatus: instructorStatus);
        }).ToList();

        if (sessions.Count > 0)
        {
            await sessionsRepository.AddSessions(sessions, cancellationToken);
        }
    }
}