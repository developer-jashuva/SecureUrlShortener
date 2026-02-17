using Microsoft.AspNetCore.Mvc;
using SecureUrlShortener.Models;
using SecureUrlShortener.Services;
using SecureUrlShortener.Data;
using System.Linq;


namespace SecureUrlShortener.Controllers
{
    [ApiController]
    public class UrlController : ControllerBase
    {
        private readonly UrlSafetyService _urlSafetyService;
        private readonly ShortCodeGenerator _shortCodeGenerator;
        private readonly AppDbContext _db;

        public UrlController(
            UrlSafetyService urlSafetyService,
            ShortCodeGenerator shortCodeGenerator,
            AppDbContext db)
        {
            _urlSafetyService = urlSafetyService;
            _shortCodeGenerator = shortCodeGenerator;
            _db = db;
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

    // 🔎 Check if URL already exists
    var existing = _db.ShortUrls
                      .FirstOrDefault(x => x.OriginalUrl == request.OriginalUrl);

    if (existing != null)
    {
        var existingShortUrl = $"{Request.Scheme}://{Request.Host}/{existing.ShortCode}";

        return Ok(new ShortenResponse
        {
            ShortUrl = existingShortUrl
        });
    }

    // 🆕 Generate new short code
    var code = _shortCodeGenerator.GenerateCode();

    var entity = new ShortUrl
    {
        OriginalUrl = request.OriginalUrl,
        ShortCode = code,
        CreatedAt = DateTime.UtcNow,
        ClickCount = 0
    };

    _db.ShortUrls.Add(entity);
    _db.SaveChanges();

    var shortUrl = $"{Request.Scheme}://{Request.Host}/{code}";

    return Ok(new ShortenResponse
    {
        ShortUrl = shortUrl
    });
}

[HttpGet("api/url/all")]
public IActionResult GetAllUrls()
{
    var list = _db.ShortUrls
                  .OrderByDescending(x => x.CreatedAt)
                  .Select(x => new UrlHistoryItem
                  {
                      OriginalUrl = x.OriginalUrl,
                      ShortUrl = $"{Request.Scheme}://{Request.Host}/{x.ShortCode}",
                      ClickCount = x.ClickCount,
                      CreatedAt = x.CreatedAt
                  })
                  .ToList();

    return Ok(list);
}

    [HttpDelete("api/url/{code}")]
public IActionResult DeleteUrl(string code)
{
    var record = _db.ShortUrls.FirstOrDefault(x => x.ShortCode == code);

    if (record == null)
        return NotFound();

    _db.ShortUrls.Remove(record);
    _db.SaveChanges();

    return Ok();
}



        [HttpGet("{code}")]
        public IActionResult RedirectToOriginal(string code)
        {
            var record = _db.ShortUrls.FirstOrDefault(x => x.ShortCode == code);

            if (record == null)
            {
                return NotFound("Short URL not found");
            }

            record.ClickCount++;
            _db.SaveChanges();

            return Redirect(record.OriginalUrl);
        }
    }
}
