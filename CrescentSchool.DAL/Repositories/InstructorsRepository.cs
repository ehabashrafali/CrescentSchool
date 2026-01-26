using CrescentSchool.DAL.DbContext;
using CrescentSchool.Models;
using Microsoft.EntityFrameworkCore;

namespace CrescentSchool.DAL.Repositories
{
    public class InstructorsRepository(ApplicationDbContext context) : IInstructorsRepository
    {
        public async Task<Instructor?> GetByIdAsync(Guid instructorId)
        {
            return await context.Instructors
                .Where(i => i.IsActive)
                .FirstOrDefaultAsync(i => i.Id == instructorId);
        }

        public async Task<Instructor?> GetInstructorByIdAsync(Guid instructorId)
        {
            return await context.Instructors
                .Include(i => i.Students)
                .Include(i => i.Courses)
                .Where(i => i.IsActive)
                .FirstOrDefaultAsync(i => i.Id == instructorId);
        }

        public async Task<List<Instructor>> GetInstructorsAsync(List<Guid> instructorIds)
        {
            var query = context.Instructors.Where(i => i.IsActive)
                .Include(i => i.Students)
                .Include(i => i.Courses);

            if (instructorIds.Count == 0)
                return await query.ToListAsync();

            return await query.Where(i => instructorIds.Contains(i.Id)).ToListAsync();
        }

        public Task<List<Instructor>> GetInstructorsAsync(List<Guid> instructorIds, CancellationToken cancellationToken = default)
        {
            var query = context.Instructors.Where(i => i.IsActive);

            if (instructorIds.Count == 0)
                return query.ToListAsync(cancellationToken);

            return query.Where(i => instructorIds.Contains(i.Id)).ToListAsync(cancellationToken);
        }

        public async Task<List<Student>> GetInstuctorStudents(Guid instructorId)
        {
            return await context.Instructors
                        .Where(i => i.IsActive && i.Id == instructorId)
                        .SelectMany(i => i.Students)
                        .Include(s => s.StudentMonthlyReports)
                        .ToListAsync();
        }
    }
}
