using EthereumQuery.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace EthereumQuery.DataProcesser
{
    public interface ITransactionsProcessor
    {
        IList<GetTransactionsResponse> GetTransactionsByAddress(IList<TransactionsByBlockNumber> transactions, string addr);
    }
}
