using CrescentSchool.DAL.DbContext;
using CrescentSchool.Models;
using CrescentSchool.Models.Dtos;
using Microsoft.EntityFrameworkCore;

namespace CrescentSchool.DAL.Repositories;

public class SessionsRepository(ApplicationDbContext context) : ISessionsRepository
{
    public async Task<Guid?> CreateSession(SessionDto sessionDto, CancellationToken cancellationToken)
    {
        var session = new Session(sessionDto.Date.Kind == DateTimeKind.Unspecified
                                    ? DateTime.SpecifyKind(sessionDto.Date, DateTimeKind.Utc)
                                    : sessionDto.Date.ToUniversalTime(),
                                  sessionDto.StudentId,
                                  sessionDto.InstructorId,
                                  sessionDto.StudentSessionStatus,
                                  sessionDto.InstructorSessionStatus);
        await context.Sessions.AddAsync(session, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return session.Id;
    }

    public Task<List<SessionDto>> GetSessionsByInstructorIdAndDateRangeAsync(Guid id, DateTimeOffset date, CancellationToken cancellationToken)
       => context.Sessions.Where(s => s.InstructorId == id)
               .Select(s => new SessionDto
               {
                   Id = s.Id,
                   Date = s.Date,
                   InstructorSessionStatus = s.InstructorStatus,
                   StudentSessionStatus = s.StudentStatus,
                   StudentId = s.Student.Id,
                   InstructorId = s.InstructorId,
                   Duration = s.Duration
               })
                .ToListAsync(cancellationToken);

    public Task<List<SessionDto>> GetSessionsByInstructorIdAsync(Guid instructorId, CancellationToken cancellationToken)
  => context.Sessions
                    .Where(s => s.InstructorId == instructorId)
                    .Select(s => new SessionDto
                    {
                        Id = s.Id,
                        Date = s.Date,
                        InstructorSessionStatus = s.InstructorStatus,
                        StudentSessionStatus = s.StudentStatus,
                        StudentId = s.Student.Id,
                        InstructorId = s.InstructorId,
                        Duration = s.Duration
                    })
                    .ToListAsync(cancellationToken);

    public async Task<List<SessionDto>> GetSessionsByStudentIdAndDateRangeAsync(Guid studentId, DateTimeOffset date, CancellationToken cancellationToken)
    {
        var month = date.Month;
        var year = date.Year;
        return await context.Sessions
            .Include(s => s.Instructor)
            .Where(
            s => s.Student.Id == studentId &&
            s.Date.Month == month &&
            s.Date.Year == year)
            .Select(s => new SessionDto
            {
                Id = s.Id,
                Date = s.Date,
                InstructorSessionStatus = s.InstructorStatus,
                StudentSessionStatus = s.StudentStatus,
                StudentId = s.Student.Id,
                InstructorId = s.InstructorId,
                Duration = s.Duration
            })
            .ToListAsync(cancellationToken);
    }

    public Task<List<SessionDto>> GetSessionsByStudentIdAsync(Guid studentId, CancellationToken cancellationToken = default)
            => context.Sessions
                    .Where(s => s.Student.Id == studentId)
                    .Include(s => s.Instructor)
                    .Select(s => new SessionDto
                    {
                        Id = s.Id,
                        Date = s.Date,
                        InstructorSessionStatus = s.InstructorStatus,
                        StudentSessionStatus = s.StudentStatus,
                        StudentId = s.Student.Id,
                        InstructorId = s.InstructorId,
                        Duration = s.Duration
                    })
                    .ToListAsync(cancellationToken);
}
