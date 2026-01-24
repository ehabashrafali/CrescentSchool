using CrescentSchool.DAL.DbContext;
using CrescentSchool.Models;
using CrescentSchool.Models.Dtos;
using Microsoft.EntityFrameworkCore;

namespace CrescentSchool.DAL.Repositories;

public class SessionsRepository(ApplicationDbContext context) : ISessionsRepository
{
    public async Task<Guid?> CreateSession(Guid studentId, SessionDto sessionDto, CancellationToken cancellationToken)
    {
        var student = await context.Students.
              Where(s => s.Id == studentId && s.IsActive)
              .Include(s => s.StudentMonthlyReports)
              .FirstOrDefaultAsync(cancellationToken: cancellationToken);

        if (student is null)
            return null;

        var session = new Session(sessionDto.StartTime, null, sessionDto.CourseId, sessionDto.InstructorId, student);
        var delayedTime = (int)(sessionDto.JoiningTime - sessionDto.StartTime).TotalMinutes;
        session.SetStatus(delayedTime);
        context.Sessions.Add(session);
        await context.SaveChangesAsync(cancellationToken);
        return session.Id;
    }

    public Task<List<SessionDto>> GetSessionsByInstructorIdAndDateRangeAsync(Guid id, DateTimeOffset date, CancellationToken cancellationToken)
       => context.Sessions.Where(s => s.InstructorId == id)
             .Include(s => s.Course)
               .Select(s => new SessionDto
               {
                   Id = s.Id,
                   StartTime = s.StartTime,
                   CreatedAt = s.CreatedAt,
                   CourseId = s.Course != null ? s.Course.Id : null,
                   InstructorId = id,
                   CourseName = s.Course != null ? s.Course.Name : string.Empty,
                   CoursePricePerHoure = s.Course != null ? s.Course.PricePerHour : 0,
               })
                .ToListAsync(cancellationToken);

    public Task<List<SessionDto>> GetSessionsByInstructorIdAsync(Guid instructorId, CancellationToken cancellationToken)
  => context.Sessions
                    .Where(s => s.InstructorId == instructorId)
                    .Include(s => s.Course)
                    .Include(s => s.Instructor)
                    .Select(s => new SessionDto
                    {
                        Id = s.Id,
                        StartTime = s.StartTime,
                        CreatedAt = s.CreatedAt,
                        CourseId = s.Course != null ? s.Course.Id : null,
                        InstructorId = instructorId,
                        CourseName = s.Course != null ? s.Course.Name : string.Empty,
                        CoursePricePerHoure = s.Course != null ? s.Course.PricePerHour : 0,
                    })
                    .ToListAsync(cancellationToken);

    public async Task<List<SessionDto>> GetSessionsByStudentIdAndDateRangeAsync(Guid studentId, DateTimeOffset date, CancellationToken cancellationToken)
    {
        var month = date.Month;
        var year = date.Year;
        return await context.Sessions
            .Include(s => s.Course)
            .Include(s => s.Instructor)
            .Where(
            s => s.Student.Id == studentId &&
            s.CreatedAt.Month == month &&
            s.CreatedAt.Year == year)
            .Select(s => new SessionDto
            {
                Id = s.Id,
                StartTime = s.StartTime,
                CreatedAt = s.CreatedAt,
                Status = s.Status,
                JoiningTime = s.CreatedAt,
                CourseId = s.Course != null ? s.Course.Id : null,
                InstructorId = s.Instructor != null ? s.Instructor.Id : null,
                CourseName = s.Course != null ? s.Course.Name : string.Empty,
                CoursePricePerHoure = s.Course != null ? s.Course.PricePerHour : 0,
            })
            .ToListAsync(cancellationToken);
    }

    public Task<List<SessionDto>> GetSessionsByStudentIdAsync(Guid studentId, CancellationToken cancellationToken = default)
            => context.Sessions
                    .Where(s => s.Student.Id == studentId)
                    .Include(s => s.Course)
                    .Include(s => s.Instructor)
                    .Select(s => new SessionDto
                    {
                        Id = s.Id,
                        StartTime = s.StartTime,
                        CreatedAt = s.CreatedAt,
                        CourseId = s.Course != null ? s.Course.Id : null,
                        InstructorId = s.Instructor != null ? s.Instructor.Id : null,
                        CourseName = s.Course != null ? s.Course.Name : string.Empty,
                        CoursePricePerHoure = s.Course != null ? s.Course.PricePerHour : 0,
                    })
                    .ToListAsync(cancellationToken);
}
