using Microsoft.EntityFrameworkCore;
using SecureUrlShortener.Models;

namespace SecureUrlShortener.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<ShortUrl> ShortUrls { get; set; }
    }
}
