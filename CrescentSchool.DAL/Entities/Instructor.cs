using CrescentSchool.API.Entities;

namespace CrescentSchool.DAL.Entities;

public class Instructor
{
    public Guid Id { get; set; }
    public List<Student> Students { get; set; } = [];
    public List<Course> Courses { get; set; } = [];
    public List<Session> Sessions { get; set; } = [];
    public string Country { get; set; } = string.Empty;
    public decimal Fees { get; set; }
    public string ZoomMeeting { get; set; } = string.Empty;
    public ApplicationUser User { get; set; }
}
