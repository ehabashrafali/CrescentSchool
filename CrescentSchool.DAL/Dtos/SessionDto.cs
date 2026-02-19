using CrescentSchool.Models.Enums;
using System.Text.Json.Serialization;

namespace CrescentSchool.DAL.Dtos;

public class SessionDto
{
    public DateTime Date { get; set; }
    public Guid InstructorId { get; set; }
    public Guid StudentId { get; set; }
    public SessionDuration Duration { get; set; }
    public AttendanceStatus StudentSessionStatus { get; set; }
    public AttendanceStatus InstructorSessionStatus { get; set; }
    [JsonIgnore] public Guid Id { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string InstructorName { get; set; } = string.Empty;
}