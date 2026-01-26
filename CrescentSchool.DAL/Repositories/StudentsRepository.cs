using CrescentSchool.DAL.DbContext;
using CrescentSchool.Models;
using CrescentSchool.Models.Dtos;
using Microsoft.EntityFrameworkCore;

namespace CrescentSchool.DAL.Repositories;

public class StudentsRepository(ApplicationDbContext context) : IStudentsRepository
{
    public async Task<List<Student>> GetStudentsByIdsAsync(List<Guid> studentIds, CancellationToken cancellation = default)
    {
        var query = context.Students
            .Include(s => s.Instructor)
            .Where(s => s.IsActive);

        if (studentIds.Count == 0)
            return await query.ToListAsync(cancellation);

        return await query.Where(s => studentIds.Contains(s.Id)).ToListAsync(cancellation);
    }

    public async Task<Student?> GetStudentByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Students
            .Include(s => s.Instructor)
            .Include(s => s.WeeklyAppointments)
            .FirstOrDefaultAsync(s => s.Id == id && s.IsActive, cancellationToken);
    }

    public async Task AddMonthlyReport(Guid studentId, MonthlyReportDto studentMonthlyReportDto)
    {
        var student = await context.Students.
            Where(s => s.Id == studentId && s.IsActive)
            .Include(s => s.StudentMonthlyReports)
            .Include(s => s.Sessions)
            .FirstOrDefaultAsync();
        if (student is null)
            return;
        var monthlyReport = new StudentMonthlyReport
        {
            Date = DateTime.SpecifyKind(studentMonthlyReportDto.Date,
                                        DateTimeKind.Utc),
            Memorization = studentMonthlyReportDto.Memorization,
            Reading = studentMonthlyReportDto.Reading,
            NoOfMemorizationAyah = studentMonthlyReportDto.NoOfMemorizationAyah,
            NoOfReadingAyah = studentMonthlyReportDto.NoOfReadingAyah,
            Grade = studentMonthlyReportDto.Grade,
            BasicQuranRecitationRules = studentMonthlyReportDto.BasicQuranRecitationRules,
            TajweedRules = studentMonthlyReportDto.TajweedRules,
            Progress = studentMonthlyReportDto.Progress,
            QuranComments = studentMonthlyReportDto.QuranComments,
            IslamicStudiesComments = studentMonthlyReportDto.IslamicStudiesComments,
            IslamicStudiesTopics = studentMonthlyReportDto.IslamicStudiesTopics,
            IslamicStudiesBooks = studentMonthlyReportDto.IslamicStudiesBooks,
            IslamicStudiesProgress = studentMonthlyReportDto.IslamicStudiesProgress,
        };
        student.StudentMonthlyReports.Add(monthlyReport);
        await context.SaveChangesAsync();
    }

    public async Task<List<StudentMonthlyReport>> GetMonthlyReports(Guid id, CancellationToken cancellation)
        => await context.Students
            .Where(s => s.Id == id)
            .Select(s => s.StudentMonthlyReports)
            .FirstOrDefaultAsync(cancellation) ?? [];

}
