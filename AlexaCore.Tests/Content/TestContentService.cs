using System;
using System.Net.Http;
using System.Threading.Tasks;
using AlexaCore.Content;
using AlexaCore.Web;

namespace AlexaCore.Tests.Content
{
    class TestContentService : ContentService<IntentContent>
    {
        private readonly IHttpClient _httpClient;

        public TestContentService(PersistentQueue<ApplicationParameter> applicationParameters, string userId, IHttpClient httpClient = null) 
            : base(applicationParameters, userId)
        {
            _httpClient = httpClient;
        }

        public override IHttpClient BuildClient()
        {
            return _httpClient ?? base.BuildClient();
        }

        public override Uri BaseAddress => new Uri("http://alexacontent.boro2g.co.uk");

        public override string RequestUri(string intentKey, string userId)
        {
            return $"/easyTea/intents/{intentKey}?j=1&userId={userId}";
        }
    }

    class MockHttpClient : IHttpClient
    {
        private readonly HttpResponseMessage _responseMessage;

        public MockHttpClient(HttpResponseMessage responseMessage)
        {
            _responseMessage = responseMessage;
        }

        public void Dispose()
        {
            
        }

        public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            return Task.FromResult(_responseMessage);
        }

        public Uri BaseAddress { get; set; }
        public TimeSpan Timeout { get; set; }
    }
}
