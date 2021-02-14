using EthereumQuery.Model;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;

namespace EthereumQuery.DataProcesser
{
    public class TransactionsProcessor : ITransactionsProcessor
    {
        public IList<GetTransactionsResponse> GetTransactionsByAddress(IList<TransactionsByBlockNumber> transactions, string addr)
        {
            if (transactions != null)
            {
                var filteredTrans = transactions.Where(x => x.From == addr || x.To == addr).ToList();
                var transResponseList = filteredTrans.Select(x => new GetTransactionsResponse()
                {

                    BlockHash = x.BlockHash,
                    BlockNumber = x.BlockNumber,
                    From = x.From,
                    To = x.To,
                    Hash = x.Hash,
                    Value = UnitConverter(x.Value, "Ether"),
                    Gas = UnitConverter(x.Gas, "Wei")
                }).ToList();
                return transResponseList;
            }
            else
            {
                return new List<GetTransactionsResponse>();
            }
        }

        //in real worl better to put into helper class. Add add flexible unit convertion base on the input value size.
        private string UnitConverter(string valueInHex, string unit)
        {
            if (unit == "Ether")
            {
                return (decimal.Parse(BigInteger.Parse("0" + valueInHex.Substring(2), NumberStyles.HexNumber).ToString()) * decimal.Parse("1e-18", NumberStyles.Float)).ToString("0.##########") + " Ether";
            }
            else if (unit == "GWei")
            {
                return (decimal.Parse(BigInteger.Parse("0" + valueInHex.Substring(2), NumberStyles.HexNumber).ToString()) * decimal.Parse("1e-9", NumberStyles.Float)).ToString("0.##########") + " GWei";
            }
            else if (unit == "Wei")
            {
                return BigInteger.Parse("0" + valueInHex.Substring(2), NumberStyles.HexNumber).ToString("") + " Wei";
            }
            else
            {
                return "N/A";
            }
        }
    }
}
