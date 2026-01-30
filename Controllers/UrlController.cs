using Microsoft.AspNetCore.Mvc;
using SecureUrlShortener.Models;
using SecureUrlShortener.Services;

namespace SecureUrlShortener.Controllers
{
    [ApiController]
    [Route("api/url")]
    public class UrlController : ControllerBase
    {
        private readonly UrlSafetyService _urlSafetyService;

        public UrlController(UrlSafetyService urlSafetyService)
        {
            _urlSafetyService = urlSafetyService;
        }

        [HttpPost("shorten")]
        public IActionResult ShortenUrl([FromBody] ShortenRequest request)
        {
            bool isSafe = _urlSafetyService.IsUrlSafe(request.OriginalUrl);

            if (!isSafe)
            {
                return BadRequest(new
                {
                    message = "⚠️ Suspicious or unsafe URL detected. Shortening blocked."
                });
            }

            // Temporary short URL
            var response = new ShortenResponse
            {
                ShortUrl = "http://localhost:5158/abc123"
            };

            return Ok(response);
        }
    }
}
