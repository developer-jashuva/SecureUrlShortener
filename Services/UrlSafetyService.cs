using System;
using System.Linq;

namespace SecureUrlShortener.Services
{
    public class UrlSafetyService
    {
        private readonly string[] suspiciousKeywords =
        {
            "login",
            "verify",
            "free",
            "prize",
            "bank",
            "update-password",
            "secure-account"
        };

        public bool IsUrlSafe(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return false;

            // Check valid URL format
            if (!Uri.TryCreate(url, UriKind.Absolute, out var uriResult))
                return false;

            // HTTPS check
            if (uriResult.Scheme != Uri.UriSchemeHttps)
                return false;

            // Length check
            if (url.Length > 200)
                return false;

            // Keyword check
            var lowerUrl = url.ToLower();
            if (suspiciousKeywords.Any(keyword => lowerUrl.Contains(keyword)))
                return false;

            return true; // URL is safe
        }
    }
}
