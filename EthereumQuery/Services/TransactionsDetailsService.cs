using EthereumQuery.HttpMessageHandlers;
using EthereumQuery.Model;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EthereumQuery.Services
{
    public class TransactionsDetailsService : ITransactionsDetailsService
    {
        private readonly IEtherHttpMessageHandler _httpMessageHandler;
        private readonly ILogger<TransactionsDetailsService> _log;
        private List<TransactionsByBlockNumber> TransactionsResults;
        public TransactionsDetailsService(IEtherHttpMessageHandler httpMessageHandler, ILogger<TransactionsDetailsService> log)
        {
            _httpMessageHandler = httpMessageHandler ?? throw new ArgumentNullException(nameof(httpMessageHandler));
            _log = log;
        }
        public async Task<IList<TransactionsByBlockNumber>> GetTransactions(int blockNumInInt, CancellationToken cancellationToken)
        {
            string blockNumInHex = "0x" + Convert.ToString(blockNumInInt, 16);
            string bodyData = JsonConvert.SerializeObject(new QueryTransactionsBody(blockNumInHex));

            using (var response = await _httpMessageHandler.PostAsync(bodyData))
            {
                var stream = await response.Content.ReadAsStreamAsync();
                response.EnsureSuccessStatusCode();

                _log.LogInformation("Received transactions data from server. Start to deserialize.");

                string responseBody = await StreamToStringAsync(stream); //more efficient for large json data
                if (response.StatusCode == System.Net.HttpStatusCode.OK && responseBody.Contains(@"""result"":null"))
                {
                    _log.LogWarning($"Transaction details for block number {blockNumInInt} were not found.");
                    return await Task.FromResult<List<TransactionsByBlockNumber>>(null);
                }
                TransactionsResults = new List<TransactionsByBlockNumber>();
                JObject responseJObject = JObject.Parse(responseBody);
                List<JToken> results = responseJObject["result"]["transactions"].Children().ToList();

                _log.LogInformation("Raw transaction data filtered. Ready to generate objects.");

                results.ForEach(x => TransactionsResults.Add(x.ToObject<TransactionsByBlockNumber>()));
            }

            return TransactionsResults;
        }
        private static async Task<string> StreamToStringAsync(Stream stream)
        {
            string content = null;

            if (stream != null)
                using (var sr = new StreamReader(stream))
                    content = await sr.ReadToEndAsync();

            return content;
        }
    }
}
