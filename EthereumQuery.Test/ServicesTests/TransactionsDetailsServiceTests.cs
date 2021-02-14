using Xunit;
using FluentAssertions;
using System.Linq;
using Microsoft.Extensions.Logging;
using Moq;
using EthereumQuery.Services;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using EthereumQuery.HttpMessageHandlers;

namespace EthereumQuery.Test.ServicesTests
{
    public class TransactionsDetailsServiceTests
    {

        [Fact]
        public async Task GetTransactions_ReturnNullList_WhenReturnTypeIsNotOK()
        {
            //arrange
            var mockEtherHttpMessageHandler = new Mock<IEtherHttpMessageHandler>();
            var mockLogger = new Mock<ILogger<TransactionsDetailsService>>();
            mockEtherHttpMessageHandler.Setup(_ => _.PostAsync(It.IsAny<string>(), default))
               .ReturnsAsync(GenerateNullReuslt());

            var _testee = new TransactionsDetailsService(mockEtherHttpMessageHandler.Object, mockLogger.Object);
            int blockNum = 1122334455;

            //act
            var result = await _testee.GetTransactions(blockNum, default);

            //assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetTransactions_ReturnEmptyList_WhenTransactionsAreNotFound()
        {
            //arrange
            var mockEtherHttpMessageHandler = new Mock<IEtherHttpMessageHandler>();
            var mockLogger = new Mock<ILogger<TransactionsDetailsService>>();
            mockEtherHttpMessageHandler.Setup(_ => _.PostAsync(It.IsAny<string>(), default))
               .ReturnsAsync(GenerateEmptyTransactionReuslt());

            var _testee = new TransactionsDetailsService(mockEtherHttpMessageHandler.Object, mockLogger.Object);
            int blockNum = 139;

            //act
            var result = await _testee.GetTransactions(blockNum, default);

            //assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetTransactions_ReturnValidList()
        {
            //arrange
            var mockEtherHttpMessageHandler = new Mock<IEtherHttpMessageHandler>();
            var mockLogger = new Mock<ILogger<TransactionsDetailsService>>();
            mockEtherHttpMessageHandler.Setup(_ => _.PostAsync(It.IsAny<string>(), default))
               .ReturnsAsync(GenerateValidQueryReuslt());

            var _testee = new TransactionsDetailsService(mockEtherHttpMessageHandler.Object, mockLogger.Object);
            int blockNum = 11;

            //act
            var result = await _testee.GetTransactions(blockNum, default);

            //assert
            result.Should().NotBeNull();
            result.Count().Should().Be(2);
        }

        private HttpResponseMessage GenerateValidQueryReuslt()
        {
            return new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(@"{ ""jsonrpc"": ""2.0"", ""id"": 1, ""result"" : { ""difficulty"" : ""0x92acba0bc9ee9"", ""transactions"" : 
                        [{""blockHash"" : ""0x6dbde4b224013c46537231c548bd6ff8e2a2c927c435993d351866d505c523f1"", ""blockNumber"":""0x8b99c9"",""from"": ""0xc779a4bdc3696baf2a6d62ddfc2d0664d3c4fd7f"",
                          ""gas"":""0x5208"",""gasPrice"":""0x12a05f2000"", ""hash"": ""0x827c9a4a1ae2cf9c20fa1dad305b4b8f8f336aab52ff9563379a8a9dbf0d1727"",
                            ""value"": ""0x4115f164490000"",""v"":""0x25""},
                        {""blockHash"" : ""0x6dbde4b224013c46537231c548bd6ff8e2a2c927c435993d351866d505c523f2"", ""blockNumber"":""0x8b99b9"",""from"": ""0xc779a4bdc3696baf2a6d62ddfc2d0664d3c4fd7f"",
                            ""gas"":""0x5208"",""gasPrice"":""0x12a05f2000"", ""hash"": ""0x827c9a4a1ae2cf9c20fa1dad305b4b8f8f336aab52ff9563379a8a9dbf0d1728"",
                        ""value"": ""0x4115f1644900"",""v"":""0x26""}]}}"),
            };
        }
        private HttpResponseMessage GenerateEmptyTransactionReuslt()
        {
            return new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(@"{ ""jsonrpc"": ""2.0"", ""id"": 1, ""result"" : { ""difficulty"" : ""0x92acba0bc9ee9"", ""transactions"":[]}}"),
            };
        }

        private HttpResponseMessage GenerateNullReuslt()
        {
            return new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(@"{""jsonrpc"":""2.0"",""id"":1,""result"":null}"),
            };
        }




    }
}
