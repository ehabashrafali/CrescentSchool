using CrescentSchool.DAL.Entities;
using CrescentSchool.Models.Enums;


public class Session
{
    private Session()
    { }
    public Guid Id { get; set; }
    public DateTime Date { get; init; }
    public Instructor Instructor { get; init; }
    public Student Student { get; set; }
    public AttendanceStatus StudentStatus { get; set; }
    public AttendanceStatus InstructorStatus { get; set; }
    public Guid InstructorId { get; set; }
    public Guid StudentId { get; set; }
    public SessionDuration Duration { get; set; }

    public Session(DateTime date, Guid studentId, Guid instructorId, AttendanceStatus studentSessionStatus, AttendanceStatus instructorSessionStatus)

    {
        Date = date;
        StudentId = studentId;
        InstructorId = instructorId;
        StudentStatus = studentSessionStatus;
        InstructorStatus = instructorSessionStatus;
    }

}
