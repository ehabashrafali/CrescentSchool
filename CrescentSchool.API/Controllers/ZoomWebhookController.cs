using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using CrescentSchool.Core.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CrescentSchool.API.Controllers
{
    [ApiController]
    [Route("api/webhooks/zoom")]
    public class ZoomWebhookController(IOptions<ZoomOptions> zoomOptions) : ControllerBase
    {
        private readonly ZoomOptions _options = zoomOptions.Value;

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Receive()
        {
            using var reader = new StreamReader(Request.Body, Encoding.UTF8);
            var body = await reader.ReadToEndAsync();

            using var doc = JsonDocument.Parse(body);
            var root = doc.RootElement;
            var eventName = root.TryGetProperty("event", out var ev) ? ev.GetString() : null;

            if (eventName == "endpoint.url_validation")
            {
                var plainToken = root.GetProperty("payload").GetProperty("plainToken").GetString() ?? string.Empty;
                return Ok(new { plainToken, encryptedToken = ComputeHmac(plainToken) });
            }

            var timestamp = Request.Headers["x-zm-request-timestamp"].ToString();
            var signature = Request.Headers["x-zm-signature"].ToString();
            var expected = "v0=" + ComputeHmac($"v0:{timestamp}:{body}");

            if (!CryptographicOperations.FixedTimeEquals(Encoding.UTF8.GetBytes(expected), Encoding.UTF8.GetBytes(signature)))
            {
                return Unauthorized();
            }

            // Handle Zoom webhook events.
            return Ok();
        }

        private string ComputeHmac(string message)
        {
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_options.WebhookSecretToken));
            return Convert.ToHexString(hmac.ComputeHash(Encoding.UTF8.GetBytes(message))).ToLowerInvariant();
        }
    }
}
