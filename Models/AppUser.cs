using System.ComponentModel.DataAnnotations;

namespace SecureUrlShortener.Models
{
    public class AppUser
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public required string Username { get; set; }

        [Required]
        [MaxLength(200)]
        public required string Email { get; set; }

        [Required]
        public required string PasswordHash { get; set; }
    }
}