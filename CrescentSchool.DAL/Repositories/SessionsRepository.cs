using CrescentSchool.DAL.DbContext;
using CrescentSchool.DAL.Dtos;
using Microsoft.EntityFrameworkCore;

namespace CrescentSchool.DAL.Repositories;

public class SessionsRepository(ApplicationDbContext context) : ISessionsRepository
{
    public async Task<Guid?> CreateSession(CreateSessionDto sessionDto, CancellationToken cancellationToken)
    {
        var session = new Session(sessionDto.Date.Kind == DateTimeKind.Unspecified
                                    ? DateTime.SpecifyKind(sessionDto.Date, DateTimeKind.Utc)
                                    : sessionDto.Date.ToUniversalTime(),
                                  sessionDto.StudentId,
                                  sessionDto.InstructorId,
                                  sessionDto.StudentSessionStatus,
                                  sessionDto.InstructorSessionStatus,
                                  sessionDto.Duration);
        await context.Sessions.AddAsync(session, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return session.Id;
    }

    public async Task DeleteSessionAsync(Session session, CancellationToken cancellation)
    {
        context.Sessions.Remove(session);
        await context.SaveChangesAsync(cancellation);
    }

    public Task<Session?> GetSessionByIdAsync(Guid id, CancellationToken cancellationToken)
        => context.Sessions.FirstOrDefaultAsync(session => session.Id == id, cancellationToken: cancellationToken);

    public Task<List<GetSessionDto>> GetSessionsAsync(CancellationToken cancellationToken)
    {
        return context.Sessions
                     .Include(s => s.Student)
                     .Include(s => s.Instructor)
                     .Select(s => new GetSessionDto
                     {
                         Id = s.Id,
                         Date = s.Date,
                         InstructorSessionStatus = s.InstructorStatus,
                         StudentSessionStatus = s.StudentStatus,
                         StudentId = s.Student.Id,
                         InstructorId = s.InstructorId,
                         Duration = s.Duration,
                         StudentName = s.Student.User.FullName,
                         InstructorName = s.Instructor.User.FullName
                     })
                     .ToListAsync(cancellationToken);
    }

    public Task<List<GetSessionDto>> GetSessionsByInstructorIdAndDateAsync(Guid id, DateTimeOffset date, CancellationToken cancellationToken)
       => context.Sessions.Where(s => s.InstructorId == id)
               .Select(s => new GetSessionDto
               {
                   Id = s.Id,
                   Date = s.Date,
                   InstructorSessionStatus = s.InstructorStatus,
                   StudentSessionStatus = s.StudentStatus,
                   StudentId = s.Student.Id,
                   InstructorId = s.InstructorId,
                   Duration = s.Duration,
                   StudentName = s.Student.User.FullName,
                   InstructorName = s.Instructor.User.FullName
               })
                .ToListAsync(cancellationToken);

    public Task<List<GetSessionDto>> GetSessionsByInstructorIdAsync(Guid instructorId, CancellationToken cancellationToken)
  => context.Sessions
                    .Where(s => s.InstructorId == instructorId)
                    .Select(s => new GetSessionDto
                    {
                        Id = s.Id,
                        Date = s.Date,
                        InstructorSessionStatus = s.InstructorStatus,
                        StudentSessionStatus = s.StudentStatus,
                        StudentId = s.Student.Id,
                        InstructorId = s.InstructorId,
                        Duration = s.Duration,
                        StudentName = s.Student.User.FullName,
                        InstructorName = s.Instructor.User.FullName
                    })
                    .ToListAsync(cancellationToken);

    public async Task<List<GetSessionDto>> GetSessionsByStudentIdAndDateAsync(Guid studentId, DateTimeOffset date, CancellationToken cancellationToken)
    {
        var month = date.Month;
        var year = date.Year;
        return await context.Sessions
            .Include(s => s.Instructor)
            .Where(
            s => s.Student.Id == studentId &&
            s.Date.Month == month &&
            s.Date.Year == year)
            .Select(s => new GetSessionDto
            {
                Id = s.Id,
                Date = s.Date,
                InstructorSessionStatus = s.InstructorStatus,
                StudentSessionStatus = s.StudentStatus,
                StudentId = s.Student.Id,
                InstructorId = s.InstructorId,
                Duration = s.Duration,
                StudentName = s.Student.User.FullName,
                InstructorName = s.Instructor.User.FullName
            })
            .ToListAsync(cancellationToken);
    }

    public Task<List<GetSessionDto>> GetSessionsByStudentIdAsync(Guid studentId, CancellationToken cancellationToken = default)
            => context.Sessions
                    .Where(s => s.Student.Id == studentId)
                    .Include(s => s.Instructor)
                    .Select(s => new GetSessionDto
                    {
                        Id = s.Id,
                        Date = s.Date,
                        InstructorSessionStatus = s.InstructorStatus,
                        StudentSessionStatus = s.StudentStatus,
                        StudentId = s.Student.Id,
                        InstructorId = s.InstructorId,
                        Duration = s.Duration,
                        StudentName = s.Student.User.FullName,
                        InstructorName = s.Instructor.User.FullName
                    })
                    .ToListAsync(cancellationToken);

    public async Task UpdateSession(Session session, CancellationToken cancellationToken)
    {
        context.Update(session);
        await context.SaveChangesAsync(cancellationToken);
    }
}
