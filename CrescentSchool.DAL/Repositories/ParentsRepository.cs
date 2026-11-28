using CrescentSchool.DAL.DbContext;
using CrescentSchool.DAL.Dtos;
using CrescentSchool.DAL.Entities;
using CrescentSchool.Models.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CrescentSchool.DAL.Repositories;

public interface IParentsRepository
{
    Task<Guid> CreateParentAsync(CreateParentDto dto, CancellationToken cancellationToken);
    Task<List<Parent>> GetParentsAsync(CancellationToken cancellationToken);
}

public class ParentsRepository(ApplicationDbContext context, UserManager<ApplicationUser> userManager) : IParentsRepository
{
    public async Task<Guid> CreateParentAsync(CreateParentDto dto, CancellationToken cancellationToken)
    {
        using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);

        var students = await context.Students
            .Where(s => dto.StudentIds.Contains(s.Id))
            .ToListAsync(cancellationToken);

        if (students.Count != dto.StudentIds.Distinct().Count())
            throw new InvalidOperationException("One or more students were not found");

        var user = new ApplicationUser
        {
            Id = Guid.NewGuid().ToString(),
            UserName = dto.Email,
            Email = dto.Email,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            PhoneNumber = dto.PhoneNumber,
            EmailConfirmed = true,
            IsActive = true
        };

        var result = await userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
            throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));

        await userManager.AddToRoleAsync(user, nameof(Role.Parent));

        var parent = new Parent
        {
            Id = Guid.Parse(user.Id),
            Email = dto.Email,
            User = user,
            Students = students
        };
        context.Parents.Add(parent);

        await context.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);

        return parent.Id;
    }

    public async Task<List<Parent>> GetParentsAsync(CancellationToken cancellationToken)
    {
        return await context.Parents
            .Include(p => p.User)
            .Include(p => p.Students)
                .ThenInclude(s => s.User)
            .Include(p => p.Students)
                .ThenInclude(s => s.WeeklyAppointments)
            .ToListAsync(cancellationToken);
    }
}
