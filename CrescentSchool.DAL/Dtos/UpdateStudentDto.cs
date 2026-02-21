using System.ComponentModel.DataAnnotations;

namespace CrescentSchool.DAL.Dtos;

public class UpdateStudentDto
{
    [Required]
    public string FirstName { get; set; } = string.Empty;
    [Required]
    public string Email { get; set; } = string.Empty;
    [Required]
    public decimal Fees { get; set; }
    public string LastName { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;

    public string ZoomLink { get; set; } = string.Empty;
    public List<WeeklyAppointmentDto> WeeklyAppointment { get; set; } = [];
}