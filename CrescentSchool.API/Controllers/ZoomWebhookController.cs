using CrescentSchool.BLL.Services;
using CrescentSchool.Core.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Data.Common;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace CrescentSchool.API.Controllers;

[ApiController]
[Route("api/webhooks/zoom")]
public class ZoomWebhookController(ILogger<ZoomWebhookController> logger, IOptions<ZoomOptions> options, IZoomWebhookInbox inbox) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost]
    [RequestSizeLimit(1_048_576)]
    public async Task<IActionResult> Receive(CancellationToken cancellationToken)
    {
        var secret = options.Value.WebhookSecretToken;

        if (string.IsNullOrWhiteSpace(secret) || secret.Contains("##{", StringComparison.Ordinal))
        {
            logger.LogError("Zoom webhook secret is not configured.");
            return StatusCode(StatusCodes.Status503ServiceUnavailable);
        }

        using var reader = new StreamReader(Request.Body, Encoding.UTF8);
        var rawBody = await reader.ReadToEndAsync(cancellationToken);

        JsonDocument document;

        try
        {
            document = JsonDocument.Parse(rawBody);
        }
        catch (JsonException)
        {
            return BadRequest();
        }

        using (document)
        {
            var root = document.RootElement;

            if (root.ValueKind != JsonValueKind.Object || !root.TryGetProperty("event", out var eventElement) || eventElement.ValueKind != JsonValueKind.String)
                return BadRequest();

            var eventName = eventElement.GetString();

            if (string.IsNullOrWhiteSpace(eventName))
                return BadRequest();

            if (eventName == "endpoint.url_validation")
            {
                if (!root.TryGetProperty("payload", out var validationPayload) || validationPayload.ValueKind != JsonValueKind.Object ||
                    !validationPayload.TryGetProperty("plainToken", out var token) || token.ValueKind != JsonValueKind.String)

                    return BadRequest();

                var plainToken = token.GetString();

                if (string.IsNullOrWhiteSpace(plainToken))
                    return BadRequest();

                logger.LogInformation("Zoom webhook URL validation challenge received.");

                return new JsonResult(new
                {
                    plainToken,
                    encryptedToken = Hash(secret, plainToken)
                });
            }

            if (!IsValidSignature(rawBody, secret))
            {
                logger.LogWarning("Zoom webhook signature verification failed.");
                return Unauthorized();
            }

            if (eventName is not ("meeting.participant_joined" or "meeting.participant_left"))
                return Ok();

            if (!root.TryGetProperty("payload", out var payload) ||
                payload.ValueKind != JsonValueKind.Object ||
                !payload.TryGetProperty("object", out var meeting) ||
                meeting.ValueKind != JsonValueKind.Object ||
                !meeting.TryGetProperty("uuid", out var uuidElement) ||
                uuidElement.ValueKind != JsonValueKind.String ||
                !meeting.TryGetProperty("participant", out var participant) ||
                participant.ValueKind != JsonValueKind.Object)
            {
                return BadRequest();
            }

            var meetingUuid = uuidElement.GetString();

            if (string.IsNullOrWhiteSpace(meetingUuid) || meetingUuid.Length > 100)
                return BadRequest();

            bool inserted;

            try
            {
                inserted = await inbox.SaveIfNewAsync(eventName, meetingUuid, rawBody, cancellationToken);
            }
            catch (Exception exception) when (exception is DbUpdateException or DbException)
            {
                logger.LogError(exception, "Failed to save Zoom webhook {EventName} for meeting {MeetingUuid}.", eventName, meetingUuid);

                return StatusCode(StatusCodes.Status503ServiceUnavailable);
            }

            logger.LogInformation("Zoom webhook acknowledged. Event {EventName}, meeting {MeetingUuid}, inserted {Inserted}.", eventName, meetingUuid, inserted);

            return Ok();
        }
    }

    private bool IsValidSignature(string rawBody, string secret)
    {
        var timestamp = Request.Headers["x-zm-request-timestamp"].ToString();
        var receivedSignature = Request.Headers["x-zm-signature"].ToString();

        if (string.IsNullOrWhiteSpace(receivedSignature) || !long.TryParse(timestamp, NumberStyles.Integer, CultureInfo.InvariantCulture, out var timestampSeconds))
            return false;

        var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        if (timestampSeconds < now - 300 || timestampSeconds > now + 300)
            return false;

        var message = $"v0:{timestamp}:{rawBody}";
        var expectedSignature = $"v0={Hash(secret, message)}";

        return CryptographicOperations.FixedTimeEquals(Encoding.UTF8.GetBytes(expectedSignature), Encoding.UTF8.GetBytes(receivedSignature));
    }

    private static string Hash(string secret, string value)
    {
        var hash = HMACSHA256.HashData(
            Encoding.UTF8.GetBytes(secret),
            Encoding.UTF8.GetBytes(value));

        return Convert.ToHexString(hash).ToLowerInvariant();
    }
}