using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;

namespace AlexaCore.Content
{
    public interface IContentService<T> where T : IIntentContent
    {
        T LoadContent(string key, string defaultText, string userId, IEnumerable<CookieValue> cookies,
            out IEnumerable<CookieValue> requestCookies);

        string LoadAndFormatContent(string key, string defaultText, params string[] parameters);
    }

    public abstract class ContentService<T> : IContentService<T> where T : IIntentContent
    {
        private const string AspNetSessionIdKey = "ASP.NET_SessionId";

        public abstract string BaseAddress { get; }

        public abstract string RequestUri(string key, string userId);

        public virtual TimeSpan Timeout()
        {
            return TimeSpan.FromSeconds(1);
        }

        public T LoadContent(string key, string defaultText, string userId,
            IEnumerable<CookieValue> cookies, out IEnumerable<CookieValue> requestCookies) 
        {
            var sessionCookie = cookies?.FirstOrDefault(a => a.Key == AspNetSessionIdKey);

            var baseAddress = new Uri(BaseAddress);

            List<CookieValue> cookieStrings = new List<CookieValue>();

            try
            {
                using (var handler = new HttpClientHandler { UseCookies = false })
                {
                    using (var client = new HttpClient(handler) { BaseAddress = baseAddress })
                    {
                        client.Timeout = Timeout();

                        string requestUri = RequestUri(key, userId);
                        
                        Console.WriteLine($"Loading content from: {baseAddress}{requestUri}");

                        var message = new HttpRequestMessage(HttpMethod.Get, requestUri);

                        if (cookies.Any())
                        {
                            message.Headers.TryAddWithoutValidation("Cookie", String.Join(";", cookies.Select(a => a.ToString())));
                        }

                        var result = client.SendAsync(message).Result;

                        result.EnsureSuccessStatusCode();

                        var cookieHeader = result.Headers.FirstOrDefault(a =>
                            String.Equals(a.Key, "set-cookie", StringComparison.OrdinalIgnoreCase)).Value;

                        if (cookieHeader != null)
                        {
                            foreach (var cookieValue in cookieHeader)
                            {
                                var parts = cookieValue.Split(';');

                                if (parts.Length > 0)
                                {
                                    cookieStrings.Add(CookieValueParser.ParseCookie(parts[0]));
                                }
                            }
                        }

                        if (cookieStrings.Count > 0 && !SessionCookieExists(cookieStrings) && sessionCookie != null)
                        {
                            cookieStrings.Add(sessionCookie);
                        }

                        return (T)(object)JsonConvert.DeserializeObject<IntentContent>(result.Content.ReadAsStringAsync().Result);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                Console.WriteLine(e.StackTrace);

                return (T)(object) new IntentContent { Content = defaultText, Default = true };
            }
            finally
            {
                requestCookies = cookieStrings.Any() ? cookieStrings : cookies;
            }
        }

        public string LoadAndFormatContent(string key, string defaultText, params string[] parameters)
        {

            //set keys in application parameters?
            //where best to get application parameters from?
            //how to resolve the IContentService?
            //is T best at interface or method level?

            throw new NotImplementedException();
        }

        private static bool SessionCookieExists(List<CookieValue> cookieStrings)
        {
            return cookieStrings.Any(a => a.Key == AspNetSessionIdKey);
        }
    }
}
