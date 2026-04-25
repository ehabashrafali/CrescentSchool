using CrescentSchool.Core.Extensions;
using CrescentSchool.DAL.DbContext;
using CrescentSchool.DAL.Dtos;
using Microsoft.EntityFrameworkCore;

namespace CrescentSchool.DAL.Repositories;

public class SessionsRepository(ApplicationDbContext context) : ISessionsRepository
{
    public async Task<Guid?> CreateSession(CreateSessionDto sessionDto, CancellationToken cancellationToken)
    {
        try
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
        catch (Exception)
        {
            throw new Exception("An error occurred while saving the entity changes");
        }
    }

    public async Task DeleteSessionAsync(Guid sessionId, CancellationToken cancellation)
    {
        var session = await context.Sessions.FindAsync([sessionId], cancellation);
        if (session is null)
            throw new NotFoundException("Session", sessionId);

        context.Sessions.Remove(session);
        await context.SaveChangesAsync(cancellation);
    }

    public Task<Session?> GetSessionByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return context.Sessions
              .Include(s => s.Instructor)
                .ThenInclude(i => i.User)
            .Include(s => s.Student)
                .ThenInclude(s => s.User)
            .FirstOrDefaultAsync(session => session.Id == id, cancellationToken);
    }

    public async Task<(List<Session>, int)> GetSessionsAsync(int? pageNumber, int? pageSize, CancellationToken cancellationToken)
    {
        IQueryable<Session> query = context.Sessions
            .AsNoTracking()
            .Include(s => s.Instructor)
                .ThenInclude(i => i.User)
            .Include(s => s.Student)
                .ThenInclude(st => st.User)
                .OrderByDescending(s => s.Date);
        var totalCount = await query.CountAsync(cancellationToken);

        if (pageNumber.HasValue && pageSize.HasValue && pageNumber > 0 && pageSize > 0)
        {
            var skip = (pageNumber.Value - 1) * pageSize.Value;
            query = query.Skip(skip).Take(pageSize.Value);
        }

        return (await query.ToListAsync(cancellationToken), totalCount);
    }

    public async Task<List<Session>> GetSessionsByInstructorIdAndDateAsync(Guid id, DateTimeOffset date, CancellationToken cancellationToken)
    {
        var month = date.Month;
        var year = date.Year;
        return await context.Sessions
            .Include(s => s.Instructor)
                .ThenInclude(i => i.User)
            .Include(s => s.Student)
                .ThenInclude(s => s.User)
            .Where(
                    s => s.Instructor.Id == id &&
                    s.Date.Month == month &&
                    s.Date.Year == year)
            .ToListAsync(cancellationToken);
    }

    public Task<List<Session>> GetSessionsByInstructorIdAsync(Guid instructorId, CancellationToken cancellationToken)
         => context.Sessions
                    .Where(s => s.InstructorId == instructorId)
                    .Include(s => s.Instructor)
                        .ThenInclude(i => i.User)
                    .Include(s => s.Student)
                        .ThenInclude(s => s.User)
                    .ToListAsync(cancellationToken);

    public async Task<List<Session>> GetSessionsByStudentIdAndDateAsync(Guid studentId, DateTimeOffset date, CancellationToken cancellationToken)
    {
        var month = date.Month;
        var year = date.Year;
        return await context.Sessions
            .Include(s => s.Instructor)
                .ThenInclude(i => i.User)
            .Include(s => s.Student)
                .ThenInclude(s => s.User)
            .Where(
                    s => s.Student.Id == studentId &&
                    s.Date.Month == month &&
                    s.Date.Year == year)
            .ToListAsync(cancellationToken);
    }

    public Task<List<Session>> GetSessionsByStudentIdAsync(Guid studentId, CancellationToken cancellationToken = default)
            => context.Sessions
                    .Where(s => s.Student.Id == studentId)
                    .Include(s => s.Instructor)
                        .ThenInclude(i => i.User)
                    .Include(s => s.Student)
                        .ThenInclude(s => s.User)
                    .ToListAsync(cancellationToken);

    public async Task UpdateSession(Session session, CancellationToken cancellationToken)
    {
        context.Update(session);
        await context.SaveChangesAsync(cancellationToken);
    }
}
