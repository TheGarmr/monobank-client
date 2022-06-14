using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Microsoft.Extensions.Logging;
using Monobank.Client.Models;
using Xunit;

namespace Monobank.Client.UnitTests
{
    public class MonobankClientTests
    {
        private readonly IMock<ILogger<MonobankClient>> _loggerMock;
        private readonly IMock<HttpClient> _httpClientMock;

        public MonobankClientTests()
        {
            _loggerMock = new Mock<ILogger<MonobankClient>>();
            _httpClientMock = new Mock<HttpClient>();
        }

        #region Constructor

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("               ")]
        public void MonobankClientShouldThrowMonobankClientExceptionIfApiBaseUrlIsInvalid(string apiBaseUrl)
        {
            // Arrange
            var options = new MonobankClientOptions
            {
                ApiBaseUrl = apiBaseUrl
            };

            // Assert
            var exception = Assert.Throws<MonobankClientException>(() =>
            {
                new MonobankClient(null, options, _loggerMock.Object);
            });
            Assert.Contains(nameof(options.ApiBaseUrl), exception.Message);
        }

        [Fact]
        public void MonobankClientShouldThrowMonobankClientExceptionIfMaxStatementRangeIsInvalid()
        {
            // Arrange
            var options = new MonobankClientOptions
            {
                ApiBaseUrl = "some-valid-api-url",
                MaxStatementRange = new TimeSpan()
            };

            // Assert
            var exception = Assert.Throws<MonobankClientException>(() =>
            {
                new MonobankClient(null, options, _loggerMock.Object);
            });
            Assert.Contains(nameof(options.ApiBaseUrl), exception.Message);
        }

        #endregion

        #region GetClientInfoAsync

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("               ")]
        public async Task GetClientInfoAsyncShouldThrowMonobankClientExceptionIfUserTokenIsInvalid(string token)
        {
            // Arrange
            var client = GetClient();

            // Act
            await Assert.ThrowsAsync<MonobankClientException>(async () =>
            {
                await client.GetClientInfoAsync(token, CancellationToken.None);
            });
        }

        #endregion

        #region SetWebhookAsync

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("               ")]
        public async Task SetWebhookAsyncShouldThrowMonobankClientExceptionIfWebHookUrlIsInvalid(string webHookUrl)
        {
            // Arrange
            var client = GetClient();

            // Act
            await Assert.ThrowsAsync<MonobankClientException>(async () =>
            {
                await client.SetWebhookAsync(webHookUrl, "some-valid-token", CancellationToken.None);
            });
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("               ")]
        public async Task SetWebhookAsyncShouldThrowMonobankClientExceptionIfUserTokenIsInvalid(string token)
        {
            // Arrange
            var client = GetClient();

            // Act
            await Assert.ThrowsAsync<MonobankClientException>(async () =>
            {
                await client.SetWebhookAsync("some-valid-url", token, CancellationToken.None);
            });
        }

        #endregion

        #region SetWebhookAsync

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("               ")]
        public async Task GetStatementsAsyncShouldThrowMonobankClientExceptionIfWebHookUrlIsInvalid(string cardId)
        {
            // Arrange
            var client = GetClient();

            // Act
            await Assert.ThrowsAsync<MonobankClientException>(async () =>
            {
                await client.GetStatementsAsync("some-valid-tone", DateTime.UtcNow, DateTime.UtcNow, cardId, CancellationToken.None);
            });
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("               ")]
        public async Task GetStatementsAsyncShouldThrowMonobankClientExceptionIfUserTokenIsInvalid(string token)
        {
            // Arrange
            var client = GetClient();
            
            // Act
            await Assert.ThrowsAsync<MonobankClientException>(async () =>
            {
                await client.GetStatementsAsync(token, DateTime.UtcNow, DateTime.UtcNow, "some-valid-card-id", CancellationToken.None);
            });
        }

        [Fact]
        public async Task GetStatementsAsyncShouldThrowMonobankClientExceptionIfFromDateBiggerThanToDateMuchThanMaxStatementRange()
        {
            // Arrange
            var maxStatementRange = TimeSpan.FromDays(10);
            var options = new MonobankClientOptions();
            var client = GetClient(options);


            var dateTo = DateTime.UtcNow;
            var dateFrom = dateTo - maxStatementRange - TimeSpan.FromSeconds(1);

            // Act
            await Assert.ThrowsAsync<MonobankClientException>(async () =>
            {
                await client.GetStatementsAsync("some-valid-token", dateFrom, dateTo, "some-valid-card-id", CancellationToken.None);
            });
        }

        #endregion

        private MonobankClient GetClient(MonobankClientOptions options = null)
        {
            options ??= GetValidClientOptions();
            return new MonobankClient(_httpClientMock.Object, options, _loggerMock.Object);
        }

        private MonobankClientOptions GetValidClientOptions()
        {
            return new MonobankClientOptions { ApiBaseUrl = "https://example.com" };
        }
    }
}
