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
using System.Net.Http;
using EthereumQuery.HttpMessageHandlers;
using System.Net;
using Moq.Protected;
using System.Threading;

namespace EthereumQuery.Test.HttpMessageHandlersTests
{
    public class EtherHttpMessageHandlerTests
    {
        [Fact]
        public async Task PostAsync_ShouldReturnHttpRequestMessage()
        {
            //arrange
            var handlerMock = new Mock<HttpMessageHandler>();
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(@"{ ""id"": 1, ""title"": ""Hello BTC""}"),
            };
            handlerMock
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>())
               .ReturnsAsync(response);
            var mockHttpClient = new HttpClient(handlerMock.Object);
            var _testee = new EtherHttpMessageHandler(mockHttpClient);

            //act
            var result = await _testee.PostAsync("someText", default);

            //assert
            handlerMock.Protected().Verify(
               "SendAsync",
               Times.Exactly(1),
               ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Post),
               ItExpr.IsAny<CancellationToken>());
            result.Should().NotBeNull();
            var contentString = await result.Content.ReadAsStringAsync();
            contentString.Should().Contain("Hello BTC");

        }
    }
}
