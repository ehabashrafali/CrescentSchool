using System.ComponentModel.DataAnnotations;

namespace CrescentSchool.DAL.Dtos;

public class CreateInstructorDto
{
    [Required]
    public decimal Fees { get; set; }
    [Required]
    public string Password { get; set; } = string.Empty;
    [Required]
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string? LastName { get; set; }
    public string Country { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
}
