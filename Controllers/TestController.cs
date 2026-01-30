using Microsoft.AspNetCore.Mvc;

namespace SecureUrlShortener.Controllers
{
    [ApiController]
    [Route("api/test")]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            return "Backend is working!";
        }
    }
}
