namespace CrescentSchool.DAL.Dtos;

public class TimeTableSessionDto
{
    public DateTime SessionDateTime { get; set; }
    public string ParticipantName { get; set; } = string.Empty;
    public string ZoomMeeting { get; set; } = string.Empty;
    public List<string> CourseName { get; set; } = [];
    public string StudentName { get; set; } = string.Empty;
    public string InstructorName { get; set; } = string.Empty;
}