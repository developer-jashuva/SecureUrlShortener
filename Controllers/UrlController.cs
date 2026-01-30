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
        private readonly ShortCodeGenerator _shortCodeGenerator;

        public UrlController(
            UrlSafetyService urlSafetyService,
            ShortCodeGenerator shortCodeGenerator)
        {
            _urlSafetyService = urlSafetyService;
            _shortCodeGenerator = shortCodeGenerator;
        }

        [HttpPost("shorten")]
        public IActionResult ShortenUrl([FromBody] ShortenRequest request)
        {
            if (!_urlSafetyService.IsUrlSafe(request.OriginalUrl))
            {
                return BadRequest(new
                {
                    message = "⚠️ Suspicious or unsafe URL detected. Shortening blocked."
                });
            }

            var code = _shortCodeGenerator.GenerateCode();

            var shortUrl = $"{Request.Scheme}://{Request.Host}/{code}";

            var response = new ShortenResponse
            {
                ShortUrl = shortUrl
            };

            return Ok(response);
        }
    }
}
