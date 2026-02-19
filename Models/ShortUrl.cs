using System;

namespace SecureUrlShortener.Models
{
  using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class ShortUrl
{
    public int Id { get; set; }

    [Required]
    public required string OriginalUrl { get; set; }

    [Required]
    public required string ShortCode { get; set; }

    [Column(TypeName = "timestamp")]
    public DateTime CreatedAt { get; set; }

    public int ClickCount { get; set; }
}


}
