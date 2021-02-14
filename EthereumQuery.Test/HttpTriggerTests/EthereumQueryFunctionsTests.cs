using EthereumQuery.DataProcesser;
using EthereumQuery.Model;
using System.Collections.Generic;
using Xunit;
using FluentAssertions;
using Moq;
using Microsoft.Extensions.Logging;
using EthereumQuery.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EthereumQuery.Test.DataProcessorTests
{
    public class EthereumQueryFunctionsTests
    {

        [Theory]
        [InlineData("-10", "0xaa")]
        [InlineData("-a10", "0xaa")]
        [InlineData("abc", "0xaa")]
        [InlineData("", "0xaa")]
        public async Task GetTransOnBlockNumAndAddr_ShouldReturnBadRequst_whenBlockNumIsInvalid(string blockNum, string addr)
        {
            //arrange
            var mockTransactionsProcessor = new Mock<ITransactionsProcessor>();
            var mockLogger = new Mock<ILogger>();
            var mockTransactionsDetailsService = new Mock<ITransactionsDetailsService>();
            var _testee = new EthereumQueryFunctions(mockTransactionsDetailsService.Object, mockTransactionsProcessor.Object);

            //act
            var result = await _testee.GetTransactionsByBlockNumAndAddress(null, blockNum, addr, mockLogger.Object);

            //assert
            result.Should().BeOfType(typeof(BadRequestObjectResult));
            var badResult = result as BadRequestObjectResult;
            badResult.Value.ToString().Should().Contain("Invalid Block number");
        }

        [Theory]
        [InlineData("11", "aa")]
        [InlineData("11", "-132")]
        [InlineData("11", "x0aa")]
        [InlineData("11", "")]
        public async Task GetTransOnBlockNumAndAddr_ShouldReturnBadRequst_whenAddrIsInvalid(string blockNum, string addr)
        {
            //arrange
            var mockTransactionsProcessor = new Mock<ITransactionsProcessor>();
            var mockLogger = new Mock<ILogger>();
            var mockTransactionsDetailsService = new Mock<ITransactionsDetailsService>();
            var _testee = new EthereumQueryFunctions(mockTransactionsDetailsService.Object, mockTransactionsProcessor.Object);

            //act
            var result = await _testee.GetTransactionsByBlockNumAndAddress(null, blockNum, addr, mockLogger.Object);

            //assert
            result.Should().BeOfType(typeof(BadRequestObjectResult));
            var badResult = result as BadRequestObjectResult;
            badResult.Value.ToString().Should().Contain("Invalid Address");
        }

        [Theory]
        [InlineData("11", "0Xaa")]
        [InlineData("11", "0xaa")]
        [InlineData("100", "0XAA")]
        [InlineData("13579", "0xAA")]
        public async Task GetTransOnBlockNumAndAddr_ShouldReturnOKRequst_whenInputIsInvalid(string blockNum, string addr)
        {
            //arrange
            var mockTransactionsProcessor = new Mock<ITransactionsProcessor>();
            var mockLogger = new Mock<ILogger>();
            var mockTransactionsDetailsService = new Mock<ITransactionsDetailsService>();
            mockTransactionsProcessor.Setup(_ => _.GetTransactionsByAddress(It.IsAny<List<TransactionsByBlockNumber>>(), It.IsAny<string>()))
                .Returns(new List<GetTransactionsResponse>());
            var _testee = new EthereumQueryFunctions(mockTransactionsDetailsService.Object, mockTransactionsProcessor.Object);

            //act
            var result = await _testee.GetTransactionsByBlockNumAndAddress(null, blockNum, addr, mockLogger.Object);

            //assert
            result.Should().BeOfType(typeof(OkObjectResult));
        }


    }
}
