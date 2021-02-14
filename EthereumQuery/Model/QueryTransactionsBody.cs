using System.Collections.Generic;

namespace EthereumQuery.Model
{
    public class QueryTransactionsBody
    {
        public string jsonrpc { get; private set; } = "2.0";
        public int id { get; private set; } = 1;
        public string method { get; private set; } = "eth_getBlockByNumber";
        public IList<object> @params { get; set; }
        public QueryTransactionsBody(string blockNumInHex)
        {
            @params = new List<object>();
            @params.Add(blockNumInHex);
            @params.Add(true);
        }

    }
}
