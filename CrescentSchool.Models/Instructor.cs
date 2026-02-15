namespace CrescentSchool.Models;

public class Instructor
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public List<Student> Students { get; set; } = [];
    public List<Course> Courses { get; set; } = [];
    public List<Session> Sessions { get; set; } = [];
    public string Country { get; set; } = string.Empty;
    public string FullName => FirstName + ' ' + LastName;
    public decimal Fees { get; set; }
    public string ZoomMeeting { get; set; } = string.Empty;
}
