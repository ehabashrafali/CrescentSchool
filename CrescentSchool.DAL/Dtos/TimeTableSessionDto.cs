namespace CrescentSchool.DAL.Dtos;

public class TimeTableSessionDto
{
    public DateTime SessionDateTime { get; set; }
    public string ZoomMeeting { get; set; } = string.Empty;
    public string StudentName { get; set; } = string.Empty;
    public string InstructorName { get; set; } = string.Empty;
}

