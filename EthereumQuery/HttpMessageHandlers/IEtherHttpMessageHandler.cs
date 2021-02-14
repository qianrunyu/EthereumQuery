using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace EthereumQuery.HttpMessageHandlers
{
    public interface IEtherHttpMessageHandler
    {
        Task<HttpResponseMessage> PostAsync(string bodyData, CancellationToken cancellationToken = default);
    }
}
