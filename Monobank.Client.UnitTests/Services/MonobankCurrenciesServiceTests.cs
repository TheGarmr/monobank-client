using Monobank.Client.Interfaces;
using Monobank.Client.Services;
using Moq;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Monobank.Client.Models;
using Xunit;

namespace Monobank.Client.UnitTests.Services;

public class MonobankCurrenciesServiceTests
{
    private readonly Mock<IMonobankClient> _monobankClientMock = new();

    #region GetCurrenciesAsync

    [Fact]
    public async Task GetCurrenciesAsync_Should_ReturnCurrenciesFromInternalClient()
    {
        // Arrange
        var expectedCurrencies = new List<CurrencyInfo> { new() { CurrencyCodeA = 980, CurrencyCodeB = 105 } };
        _monobankClientMock
            .Setup(x => x.GetCurrenciesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedCurrencies);
        var service = GetService();

        // Act
        var result = await service.GetCurrenciesAsync(CancellationToken.None);
        Assert.Equal(expectedCurrencies, result);
        _monobankClientMock.Verify(x => x.GetCurrenciesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion

    #region PrivateMethods

    private MonobankCurrenciesService GetService()
    {
        return new MonobankCurrenciesService(_monobankClientMock.Object);
    }

    #endregion
}