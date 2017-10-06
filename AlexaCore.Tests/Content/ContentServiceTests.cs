using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Alexa.NET.Request;
using AlexaCore.Content;
using AlexaCore.Web;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace AlexaCore.Tests.Content
{
    [TestFixture]
    class ContentServiceTests
    {
        [Test]
        public void WhenContentIsRequested_ValidTextIsReturned()
        {
            var queue = BuildQueue();

            var responseText = "Welcome to the house of fun";

            var contentService = new TestContentService(queue, "userId",
                new MockHttpClient(BuildResponseMessage(responseText)));

            var result = contentService.LoadAndFormatContent("HelpIntent", "default text");

            Assert.That(result, Is.EqualTo(responseText));
        }

        [Test]
        public void WhenContentIsRequested_CorrectUrlIsRequested()
        {
            Assert.That(RunRequestWithParameters(), Is.EqualTo("/url/intents/HelpIntent?j=1&userId=userId"));

            Assert.That(
                RunRequestWithParameters(new RequestParameters
                {
                    Parameters = new[] {new RequestParameter("a","b")}
                }), Is.EqualTo("/url/intents/HelpIntent?j=1&userId=userId&a=b"));
        }

        private static string RunRequestWithParameters(RequestParameters requestParameters = null)
        {
            var queue = BuildQueue();

            var mockHttpClient = new Mock<IHttpClient>();

            HttpRequestMessage message = null;

            mockHttpClient.Setup(a => a.SendAsync(It.IsAny<HttpRequestMessage>()))
                .Callback<HttpRequestMessage>(a => message = a);

            var contentService = new TestContentService(queue, "userId", mockHttpClient.Object);

            contentService.LoadAndFormatContent("HelpIntent", "default text", requestParameters);

            var requestUri = message.RequestUri.ToString();
            return requestUri;
        }

        [Test]
        public void WhenContentIsRequested_CookiesGetPersistedInQueue()
        {
            var queue = BuildQueue();

            var aspNetSessionid = "ASP.NET_SessionId";

            var cookieValue = "123";

            var contentService = new TestContentService(queue, "userId",
                new MockHttpClient(BuildResponseMessage("text",
                    new[] {new CookieValue {Key = aspNetSessionid, Value = cookieValue}})));

            contentService.LoadAndFormatContent("HelpIntent", "default text");

            VerifyQueueContainsCookie(queue, aspNetSessionid, cookieValue);

            contentService = new TestContentService(queue, "userId",
                new MockHttpClient(BuildResponseMessage("text")));

            contentService.LoadAndFormatContent("HelpIntent", "default text");

            VerifyQueueContainsCookie(queue, aspNetSessionid, cookieValue);
        }

        private void VerifyQueueContainsCookie(PersistentQueue<ApplicationParameter> queue, string cookieName, string cookieValue)
        {
            var cookieEntry = queue.GetValue("__cookies");

            Assert.That(cookieEntry, Is.Not.Null);

            var cookieValues = JsonConvert.DeserializeObject<CookieValue[]>(cookieEntry);

            var matchingCookie = cookieValues.FirstOrDefault(a => a.Key == cookieName);

            Assert.That(matchingCookie, Is.Not.Null);

            Assert.That(matchingCookie.Value, Is.EqualTo(cookieValue));
        }

        private static HttpResponseMessage BuildResponseMessage(string responseText, CookieValue[] cookies = null)
        {
            var responseMessage = new HttpResponseMessage
            {
                Content = new StringContent(
                    JsonConvert.SerializeObject(new IntentContent
                    {
                        Content = responseText
                    })),
            };

            if (cookies != null)
            {
                responseMessage.Headers.Add("set-cookie", String.Join(";", cookies.Select(a => a.ToString())));
            }

            return responseMessage;
        }

        private static PersistentQueue<ApplicationParameter> BuildQueue()
        {
            return new PersistentQueue<ApplicationParameter>(null,
                new Session {Attributes = new Dictionary<string, object>()}, "key");
        }
    }
}
