namespace CrescentSchool.Models;

public class Student
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string ParentName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public List<Instructor> Instructors { get; set; } = [];
    public List<Course> Courses { get; set; } = [];
    public bool IsActive { get; set; } = true;
    public Parent? Parent { get; set; }
    public string ZoomMeeting { get; set; } = string.Empty;
    public List<StudentMonthlyReport> StudentMonthlyReports { get; set; } = [];
    public DateOnly DateOfBirth { get; set; }
    public List<Session> Sessions { get; set; } = [];
    public List<WeeklyAppointment> WeeklyAppointments { get; set; } = [];
    public string FullName => FirstName + ' ' + LastName;
}
