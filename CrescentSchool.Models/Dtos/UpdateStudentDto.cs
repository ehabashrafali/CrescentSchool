namespace CrescentSchool.Models.Dtos;

public class UpdateStudentDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public decimal Fees { get; set; }
    public List<WeeklyAppointmentDto> weeklyAppointment { get; set; } = [];
}
