using System;
using System.Net.Http;
using System.Threading.Tasks;
using AlexaCore.Web;

namespace AlexaCore.Tests.Content
{
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