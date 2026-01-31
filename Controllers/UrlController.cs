using Microsoft.AspNetCore.Mvc;
using SecureUrlShortener.Models;
using SecureUrlShortener.Services;

namespace SecureUrlShortener.Controllers
{
    [ApiController]
    public class UrlController : ControllerBase
    {
        private readonly UrlSafetyService _urlSafetyService;
        private readonly ShortCodeGenerator _shortCodeGenerator;
        private readonly UrlStoreService _urlStoreService;

        public UrlController(
            UrlSafetyService urlSafetyService,
            ShortCodeGenerator shortCodeGenerator,
            UrlStoreService urlStoreService)
        {
            _urlSafetyService = urlSafetyService;
            _shortCodeGenerator = shortCodeGenerator;
            _urlStoreService = urlStoreService;
        }

        [HttpPost("api/url/shorten")]
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

            _urlStoreService.Save(code, request.OriginalUrl);

            var shortUrl = $"{Request.Scheme}://{Request.Host}/{code}";

            return Ok(new ShortenResponse
            {
                ShortUrl = shortUrl
            });
        }

        // 🔥 REDIRECT ENDPOINT
        [HttpGet("{code}")]
        public IActionResult RedirectToOriginal(string code)
        {
            var originalUrl = _urlStoreService.GetOriginalUrl(code);

            if (originalUrl == null)
            {
                return NotFound("Short URL not found");
            }

            return Redirect(originalUrl); // HTTP 302
        }
    }
}
