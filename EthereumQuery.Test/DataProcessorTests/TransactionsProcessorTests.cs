using EthereumQuery.DataProcesser;
using EthereumQuery.Model;
using System.Collections.Generic;
using Xunit;
using FluentAssertions;
using System.Linq;

namespace EthereumQuery.Test.HttpTriggerTests
{
    public class TransactionsProcessorTests
    {
        private readonly TransactionsProcessor _testee;

        public TransactionsProcessorTests()
        {
            _testee = new TransactionsProcessor();
        }
        [Fact]
        public void GetTransactions_ShouldNot_ReturnNullList()
        {
            //arrange
            List<TransactionsByBlockNumber> inputList = null;
            var addr = "0x11";

            //act
            var result = _testee.GetTransactionsByAddress(inputList, addr);

            //assert
            result.Should().NotBeNull();
        }

        [Fact]
        public void FilteredTransactions_ReturnValidCount()
        {
            //arrange
            var addr = "0x11";
            List<TransactionsByBlockNumber> inputList = new List<TransactionsByBlockNumber>();
            inputList.Add(new TransactionsByBlockNumber() { From = addr, To = "0xAA", Value = "0x5555", BlockHash = "0x1", BlockNumber = "0x1", Gas = "0x1", Hash = "0x2" });
            inputList.Add(new TransactionsByBlockNumber() { From = addr, To = "0xAA", Value = "0x5555", BlockHash = "0x1", BlockNumber = "0x1", Gas = "0x1", Hash = "0x2" });
            inputList.Add(new TransactionsByBlockNumber() { From = "0xBB", To = addr, Value = "0x5555", BlockHash = "0x1", BlockNumber = "0x1", Gas = "0x1", Hash = "0x2" });
            inputList.Add(new TransactionsByBlockNumber() { From = "0xBB", To = "0xCC", Value = "0x5555", BlockHash = "0x1", BlockNumber = "0x1", Gas = "0x1", Hash = "0x2" });

            //act
            var result = _testee.GetTransactionsByAddress(inputList, addr);

            //assert
            result.Should().NotBeNull();
            result.Count().Should().Be(3);
        }

        [Theory]
        [InlineData("0xf17937cf93cc0000", "17.4 Ether")]
        [InlineData("0x4563918244f400000", "80 Ether")]
        public void FilteredTransactions_ReturnValidUnitConvertionResult(string valueInHex, string expected)
        {
            //arrange
            var addr = "0x11";
            List<TransactionsByBlockNumber> inputList = new List<TransactionsByBlockNumber>();
            inputList.Add(new TransactionsByBlockNumber() { From = addr, To = "0xAA", Value = valueInHex, BlockHash = "0x1", BlockNumber = "0x1", Gas = "0x1", Hash = "0x2" });

            //act
            var result = _testee.GetTransactionsByAddress(inputList, addr);

            //assert
            result.Count().Should().Be(1);
            result[0].Value.Should().Be(expected);
        }
    }
}
