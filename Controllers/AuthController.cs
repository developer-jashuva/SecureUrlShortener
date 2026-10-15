using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureUrlShortener.Data;
using SecureUrlShortener.Models;
using SecureUrlShortener.Services;

namespace SecureUrlShortener.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly PasswordService _passwordService;

        public AuthController(
            AppDbContext db,
            PasswordService passwordService)
        {
            _db = db;
            _passwordService = passwordService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(
            RegisterRequest request)
        {
            var email = request.Email.Trim();

            var existingUser = await _db.Users
                .AnyAsync(u => u.Email == email);

            if (existingUser)
            {
                return Conflict(new
                {
                    message = "An account with this email already exists."
                });
            }

            var user = new AppUser
            {
                Username = request.Username.Trim(),
                Email = email,
                PasswordHash = string.Empty
            };

            user.PasswordHash = _passwordService.HashPassword(
                user,
                request.Password
            );

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            return StatusCode(201, new
            {
                message = "Registration successful.",
                userId = user.Id,
                username = user.Username,
                email = user.Email
            });
        }
    }
}