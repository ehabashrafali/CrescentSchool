using CrescentSchool.Models.Enums;

namespace CrescentSchool.Models.Dtos;

public class SessionDto
{
    public Guid Id { get; set; }
    public DateTime StartTime { get; set; }
    public Guid? CourseId { get; set; }
    public Guid? InstructorId { get; set; }
    public Guid? StudentId { get; set; }
    public DateTime JoiningTime { get; set; }
    public DateTime CreatedAt { get; set; }
    public AttendanceStatus Status { get; set; }
    public string CourseName { get; set; } = string.Empty;
    public decimal CoursePricePerHoure { get; set; } = 0;
}
