namespace CrescentSchool.Models.Dtos;

public class UpcomingSessionDto
{
    public DateTime SessionDateTime { get; set; }
    public string InstructorName { get; set; }
    public string ZoomMeeting { get; set; } = string.Empty;
    public List<string> CourseName { get; set; } = [];
}
