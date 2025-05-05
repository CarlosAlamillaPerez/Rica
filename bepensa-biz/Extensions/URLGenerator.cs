using System;

namespace bepensa_biz.Extensions
{
    public class URLGenerator
    {
        private static Random random = new();

        private static string _chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";


        public static string GenerateShortUrl(int length = 6)
        {
            var shortUrl = new char[length];

            for (int i = 0; i < length; i++)
            {
                shortUrl[i] = _chars[random.Next(_chars.Length)];
            }

            return new string(shortUrl);
        }
    }
}
