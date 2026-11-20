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
        var upComingSessions = await sessionsRepository.GetUpComingSessionsToSettleAsync(utcNow, cancellationToken);

        var sessions = upComingSessions.Select(s =>
        {
            var noAttendance = s.SessionDateTime <= noAttendanceCutoff
                && s.FirstJoinedAt is null && s.LastLeftAt is null;

            var studentStatus = noAttendance
                ? StudentAttendanceStatus.CancelledByInstructor
                : s.Canceled switch
                {
                    StudentAttendanceStatus.Cancelled => StudentAttendanceStatus.Cancelled,
                    StudentAttendanceStatus.CancelledByInstructor => StudentAttendanceStatus.CancelledByInstructor,
                    _ => StudentAttendanceStatus.Attend
                };
            var instructorStatus = noAttendance
                ? InstructorAttendanceStatus.Absent
                : s.Canceled == StudentAttendanceStatus.CancelledByInstructor
                    ? InstructorAttendanceStatus.Cancelled
                    : InstructorAttendanceStatus.Attend;

            return new Session(
                instructorId: s.InstructorId,
                studentId: s.StudentId,
                sessionDuration: s.TotalDuration > 30 ? (SessionDuration)s.TotalDuration : SessionDuration.ThirtyMinutes,
                date: s.SessionDateTime,
                studentSessionStatus: studentStatus,
                instructorSessionStatus: instructorStatus);
        }).ToList();

        if (sessions.Count > 0)
            await sessionsRepository.AddSessions(sessions, cancellationToken);
    }
}
