using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace AlexaCore.Web
{
    public interface IHttpClient : IDisposable
    {
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage request);
        Uri BaseAddress { get; set; }
        TimeSpan Timeout { get; set; }
    }

    class AlexaHttpClient : IHttpClient
    {
        private readonly HttpClient _httpClient;

        public AlexaHttpClient(HttpClientHandler httpClientHandler)
        {
            _httpClient = new HttpClient(httpClientHandler);
        }

        public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            return _httpClient.SendAsync(request);
        }

        public Uri BaseAddress
        {
            get => _httpClient.BaseAddress;
            set => _httpClient.BaseAddress = value;
        }
        
        public TimeSpan Timeout
        {
            get => _httpClient.Timeout;
            set => _httpClient.Timeout = value;
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}
