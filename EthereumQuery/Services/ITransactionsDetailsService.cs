using EthereumQuery.Model;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EthereumQuery.Services
{
    public interface ITransactionsDetailsService
    {

        Task<IList<TransactionsByBlockNumber>> GetTransactions(int blockNumInInt, CancellationToken cancellationToken = default);

    }
}
