using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using EthereumQuery.Services;
using System.Linq;
using EthereumQuery.DataProcesser;

namespace EthereumQuery
{
    public class EthereumQueryFunctions
    {
        private readonly ITransactionsDetailsService _transactionsDetailsService;
        private readonly ITransactionsProcessor _transactionsProcessor;

        public EthereumQueryFunctions(ITransactionsDetailsService transactionsDetailsService, ITransactionsProcessor transactionsProcessor)
        {
            _transactionsDetailsService = transactionsDetailsService;
            _transactionsProcessor = transactionsProcessor;
        }

        [FunctionName("GetTransactionsByBlockNumAndAddress")]
        public async Task<IActionResult> GetTransactionsByBlockNumAndAddress(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "blocknum/{blocknum}/address/{addr}")] HttpRequest req, string blocknum, string addr,
            ILogger log)
        {
            log.LogInformation($"GetTransactionsByBlockNumAndAddress Request received. Block number: {blocknum} , Address: {addr}.");
            //in real world application should use proper validator class
            int blokNumInInt = -1;
            if (!int.TryParse(blocknum, out blokNumInInt) || blokNumInInt < 0)
            {
                log.LogError($"GetTransactionsByBlockNumAndAddress Request received with invalid block number: {blocknum}.");
                return new BadRequestObjectResult("Invalid Block number.");
            }
            if (!addr.StartsWith("0x", System.StringComparison.OrdinalIgnoreCase))
            {
                log.LogError($"GetTransactionsByBlockNumAndAddress Request received with invalid address: {addr}.");
                return new BadRequestObjectResult("Invalid Address: hex string without 0x prefix");

            }
            var transactions = await _transactionsDetailsService.GetTransactions(blokNumInInt);
            var responseList = _transactionsProcessor.GetTransactionsByAddress(transactions, addr);
            if (responseList.Count() == 0)
            {
                log.LogInformation($"Address {addr} is not found on block number {blocknum}.");
            }
            return new OkObjectResult(responseList);
        }
    }
}
