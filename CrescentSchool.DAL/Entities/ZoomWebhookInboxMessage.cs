namespace CrescentSchool.DAL.Entities;

public class ZoomWebhookInboxMessage
{
    public Guid Id { get; set; }
    public string EventName { get; set; } = string.Empty;
    public string MeetingUUID { get; set; } = string.Empty;
    public string Payload { get; set; } = string.Empty;
    public byte[] BodySha256 { get; set; } = [];
    public DateTime ReceivedAt { get; set; }
    public DateTime? ProcessedAt { get; set; }
}