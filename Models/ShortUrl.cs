using System;

namespace SecureUrlShortener.Models
{
    public class ShortUrl
    {
        public int Id { get; set; }
       required public string OriginalUrl { get; set; }
      required  public string ShortCode { get; set; }
        public DateTime CreatedAt { get; set; }
        public int ClickCount { get; set; }
    }
}
