namespace SecureUrlShortener.Models
{
    public class UrlHistoryItem
    {
        required public string  OriginalUrl { get; set; }
        required public string ShortUrl  { get; set; }
        public int ClickCount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
