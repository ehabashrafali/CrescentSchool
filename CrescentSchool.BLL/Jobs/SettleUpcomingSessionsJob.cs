using CrescentSchool.DAL.Entities;
using CrescentSchool.DAL.Repositories;
using CrescentSchool.Models.Enums;

namespace CrescentSchool.BLL.Jobs;

public class SettleUpcomingSessionsJob(ISessionsRepository sessionsRepository)
{
    public static string Name => "SettleUpcomingSessionsJob";
    public async Task Execute(object? state, CancellationToken cancellationToken = default)
    {
        var upComingSessions = await sessionsRepository.GetUpComingSessionsToSettleAsync(cancellationToken);

        var sessions = upComingSessions.Select(s => new Session
        (
            instructorId: s.InstructorId,
            studentId: s.StudentId,
            sessionDuration: s.TotalDuration > 30 ? (SessionDuration)s.TotalDuration : SessionDuration.ThirtyMinutes,
            date: s.SessionDateTime,
            studentSessionStatus: s.Canceled.HasValue
                                  && s.Canceled.Value == StudentAttendanceStatus.Cancelled ?
                StudentAttendanceStatus.Cancelled : s.Canceled.HasValue && s.Canceled.Value == StudentAttendanceStatus.CancelledByInstructor ?
                    StudentAttendanceStatus.CancelledByInstructor : StudentAttendanceStatus.Attend,

            instructorSessionStatus: s.Canceled.HasValue
                                     && s.Canceled.Value == StudentAttendanceStatus.Cancelled ?
                InstructorAttendanceStatus.Attend : s.Canceled.HasValue && s.Canceled.Value == StudentAttendanceStatus.CancelledByInstructor ?
                    InstructorAttendanceStatus.Cancelled : InstructorAttendanceStatus.Attend
        )).ToList();

        await sessionsRepository.AddSessions(sessions, cancellationToken);
    }
}