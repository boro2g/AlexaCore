using System;

namespace AlexaCore.Content
{
    internal class CookieValueParser
    {
        public static CookieValue ParseCookie(string value)
        {
            Console.WriteLine("cookieValue " + value);

            var equalsPosition = value.IndexOf("=");

            if (equalsPosition == -1)
            {
                throw new NotSupportedException();
            }

            return new CookieValue
            {
                Key = value.Substring(0, equalsPosition),

                Value = value.Substring(equalsPosition + 1)
            };
        }
    }
}