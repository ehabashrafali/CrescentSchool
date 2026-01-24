using CrescentSchool.BLL.DTOs;
using CrescentSchool.BLL.Interfaces;
using CrescentSchool.DAL.Repositories;
using CrescentSchool.Models;
using CrescentSchool.Models.Dtos;

namespace CrescentSchool.BLL.Services;

public class StudentsService(IStudentsRepository studentsRepository, ISessionsRepository sessionsRepository) : IStudentService
{
    public async Task<StudentDto?> GetStudentByIdAsync(Guid studentId, CancellationToken cancellationToken = default)
    {
        var student = await studentsRepository.GetStudentByIdAsync(studentId, cancellationToken);
        if (student is null)
            return null;

        return new StudentDto
        {
            Id = student.Id,
            FirstName = student.FirstName,
            LastName = student.LastName,
            Email = student.Email,
            Country = student.Country,
            PhoneNumber = student.PhoneNumber,
            ZoomMeeting = student.ZoomMeeting,
            DateOfBirth = student.DateOfBirth,
            IsActive = student.IsActive,
            MonthlyReportDtos = [.. student.StudentMonthlyReports.Select(r => new MonthlyReportDto
            {
                Id = r.Id,
                Date = r.Date,
                Memorization = r.Memorization,
                Reading = r.Reading,
                NoOfMemorizationAyah = r.NoOfMemorizationAyah,
                NoOfReadingAyah = r.NoOfReadingAyah,
                Grade = r.Grade,
                BasicQuranRecitationRules = r.BasicQuranRecitationRules,
                TajweedRules = r.TajweedRules,
                Progress = r.Progress,
                QuranComments = r.QuranComments,
                IslamicStudiesComments = r.IslamicStudiesComments,
                IslamicStudiesTopics = r.IslamicStudiesTopics,
                IslamicStudiesBooks = r.IslamicStudiesBooks,
                IslamicStudiesProgress = r.IslamicStudiesProgress
            })],
            WeeklyAppointments = [.. student.WeeklyAppointments.Select(wa => new WeeklyAppointmentDto
            {
                DayOfWeek = wa.Day.ToUpper(),
                Time = wa.Time,
            })]
        };
    }
    public async Task AddMonthlyReport(Guid studentId, MonthlyReportDto studentMonthlyReportDto)
        => await studentsRepository.AddMonthlyReport(studentId, studentMonthlyReportDto);
    public async Task<List<StudentDto>> GetStudentsAsync(List<Guid> studentIds, CancellationToken cancellationToken)
    {
        var students = await studentsRepository.GetStudentsByIdsAsync(studentIds, cancellationToken);
        return [.. students.Select(student => new StudentDto
        {
            Id = student.Id,
            FirstName = student.FirstName,
            LastName = student.LastName,
            Email = student.Email,
            Country = student.Country,
            PhoneNumber = student.PhoneNumber,
            ZoomMeeting = student.ZoomMeeting,
            DateOfBirth = student.DateOfBirth,
            IsActive = student.IsActive,
            WeeklyAppointments = [.. student.WeeklyAppointments.Select(wa => new WeeklyAppointmentDto
            {
                DayOfWeek = wa.Day.ToUpper(),
                Time = wa.Time,
            })]

        })];
    }
    public async Task<List<MonthlyReportDto>> GetMonthlyReportsAsync(Guid id, CancellationToken cancellation = default)
    {
        var montlyReports = await studentsRepository.GetMonthlyReports(id, cancellation);

        return [.. montlyReports.Select(r => new MonthlyReportDto
        {
            Id = r.Id,
            Date = r.Date,
            Memorization = r.Memorization,
            Reading = r.Reading,
            NoOfMemorizationAyah = r.NoOfMemorizationAyah,
            NoOfReadingAyah = r.NoOfReadingAyah,
            Grade = r.Grade,
            BasicQuranRecitationRules = r.BasicQuranRecitationRules,
            TajweedRules = r.TajweedRules,
            Progress = r.Progress,
            QuranComments = r.QuranComments,
            IslamicStudiesComments = r.IslamicStudiesComments,
            IslamicStudiesTopics = r.IslamicStudiesTopics,
            IslamicStudiesBooks = r.IslamicStudiesBooks,
            IslamicStudiesProgress = r.IslamicStudiesProgress
        }
        )];

    }
    public async Task<List<UpcomingSessionDto>> GetUpcomingSessionsDtoAsync(
        Guid id,
        DateTime? startDate = null,
        DateTime? endDate = null,
        CancellationToken cancellationToken = default)
    {
        var student = await studentsRepository.GetStudentByIdAsync(id, cancellationToken);
        if (student is null)
            return default!;

        var now = DateTime.Now;
        var currentMonth = now.Month;
        var result = new List<UpcomingSessionDto>();
        var year = now.Year;

        var firstDayOfMonth = new DateTime(year, currentMonth, 1);
        var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

        foreach (var appointment in student.WeeklyAppointments)
        {

            if (!Enum.TryParse<DayOfWeek>(appointment.Day, true, out var dayOfWeek))
                continue;

            if (!TimeSpan.TryParse(appointment.Time, out var time))
                continue;
            for (var date = firstDayOfMonth; date <= lastDayOfMonth; date = date.AddDays(1))
            {
                if (date.DayOfWeek == dayOfWeek)
                {
                    var sessionDateTime = date.Date + time;

                    if (sessionDateTime < now)
                        continue;

                    result.Add(new UpcomingSessionDto
                    {
                        SessionDateTime = sessionDateTime,
                        ZoomMeeting = student.ZoomMeeting,
                        InstructorName = [.. student.Instructors.Select(i => i.FullName)],
                        CourseName = [.. student.Courses.Select(c => c.Name)]

                    });
                }
            }
        }
        return result;
    }


}
