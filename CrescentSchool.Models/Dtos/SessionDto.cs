using CrescentSchool.Models.Enums;
using System.Text.Json.Serialization;

namespace CrescentSchool.Models.Dtos;

public class SessionDto
{
    [JsonIgnore] public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public Guid InstructorId { get; set; }
    public Guid StudentId { get; set; }
    public AttendanceStatus StudentSessionStatus { get; set; }
    public AttendanceStatus InstructorSessionStatus { get; set; }
}
