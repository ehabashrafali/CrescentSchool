using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CrescentSchool.API.Controllers
{
    [ApiController]
    [Route("api/webhooks/zoom")]
    public class ZoomWebhookController : ControllerBase
    {
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Receive()
        {
            // Handle Zoom endpoint validation and webhook events.
            return Ok();
        }
    }
}
