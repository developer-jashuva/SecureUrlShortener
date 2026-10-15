using Microsoft.AspNetCore.Identity;
using SecureUrlShortener.Models;

namespace SecureUrlShortener.Services
{
    public class PasswordService
    {
        private readonly PasswordHasher<AppUser> _passwordHasher = new();

        public string HashPassword(AppUser user, string password)
        {
            return _passwordHasher.HashPassword(user, password);
        }

        public bool VerifyPassword(AppUser user, string hashedPassword, string password)
        {
            var result = _passwordHasher.VerifyHashedPassword(
                user,
                hashedPassword,
                password
            );

            return result != PasswordVerificationResult.Failed;
        }
    }
}