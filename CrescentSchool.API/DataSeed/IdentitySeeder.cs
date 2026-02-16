using CrescentSchool.API.Entities;
using CrescentSchool.BLL.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace CrescentSchool.API.DataSeed;

public static class IdentitySeeder
{
    public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roles = new[] { nameof(Roles.Admin), nameof(Roles.Instructor), nameof(Roles.Supervisor), nameof(Roles.Parent), nameof(Roles.Student) };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }


        var superVisorUser = new ApplicationUser
        {
            UserName = "Supervisor",
            Email = "supervisor@crescentschool.com",
            FirstName = "Supervisor",
            EmailConfirmed = true
        };

        var supervisorResult = await userManager.CreateAsync(superVisorUser, "Crescent26!");

        if (supervisorResult.Succeeded)
            await userManager.AddToRoleAsync(superVisorUser, nameof(Roles.Supervisor));

    }
}
