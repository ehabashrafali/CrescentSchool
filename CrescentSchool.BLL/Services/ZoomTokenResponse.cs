using System.Text.Json.Serialization;

namespace CrescentSchool.BLL.Services;

public sealed partial class ZoomService
{
    private sealed class ZoomTokenResponse
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; } = string.Empty;

        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }
    }
}
