using CrescentSchool.Models.Enums;

namespace CrescentSchool.Models;

public class Attendence : AuditableEntity
{
    public Guid StudentId { get; set; }
    public Guid InstructorId { get; set; }
    public Guid CourseId { get; set; }
    public bool InstructorAttendance { get; set; } = false;
    public bool StudentAttendance { get; set; } = false;
    public AttendanceStatus Status { get; set; }
    public DateTime? InstructorJoinTime { get; set; }
    public DateTime? StudentJoinTime { get; set; }
    //if any joined the session after session time by 5 minutes
}
