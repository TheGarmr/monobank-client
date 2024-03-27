using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Monobank.Client.Interfaces;
using Monobank.Client.Models;

namespace Monobank.Client.Services
{
    internal class MonobankCurrenciesService : IMonobankCurrenciesService
    {
        private readonly IMonobankClient _client;

        public MonobankCurrenciesService(IMonobankClient client)
        {
            _client = client;
        }

        public Task<ICollection<CurrencyInfo>> GetCurrenciesAsync(CancellationToken cancellationToken = default)
        {
            return _client.GetCurrenciesAsync(cancellationToken);
        }
    }
}