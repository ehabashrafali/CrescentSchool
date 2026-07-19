using CrescentSchool.Models.Enums;

namespace CrescentSchool.DAL.Entities;

public class Student
{
    public Guid Id { get; set; }
    public Instructor? Instructor { get; set; }
    public List<Course> Courses { get; set; } = [];
    public string ZoomMeeting { get; set; }
    public List<StudentMonthlyReport> StudentMonthlyReports { get; set; } = [];
    public List<Session> Sessions { get; set; } = [];
    public List<WeeklyAppointment> WeeklyAppointments { get; set; } = [];
    public decimal Fees { get; set; }
    public string UserId { get; set; } = null!;
    public ApplicationUser User { get; set; }
    public Guid InstructorId { get; set; }
    public StudentStatus Status { get; set; } = StudentStatus.Active;

    public Student(Guid id, Guid instructorId, decimal fees, List<WeeklyAppointment> weeklyAppointments, string zoomMeeting, ApplicationUser user)
    {
        Id = id;
        ZoomMeeting = zoomMeeting;
        WeeklyAppointments = weeklyAppointments;
        Fees = fees;
        User = user;
        InstructorId = instructorId;
    }

    private Student()
    {
    }
}
