using CrescentSchool.API.Entities;
using CrescentSchool.BLL.Enums;
using CrescentSchool.DAL.DbContext;
using CrescentSchool.DAL.Dtos;
using CrescentSchool.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CrescentSchool.DAL.Repositories
{
    public class InstructorsRepository(ApplicationDbContext context, UserManager<ApplicationUser> _userManager) : IInstructorsRepository
    {
        public async Task<Guid> CreateInstructorAsync(CreateInstructorDto createInstructorDto, CancellationToken cancellationToken)
        {
            using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
            var user = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = createInstructorDto.Email,
                Email = createInstructorDto.Email,
                FirstName = createInstructorDto.FirstName,
                LastName = string.IsNullOrWhiteSpace(createInstructorDto.LastName)
                            ? GenerateRandomString()
                            : createInstructorDto.LastName,
                Country = createInstructorDto.Country,
                PhoneNumber = createInstructorDto.PhoneNumber,
                EmailConfirmed = true,
                IsActive = true
            };

            var result = await _userManager.CreateAsync(user, createInstructorDto.Password);

            if (!result.Succeeded)
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));

            await _userManager.AddToRoleAsync(user, nameof(Roles.Instructor));
            var instructor = new Instructor
            {
                Id = new Guid(user.Id),
                Fees = createInstructorDto.Fees,
                User = user,
                ZoomMeeting = createInstructorDto.ZoomLink
            };
            context.Instructors.Add(instructor);

            await context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return instructor.Id;
        }
        public async Task<Instructor?> GetByIdAsync(Guid instructorId)
        {
            return await context.Instructors
                .Where(i => i.User.IsActive)
                .Include(i => i.User)
                .FirstOrDefaultAsync(i => i.Id == instructorId);
        }
        public async Task<Instructor?> GetInstructorByIdAsync(Guid instructorId)
        {
            return await context.Instructors
                .Include(i => i.Students)
                .Include(i => i.Courses)
                .Where(i => i.User.IsActive)
                .FirstOrDefaultAsync(i => i.Id == instructorId);
        }
        public async Task<List<Instructor>> GetInstructorsAsync(List<Guid> instructorIds)
        {
            var query = context.Instructors.Where(i => i.User.IsActive)
                .Include(i => i.Students)
                .Include(i => i.Courses);

            if (instructorIds.Count == 0)
                return await query.ToListAsync();

            return await query.Where(i => instructorIds.Contains(i.Id)).ToListAsync();
        }
        public Task<List<Instructor>> GetInstructorsAsync(List<Guid> instructorIds, CancellationToken cancellationToken = default)
        {
            var query = context.Instructors.Include(i => i.User);

            if (instructorIds.Count == 0)
                return query.ToListAsync(cancellationToken);

            return query.Where(i => instructorIds.Contains(i.Id)).ToListAsync(cancellationToken);
        }
        public async Task<List<Student>> GetInstuctorStudents(Guid instructorId)
        {
            return await context.Instructors
                        .Where(i => i.User.IsActive && i.Id == instructorId)
                        .SelectMany(i => i.Students)
                        .Include(s => s.StudentMonthlyReports)
                            .ThenInclude(r => r.IslamicStudiesBooks)
                        .Include(s => s.StudentMonthlyReports)
                            .ThenInclude(r => r.TajweedRules)
                        .Include(s => s.StudentMonthlyReports)
                            .ThenInclude(r => r.BasicQuranRecitationRules)
                        .ToListAsync();
        }
        public static string GenerateRandomString(int length = 5)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string([.. Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)])]);
        }
        public Task DeactivateInstructor(Guid id, CancellationToken cancellationToken)
        {
            var instructor = context.Instructors.Include(i => i.User).FirstOrDefault(s => s.Id == id);
            if (instructor is not null)
            {
                instructor.User.IsActive = false;
                return context.SaveChangesAsync(cancellationToken);
            }
            return Task.CompletedTask;
        }
    }
}
