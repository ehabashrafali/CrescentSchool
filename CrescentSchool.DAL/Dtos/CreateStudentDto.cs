using System.ComponentModel.DataAnnotations;

namespace CrescentSchool.DAL.Dtos;

public class CreateStudentDto
{
    [Required]
    public Guid InstructorId { get; set; }
    [Required]
    public decimal Fees { get; set; }
    [Required]
    public string FirstName { get; set; } = string.Empty;
    public string? LastName { get; set; }
    public string Country { get; set; } = string.Empty;
    public DateOnly? DateOfBirth { get; set; }
    public string? PhoneNumber { get; set; }
    [Required]
    public string Email { get; set; } = string.Empty;
    [Required]
    public string Password { get; set; }
    public List<WeeklyAppointmentDto> WeeklyAppointments { get; set; } = [];

    public string ZoomLink = string.Empty;
}
