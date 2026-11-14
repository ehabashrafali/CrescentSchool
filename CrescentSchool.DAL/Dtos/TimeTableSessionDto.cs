namespace CrescentSchool.DAL.Dtos;

public class TimeTableSessionDto
{
    public DateTime SessionDateTime { get; set; }
    public string ZoomMeeting { get; set; } = string.Empty;
    public string StudentName { get; set; } = string.Empty;
    public string InstructorName { get; set; } = string.Empty;
    public long ZoomMeetingId { get; set; }
    public string Uuid { get; set; } = string.Empty;
    public Guid StudentId { get; set; }
    public Guid InstructorId { get; set; }
    public bool IsCanceled { get; set; }
}

