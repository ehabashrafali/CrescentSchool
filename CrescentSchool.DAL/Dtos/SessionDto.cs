using CrescentSchool.Models.Enums;

namespace CrescentSchool.DAL.Dtos;

public class SessionDto
{
    public DateTime Date { get; set; }
    public Guid InstructorId { get; set; }
    public Guid StudentId { get; set; }
    public SessionDuration Duration { get; set; }
    public StudentAttendanceStatus StudentSessionStatus { get; set; }
    public InstructorAttendanceStatus InstructorSessionStatus { get; set; }

}