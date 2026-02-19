using CrescentSchool.API.Entities;
namespace CrescentSchool.DAL.Entities;

public class Student
{
    public Guid Id { get; set; }
    public Instructor Instructor { get; set; }
    public List<Course> Courses { get; set; } = [];
    public string ZoomMeeting { get; set; } = string.Empty;
    public List<StudentMonthlyReport> StudentMonthlyReports { get; set; } = [];
    public List<Session> Sessions { get; set; } = [];
    public List<WeeklyAppointment> WeeklyAppointments { get; set; } = [];
    public decimal Fees { get; set; }
    public ApplicationUser User { get; set; }
    public Guid InstructorId { get; internal set; }
}
