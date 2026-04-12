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
    public StudentAttendanceStatus StudentStatus { get; set; }
    public InstructorAttendanceStatus InstructorStatus { get; set; }
    public Guid InstructorId { get; set; }
    public Guid StudentId { get; set; }
    public SessionDuration Duration { get; set; }
    public bool IsDeleted { get; set; } = false;

    public Session(DateTime date, Guid studentId, Guid instructorId, StudentAttendanceStatus studentSessionStatus, InstructorAttendanceStatus instructorSessionStatus, SessionDuration sessionDuration)
    {
        Date = date;
        StudentId = studentId;
        InstructorId = instructorId;
        StudentStatus = studentSessionStatus;
        InstructorStatus = instructorSessionStatus;
        Duration = sessionDuration;
    }

    public void UpdateSession(Guid studentId,
                              Guid instructorId,
                              StudentAttendanceStatus studentSessionStatus,
                              InstructorAttendanceStatus instructorSessionStatus,
                              SessionDuration sessionDuration)
    {
        StudentId = studentId;
        InstructorId = instructorId;
        StudentStatus = studentSessionStatus;
        InstructorStatus = instructorSessionStatus;
        Duration = sessionDuration;
    }

}
