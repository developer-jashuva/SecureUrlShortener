using System.Collections.Generic;

namespace SecureUrlShortener.Services
{
    public class UrlStoreService
    {
        private static readonly Dictionary<string, string> urlMap = new();

        public void Save(string shortCode, string originalUrl)
        {
            urlMap[shortCode] = originalUrl;
        }

        public string? GetOriginalUrl(string shortCode)
        {
            return urlMap.ContainsKey(shortCode) ? urlMap[shortCode] : null;
        }
    }
}
