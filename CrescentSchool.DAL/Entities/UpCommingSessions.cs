namespace CrescentSchool.DAL.Entities;

public class UpCommingSessions
{
    public Guid Id { get; set; }
    public DateTime SessionDateTime { get; set; }
    public string ZoomMeeting { get; set; } = string.Empty;
    public string StudentName { get; set; } = string.Empty;
    public string InstructorName { get; set; } = string.Empty;
    public Guid StudentId { get; set; }
    public Guid InstructorId { get; set; }
    public DateTime? FirstJoinedAt { get; set; }
    public DateTime? LastLeftAt { get; set; }
    public int TotalDurationInSeconds { get; set; }
    public long ZoomMeetingId { get; set; }
    public string ZoomMeetingUuid { get; set; }
    public string ZoomJoinUrl { get; set; }
    public bool IsCurrentlyJoined { get; set; }
}
