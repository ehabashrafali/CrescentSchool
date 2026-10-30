using CrescentSchool.Core.Options;
using Microsoft.Extensions.Options;
using System.Globalization;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;

namespace CrescentSchool.BLL.Services;

public sealed class ZoomService(HttpClient httpClient, IOptions<ZoomOptions> options) : IZoomService
{
    private readonly ZoomOptions _options = options.Value;
    private readonly SemaphoreSlim _tokenLock = new(1, 1);

    private string? _accessToken;
    private DateTimeOffset _tokenExpiresAt;

    public async Task<string> CreateMeetingAsync(string topic, DateTime startTime, CancellationToken cancellationToken = default)
    {
        var accessToken = await GetAccessTokenAsync(cancellationToken);

        var payload = new
        {
            topic,
            type = 2, // Scheduled meeting
            start_time = startTime.ToString("yyyy-MM-dd'T'HH:mm:ss", CultureInfo.InvariantCulture),
            settings = new
            {
                host_video = true,
                participant_video = true,
                join_before_host = false,
                waiting_room = true
            }
        };

        var hostUserId = Uri.EscapeDataString(_options.AccountId);

        using var request = new HttpRequestMessage(HttpMethod.Post, $"https://api.zoom.us/v2/users/{hostUserId}/meetings")
        {
            Content = JsonContent.Create(payload)
        };

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        using var response = await httpClient.SendAsync(request, cancellationToken);

        response.EnsureSuccessStatusCode();

        var meeting = await response.Content.ReadFromJsonAsync<ZoomMeetingResponse>(cancellationToken: cancellationToken);

        if (string.IsNullOrWhiteSpace(meeting?.JoinUrl))
            throw new InvalidOperationException(
                "Zoom did not return a meeting join URL.");

        return meeting.JoinUrl;
    }

    private async Task<string> GetAccessTokenAsync(CancellationToken cancellationToken)
    {
        await _tokenLock.WaitAsync(cancellationToken);

        try
        {
            if (!string.IsNullOrWhiteSpace(_accessToken) && DateTimeOffset.UtcNow < _tokenExpiresAt)
                return _accessToken;

            var credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_options.ClientId}:{_options.ClientSecret}"));

            var accountId = Uri.EscapeDataString(_options.AccountId);

            using var request = new HttpRequestMessage(HttpMethod.Post, "https://zoom.us/oauth/token" + $"?grant_type=account_credentials&account_id={accountId}");

            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", credentials);

            using var response = await httpClient.SendAsync(request, cancellationToken);

            response.EnsureSuccessStatusCode();

            var token = await response.Content.ReadFromJsonAsync<ZoomTokenResponse>(cancellationToken: cancellationToken);

            if (string.IsNullOrWhiteSpace(token?.AccessToken))
                throw new InvalidOperationException("Zoom did not return an access token.");

            _accessToken = token.AccessToken;

            _tokenExpiresAt = DateTimeOffset.UtcNow.AddSeconds(Math.Max(0, token.ExpiresIn - 60));

            return _accessToken;
        }
        finally
        {
            _tokenLock.Release();
        }
    }

    private sealed class ZoomTokenResponse
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; } = string.Empty;

        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }
    }

    private sealed class ZoomMeetingResponse
    {
        [JsonPropertyName("join_url")]
        public string JoinUrl { get; set; } = string.Empty;
    }
}
