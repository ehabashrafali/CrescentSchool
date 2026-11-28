using System.ComponentModel.DataAnnotations;

namespace CrescentSchool.DAL.Dtos;

public class CreateParentDto
{
    [Required]
    public string FirstName { get; set; } = string.Empty;
    [Required]
    public string LastName { get; set; } = string.Empty;
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;
    [Required]
    public string Password { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public List<Guid> StudentIds { get; set; } = [];
}
