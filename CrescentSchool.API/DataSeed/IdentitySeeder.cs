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

        if (await userManager.FindByEmailAsync("ehabashrafali88@gmail.com") is null)
        {
            var adminUser = new ApplicationUser
            {
                UserName = "EhabAshraf",
                Email = "ehabashrafali88@gmail.com",
                FirstName = "Ehab",
                LastName = "Ashraf",
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(adminUser, "Opaa0100@1234");
            if (result.Succeeded)
                await userManager.AddToRoleAsync(adminUser, nameof(Roles.Admin));
        }
    }

}
