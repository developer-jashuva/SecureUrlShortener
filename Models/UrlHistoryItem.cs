namespace SecureUrlShortener.Models
{
    public class UrlHistoryItem
    {
        public string OriginalUrl { get; set; }
        public string ShortUrl { get; set; }
        public int ClickCount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
