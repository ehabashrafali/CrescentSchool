using CrescentSchool.API.Entities;
using CrescentSchool.BLL.Enums;
using CrescentSchool.DAL.DbContext;
using CrescentSchool.DAL.Dtos;
using CrescentSchool.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CrescentSchool.DAL.Repositories;

public class StudentsRepository(ApplicationDbContext context, UserManager<ApplicationUser> _userManager) : IStudentsRepository
{
    public async Task<List<Student>> GetStudentsByIdsAsync(List<Guid> studentIds, CancellationToken cancellation = default)
    {
        var query = context.Students
            .Include(s => s.Instructor);

        if (studentIds.Count == 0)
            return await query.ToListAsync(cancellation);

        return await query.Where(s => studentIds.Contains(s.Id)).ToListAsync(cancellation);
    }
    public async Task<Student?> GetStudentByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Students
            .Include(s => s.Instructor)
            .Include(s => s.WeeklyAppointments)
            .FirstOrDefaultAsync(s => s.Id == id && s.User.IsActive, cancellationToken);
    }
    public async Task AddMonthlyReport(Guid studentId, MonthlyReportDto studentMonthlyReportDto)
    {
        var student = await context.Students
            .Where(s => s.Id == studentId && s.User.IsActive)
            .Include(s => s.StudentMonthlyReports)
                .ThenInclude(r => r.IslamicStudiesBooks)
            .Include(s => s.StudentMonthlyReports)
                .ThenInclude(r => r.TajweedRules)
            .Include(s => s.StudentMonthlyReports)
                .ThenInclude(r => r.BasicQuranRecitationRules)
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
            MemorizationGrade = studentMonthlyReportDto.MemorizationGrade,
            ReadingGrade = studentMonthlyReportDto.ReadingGrade,
            NoOfReadingAyah = studentMonthlyReportDto.NoOfReadingAyah,
            BasicQuranRecitationRulesProgress = studentMonthlyReportDto.BasicQuranRecitationRulesProgress,
            BasicQuranRecitationRules = studentMonthlyReportDto.BasicQuranRecitationRules,
            TajweedRules = studentMonthlyReportDto.TajweedRules,
            TajweedRulesProgress = studentMonthlyReportDto.TajweedRulesProgress,
            QuranComments = studentMonthlyReportDto.QuranComments,
            IslamicStudiesComments = studentMonthlyReportDto.IslamicStudiesComments,
            IslamicStudiesTopics = studentMonthlyReportDto.IslamicStudiesTopics,
            IslamicStudiesBooks = studentMonthlyReportDto.IslamicStudiesBooks,
            IslamicStudiesProgress = studentMonthlyReportDto.IslamicStudiesProgress,
            OthersIslamicStudiesBooks = studentMonthlyReportDto.OthersIslamicStudiesBooks
        };
        student.StudentMonthlyReports.Add(monthlyReport);
        await context.SaveChangesAsync();
    }
    public async Task<List<StudentMonthlyReport>> GetMonthlyReports(Guid id, CancellationToken cancellation)
        => await context.Students
            .Where(s => s.Id == id)
            .Select(s => s.StudentMonthlyReports)
            .FirstOrDefaultAsync(cancellation) ?? [];
    public async Task<StudentMonthlyReport?> GetCurrentMonthlyReport(Guid studentId, CancellationToken cancellationToken)
    {
        var nowUtc = DateTime.UtcNow;

        var monthStart = new DateTime(
            nowUtc.Year,
            nowUtc.Month,
            1,
            0, 0, 0,
            DateTimeKind.Utc);

        var monthEnd = monthStart.AddMonths(1);

        return await context.StudentMonthlyReports
            .Where(r =>
                r.Student.Id == studentId &&
                r.Date >= monthStart &&
                r.Date < monthEnd)
            .Include(r => r.IslamicStudiesBooks)
            .Include(r => r.TajweedRules)
            .Include(r => r.BasicQuranRecitationRules)
            .FirstOrDefaultAsync(cancellationToken);
    }
    public Task DeactivateStudent(Guid id, CancellationToken cancellationToken)
    {
        var student = context.Students.FirstOrDefault(s => s.Id == id);
        if (student is not null)
        {
            student.User.IsActive = false;
            return context.SaveChangesAsync(cancellationToken);
        }
        return Task.CompletedTask;
    }
    public async Task UpdateStudent(Student student, CancellationToken cancellationToken)
    {
        context.Update(student);
        await context.SaveChangesAsync(cancellationToken);
    }
    public async Task<Guid> CreateStudentAsync(CreateStudentDto dto, CancellationToken cancellationToken)
    {
        using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            var user = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = dto.Email,
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName ?? GenerateRandomString(),
                Country = dto.Country,
                PhoneNumber = dto.PhoneNumber,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));

            await _userManager.AddToRoleAsync(user, nameof(Roles.Student));

            var student = new Student
            {
                Id = new Guid(user.Id),
                InstructorId = dto.InstructorId,
                Fees = dto.Fees,
                WeeklyAppointments = dto.WeeklyAppointments ?? []
            };

            context.Students.Add(student);

            await context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return student.Id;
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
    public static string GenerateRandomString(int length = 5)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var random = new Random();
        return new string([.. Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)])]);
    }
}
