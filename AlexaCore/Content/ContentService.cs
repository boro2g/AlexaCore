using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using AlexaCore.Web;
using Newtonsoft.Json;

namespace AlexaCore.Content
{
    public interface IContentService<T> where T : IIntentContent
    {
        string LoadAndFormatContent(string key, string defaultText, RequestParameters additionalRequestParameters, params string[] parameters);
    }

    public abstract class ContentService<T> : IContentService<T> where T : IIntentContent
    {
        private readonly PersistentQueue<ApplicationParameter> _applicationParameters;
        private readonly string _userId;

        protected ContentService(PersistentQueue<ApplicationParameter> applicationParameters, string userId)
        {
            _applicationParameters = applicationParameters;
            _userId = userId;
        }

        public virtual IHttpClient BuildClient()
        {
            return new AlexaHttpClient(new HttpClientHandler {UseCookies = false}) { BaseAddress = BaseAddress, Timeout = Timeout() };
        }

        public abstract Uri BaseAddress { get; }

        public abstract string RequestUri(string intentKey, string userId, RequestParameters additionalRequestParameters);

        public virtual TimeSpan Timeout()
        {
            return TimeSpan.FromSeconds(1);
        }

        public virtual IEnumerable<string> CookiesToPersist()
        {
            yield return "ASP.NET_SessionId";
        }

        protected virtual T ProcessResult(T result)
        {
            return result;
        }

        public virtual HttpRequestMessage BuildRequestMessage(string requestUri, RequestParameters requestParameters)
        {
            return new HttpRequestMessage(HttpMethod.Get, requestUri);
        }

        public T LoadContent(string intentKey, string defaultText, string userId, RequestParameters additionalRequestParameters, IEnumerable<CookieValue> cookies, out IEnumerable<CookieValue> requestCookies) 
        {
            if (additionalRequestParameters == null)
            {
                additionalRequestParameters = new RequestParameters();
            }

            var cookiesToPersist = cookies?.Where(a => CookiesToPersist().Contains(a.Key));

            var baseAddress = BaseAddress;

            List<CookieValue> cookieStrings = new List<CookieValue>();

            try
            {
                using (var httpClient = BuildClient())
                {
                    httpClient.Timeout = Timeout();

                    string requestUri = RequestUri(intentKey, userId, additionalRequestParameters);

                    Console.WriteLine($"Loading content from: {baseAddress}{requestUri.TrimStart('/')}");

                    var message = BuildRequestMessage(requestUri, additionalRequestParameters);

                    if (cookies.Any())
                    {
                        message.Headers.TryAddWithoutValidation("Cookie",
                            String.Join(";", cookies.Select(a => a.ToString())));
                    }

                    var result = httpClient.SendAsync(message).Result;

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

                    cookieStrings.AddRange(cookiesToPersist?.Where(a =>
                        !cookieStrings.Select(b => b.Key).Contains(a.Key)));

                    var responseText = result.Content.ReadAsStringAsync().Result;

                    return ProcessResult(JsonConvert.DeserializeObject<T>(responseText));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                Console.WriteLine(e.StackTrace);

                return ProcessResult((T) (object) new IntentContent {Content = defaultText, Default = true});
            }
            finally
            {
                requestCookies = cookieStrings.Any() ? cookieStrings : cookies;
            }
        }

        public string LoadAndFormatContent(string key, string defaultText, RequestParameters additionalRequestParameters = null, params string[] parameters)
        {
            string cookieKey = "__cookies";

            var cookieValues = _applicationParameters.GetValue(cookieKey);

            List<CookieValue> cookieList = new List<CookieValue>();

            if (!String.IsNullOrWhiteSpace(cookieValues))
            {
                cookieList = JsonConvert.DeserializeObject<List<CookieValue>>(cookieValues);
            }

            var contentResults = LoadContent(key, defaultText, _userId, additionalRequestParameters, cookieList, out var responseCookies)?.Content;

            if (responseCookies != null && responseCookies.Any())
            {
                _applicationParameters.AddOrUpdateValue(cookieKey, JsonConvert.SerializeObject(responseCookies));
            }
            
            if (String.IsNullOrWhiteSpace(contentResults))
            {
                return String.Format(defaultText, parameters);
            }

            return String.Format(contentResults, parameters);
        }
    }
}
