using Microsoft.AspNetCore.Identity;

namespace CrescentSchool.API.Entities;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string FullName => FirstName + ' ' + LastName;
    public bool IsActive { get; set; }
    public DateOnly? DateOfBirth { get; set; }
}
