using CrescentSchool.API.Entities;
using CrescentSchool.BLL.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CrescentSchool.API.DataSeed;

public static class IdentitySeeder
{
    public static async Task SeedRolesAndUsersAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();

        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var environment = scope.ServiceProvider.GetRequiredService<IHostEnvironment>();

        await SeedRolesAsync(roleManager);
        await SeedSupervisorAsync(userManager);
        await SeedAdminAsync(userManager, environment);
    }

    private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
    {
        var roles = new[]
        {
            nameof(Roles.Admin),
            nameof(Roles.Instructor),
            nameof(Roles.Supervisor),
            nameof(Roles.Parent),
            nameof(Roles.Student)
        };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }

    private static async Task SeedSupervisorAsync(UserManager<ApplicationUser> userManager)
    {
        const string email = "supervisor@crescentschool.com";

        var existingUser = await userManager.FindByEmailAsync(email);
        if (existingUser != null)
            return;

        var supervisorUser = new ApplicationUser
        {
            UserName = email,
            Email = email,
            FirstName = "Supervisor",
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(supervisorUser, "Supervisor@Crescent26!");

        if (result.Succeeded)
            await userManager.AddToRoleAsync(supervisorUser, nameof(Roles.Supervisor));
    }

    private static async Task SeedAdminAsync(
        UserManager<ApplicationUser> userManager,
        IHostEnvironment environment)
    {
        const string email = "crescentschoolacademy@gmail.com";

        var existingUser = await userManager.FindByEmailAsync(email);
        if (existingUser != null)
            return;

        var adminUser = new ApplicationUser
        {
            UserName = email,
            Email = email,
            FirstName = "Osama",
            LastName = "Gamal",
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(adminUser, "Admin@Crescent26!");

        if (result.Succeeded)
            await userManager.AddToRoleAsync(adminUser, nameof(Roles.Admin));
    }
}