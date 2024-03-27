using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;
using Monobank.Client.Exceptions;
using Monobank.Client.Interfaces;
using Monobank.Client.Options;
using Monobank.Client.Services;
using Monobank.Client.Models;
using System.Collections.Generic;

namespace Monobank.Client.UnitTests.Services
{
    public class MonobankMultiClientsServiceTests
    {
        private readonly Mock<IMonobankClient> _monobankClientMock = new();

        #region GetClientInfoAsync

        [Fact]
        public async Task GetClientInfoAsync_Should_ReturnClientInfoFromInternalClient()
        {
            // Arrange
            var expectedInfo = new ClientInfo { Id = Guid.NewGuid().ToString()};
            _monobankClientMock
                .Setup(x =>
                    x.GetClientInfoAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedInfo);
            var dateFrom = DateTime.UtcNow.AddHours(-5);
            var dateTo = dateFrom.AddHours(1);
            var service = GetService();

            // Act
            var result = await service.GetClientInfoAsync("some-valid-token", CancellationToken.None);
            Assert.Equal(expectedInfo, result);
            _monobankClientMock.Verify(x =>
                x.GetClientInfoAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("               ")]
        public async Task GetClientInfoAsyncShouldThrowMonobankClientExceptionIfUserTokenIsInvalid(string token)
        {
            // Arrange
            var service = GetService();
            var expectedMessage = $"User token {token} is invalid.";

            // Act
            var exception = await Assert.ThrowsAsync<MonobankClientInvalidTokenException>(async () =>
            {
                await service.GetClientInfoAsync(token, CancellationToken.None);
            });
            Assert.Equal(expectedMessage, exception.Message);
            _monobankClientMock.Verify(x => x.GetClientInfoAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        #endregion

        #region SetWebhookAsync

        [Fact]
        public async Task SetWebhookAsync_Should_SetWebhookWithInternalClient()
        {
            // Arrange
            _monobankClientMock
                .Setup(x => x.SetWebhookAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);
            var service = GetService();

            // Act
            var result = await service.SetWebhookAsync("some-valid-webhook-url", "some-valid-token", CancellationToken.None);
            Assert.True(result);
            _monobankClientMock.Verify(x => x.SetWebhookAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("               ")]
        public async Task SetWebhookAsyncShouldThrowMonobankClientExceptionIfWebHookUrlIsInvalid(string webHookUrl)
        {
            // Arrange
            var service = GetService();
            var expectedMessage = $"Webhook url {webHookUrl} is invalid.";

            // Act
            var exception = await Assert.ThrowsAsync<MonobankClientInvalidWebhookException>(async () =>
            {
                await service.SetWebhookAsync(webHookUrl, "some-valid-token", CancellationToken.None);
            });
            Assert.Equal(expectedMessage, exception.Message);
            _monobankClientMock.Verify(x => x.SetWebhookAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("               ")]
        public async Task SetWebhookAsyncShouldThrowMonobankClientExceptionIfUserTokenIsInvalid(string token)
        {
            // Arrange
            var service = GetService();
            var expectedMessage = $"User token {token} is invalid.";

            // Act
            var exception = await Assert.ThrowsAsync<MonobankClientInvalidTokenException>(async () =>
            {
                await service.SetWebhookAsync("some-valid-url", token, CancellationToken.None);
            });
            Assert.Equal(expectedMessage, exception.Message);
            _monobankClientMock.Verify(x => x.SetWebhookAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        #endregion

        #region GetStatementsAsync

        [Fact]
        public async Task GetStatementsAsync_Should_ReturnStatementsFromInternalClient()
        {
            // Arrange
            var expectedStatements = new List<Statement> { new() { Balance = 100500 } };
            _monobankClientMock
                .Setup(x =>
                    x.GetStatementsAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedStatements);
            var dateFrom = DateTime.UtcNow.AddHours(-5);
            var dateTo = dateFrom.AddHours(1);
            var service = GetService();

            // Act
            var result = await service.GetStatementsAsync("some-valid-token", dateFrom, dateTo, "some-valid-card-id", CancellationToken.None);
            Assert.Equal(expectedStatements, result);
            _monobankClientMock.Verify(x =>
                x.GetStatementsAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("               ")]
        public async Task GetStatementsAsyncShouldThrowMonobankClientExceptionIfCardIdIsInvalid(string cardId)
        {
            // Arrange
            var service = GetService();
            var expectedMessage = $"Card id {cardId} is invalid.";

            // Act
            var exception = await Assert.ThrowsAsync<MonobankClientInvalidCardIdException>(async () =>
            {
                await service.GetStatementsAsync("some-valid-token", DateTime.UtcNow, DateTime.UtcNow, cardId, CancellationToken.None);
            });
            Assert.Equal(expectedMessage, exception.Message);
            _monobankClientMock
                .Verify(x =>
                        x.GetStatementsAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<CancellationToken>()),
                    Times.Never);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("               ")]
        public async Task GetStatementsAsyncShouldThrowMonobankClientExceptionIfUserTokenIsInvalid(string token)
        {
            // Arrange
            var service = GetService();
            var expectedMessage = $"User token {token} is invalid.";

            // Act
            var exception = await Assert.ThrowsAsync<MonobankClientInvalidTokenException>(async () =>
            {
                await service.GetStatementsAsync(token, DateTime.UtcNow, DateTime.UtcNow, "some-valid-card-id", CancellationToken.None);
            });
            Assert.Equal(expectedMessage, exception.Message);
            _monobankClientMock
                .Verify(x => 
                    x.GetStatementsAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<CancellationToken>()),
                    Times.Never);
        }

        [Fact]
        public async Task GetStatementsAsyncShouldThrowMonobankClientExceptionIfFromDateBiggerThanToDateMuchThanMaxStatementRange()
        {
            // Arrange
            var maxStatementRange = TimeSpan.FromDays(31) + TimeSpan.FromHours(1);
            var service = GetService();
            
            var dateTo = DateTime.UtcNow;
            var dateFrom = dateTo - maxStatementRange - TimeSpan.FromSeconds(1);
            var expectedMessage = $"Time range between {dateFrom} and {dateTo} is invalid. Difference between 'from' and 'to' should be less than 31 day + 1 hour.";

            // Act
            var exception = await Assert.ThrowsAsync<MonobankClientInvalidDateRange>(async () =>
            {
                await service.GetStatementsAsync("some-valid-token", dateFrom, dateTo, "some-valid-card-id", CancellationToken.None);
            });
            Assert.Equal(expectedMessage, exception.Message);
            _monobankClientMock
                .Verify(x =>
                        x.GetStatementsAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<CancellationToken>()),
                    Times.Never);
        }

        #endregion

        #region PrivateMethods

        private MonobankMultiClientsService GetService(MonobankClientOptions options = null)
        {
            options ??= GetValidClientOptions();
            return new MonobankMultiClientsService(_monobankClientMock.Object, Microsoft.Extensions.Options.Options.Create(options));
        }

        private MonobankClientOptions GetValidClientOptions()
        {
            return new MonobankClientOptions { ApiBaseUrl = "https://api.monobank.com" };
        }

        #endregion
    }
}
