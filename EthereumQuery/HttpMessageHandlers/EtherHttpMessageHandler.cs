using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EthereumQuery.HttpMessageHandlers
{
    public class EtherHttpMessageHandler : IEtherHttpMessageHandler
    {
        private readonly HttpClient _httpClient;
        private static string basicUrl = "https://mainnet.infura.io/v3/22b2ebe2940745b3835907b30e8257a4";

        public EtherHttpMessageHandler(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            if (_httpClient.BaseAddress == null)
            {
                _httpClient.BaseAddress = new Uri(basicUrl);
                _httpClient.DefaultRequestHeaders.Add("accept", "application/json");
            }
        }
        public async Task<HttpResponseMessage> PostAsync(string bodyData, CancellationToken cancellationToken)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                Content = new StringContent(bodyData, Encoding.UTF8, "application/json"),
            };
            var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
            return response;
        }
    }
}
