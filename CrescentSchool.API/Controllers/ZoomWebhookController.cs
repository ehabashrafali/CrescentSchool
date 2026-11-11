using CrescentSchool.Core.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace CrescentSchool.API.Controllers;

[ApiController]
[Route("api/webhooks/zoom")]
public class ZoomWebhookController(IConfiguration configuration, ILogger<ZoomWebhookController> logger, IOptions<ZoomOptions> options) : ControllerBase
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

            var eventName = eventElement.GetString()!;

            // Zoom URL validation challenge.
            if (eventName == "endpoint.url_validation")
            {
                if (!root.TryGetProperty("payload", out var payload) ||
                    payload.ValueKind != JsonValueKind.Object ||
                    !payload.TryGetProperty("plainToken", out var token) ||
                    token.ValueKind != JsonValueKind.String)
                {
                    return BadRequest();
                }

                var plainToken = token.GetString();

                if (string.IsNullOrWhiteSpace(plainToken))
                    return BadRequest();

                var encryptedToken = Hash(secret, plainToken);

                logger.LogInformation("Zoom webhook URL validation challenge received.");

                // Return these fields directly, without an API wrapper.
                return new JsonResult(new
                {
                    plainToken,
                    encryptedToken
                });
            }

            // Verify real event notifications.
            if (!IsValidSignature(rawBody, secret))
            {
                logger.LogWarning("Zoom webhook signature verification failed.");
                return Unauthorized();
            }

            var supported = eventName is "meeting.started" or
                                         "meeting.participant_joined" or
                                         "meeting.participant_left" or
                                         "meeting.ended";

            if (!supported)
                return Ok();

            
            if (!root.TryGetProperty("payload", out var element) ||
                element.ValueKind != JsonValueKind.Object ||
                !element.TryGetProperty("object", out var meeting) ||
                meeting.ValueKind != JsonValueKind.Object)
            {
                return BadRequest();
            }

            var meetingId = ReadValue(meeting, "id");
            var meetingUuid = ReadValue(meeting, "uuid");
            var hostId = ReadValue(meeting, "host_id");
            var eventTimestamp = ReadValue(root, "event_ts");

            logger.LogInformation(
                "Zoom event {EventName}, meeting {MeetingId}, UUID {MeetingUuid}, host {HostId}, event timestamp {EventTimestamp}",
                eventName, meetingId, meetingUuid, hostId, eventTimestamp);

            if (eventName is "meeting.participant_joined" or "meeting.participant_left")
            {
                if (!meeting.TryGetProperty("participant", out var participant) ||
                    participant.ValueKind != JsonValueKind.Object)
                {
                    return BadRequest();
                }

                var participantName = ReadValue(participant, "user_name");
                var participantUserId = ReadValue(participant, "participant_user_id");
                var timeField = eventName == "meeting.participant_joined"
                    ? "join_time"
                    : "leave_time";
                var participantTime = ReadValue(participant, timeField);

                logger.LogInformation(
                    "Zoom event {EventName}, meeting {MeetingId}, participant {ParticipantName}, Zoom user ID {ParticipantUserId}, {TimeField} {ParticipantTime}",
                    eventName, meetingId, participantName, participantUserId,
                    timeField, participantTime);
            }

            // Persist before acknowledging.
            // Duplicate deliveries should be treated as successful.
            //await inbox.SaveIfNewAsync(
            //    eventName,
            //    rawBody,
            //    cancellationToken);

            logger.LogInformation(
                "Zoom webhook accepted: {EventName}",
                eventName);

            
            return Ok();
        }
    }

    private bool IsValidSignature(string rawBody, string secret)
    {
        var timestamp = Request.Headers["x-zm-request-timestamp"].ToString();

        var receivedSignature =
            Request.Headers["x-zm-signature"].ToString();

        if (!long.TryParse(timestamp, NumberStyles.Integer, CultureInfo.InvariantCulture, out var timestampSeconds))
            return false;

        var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        // Application replay-protection policy: five-minute window.
        if (timestampSeconds < now - 300 || timestampSeconds > now + 300)
            return false;

        var message = $"v0:{timestamp}:{rawBody}";
        var expectedSignature = $"v0={Hash(secret, message)}";

        return CryptographicOperations.FixedTimeEquals(Encoding.UTF8.GetBytes(expectedSignature),
                                                       Encoding.UTF8.GetBytes(receivedSignature));
    }

    private static string Hash(string secret, string value)
    {
        var hash = HMACSHA256.HashData(
            Encoding.UTF8.GetBytes(secret),
            Encoding.UTF8.GetBytes(value));

        return Convert.ToHexString(hash).ToLowerInvariant();
    }
    
    private static string? ReadValue(JsonElement source, string name)
    {
        if (!source.TryGetProperty(name, out var value))
            return null;

        return value.ValueKind switch
        {
            JsonValueKind.String => value.GetString(),
            JsonValueKind.Number => value.GetRawText(),
            _ => null
        };
    }
}