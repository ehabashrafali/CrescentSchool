using System.Text.Json.Serialization;

namespace CrescentSchool.BLL.Services;

public class ZoomMeetingResponse
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("join_url")]
    public string JoinUrl { get; set; } = string.Empty;

    [JsonPropertyName("uuid")]
    public string Uuid { get; set; } = string.Empty;
}
