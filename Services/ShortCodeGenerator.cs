using System;
using System.Collections.Generic;

namespace SecureUrlShortener.Services
{
    public class ShortCodeGenerator
    {
        private static readonly string chars =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        private static readonly HashSet<string> usedCodes = new();

        public string GenerateCode(int length = 6)
        {
            var random = new Random();
            string code;

            do
            {
                var buffer = new char[length];
                for (int i = 0; i < length; i++)
                {
                    buffer[i] = chars[random.Next(chars.Length)];
                }
                code = new string(buffer);
            }
            while (usedCodes.Contains(code));

            usedCodes.Add(code);
            return code;
        }
    }
}
