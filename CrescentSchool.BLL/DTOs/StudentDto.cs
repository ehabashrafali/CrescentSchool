using CrescentSchool.BLL.Enums;
using CrescentSchool.DAL.Dtos;

namespace CrescentSchool.BLL.DTOs;

public class StudentDto
{
    public StudentDto()
    {

    }
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string FullName => $"{FirstName} {LastName}";
    public string ZoomMeeting { get; set; } = string.Empty;
    public DateOnly? DateOfBirth { get; set; }
    public int Age => DateOfBirth.HasValue ? DateTime.Now.Year - DateOfBirth.Value.Year : 0;
    public bool IsActive { get; set; }
    public List<MonthlyReportDto> MonthlyReportDtos { get; set; }
    public List<WeeklyAppointmentDto> WeeklyAppointments { get; set; }
    public decimal Fees { get; set; }
    public string Role => Roles.Student.ToString();
}
