using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureUrlShortener.Data;
using SecureUrlShortener.Models;
using SecureUrlShortener.Services;

using Microsoft.AspNetCore.Authorization;

namespace SecureUrlShortener.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly PasswordService _passwordService;
        private readonly TokenService _tokenService;

        public AuthController(
    AppDbContext db,
    PasswordService passwordService,
    TokenService tokenService)
{
    _db = db;
    _passwordService = passwordService;
    _tokenService = tokenService;
}
        [AllowAnonymous]
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
 
        [AllowAnonymous]
        [HttpPost("login")]
public async Task<IActionResult> Login(LoginRequest request)
{
    var email = request.Email.Trim();

    var user = await _db.Users
        .SingleOrDefaultAsync(u => u.Email == email);

    if (user == null ||
        !_passwordService.VerifyPassword(
            user,
            user.PasswordHash,
            request.Password))
    {
        return Unauthorized(new
        {
            message = "Invalid email or password."
        });
    }

    var token = _tokenService.GenerateToken(user);

    return Ok(new
    {
        message = "Login successful.",
        token,
        user = new
        {
            user.Id,
            user.Username,
            user.Email
        }
    });
}
 
    }
}