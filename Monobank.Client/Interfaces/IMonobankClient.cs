using Monobank.Client.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace Monobank.Client.Interfaces
{
    internal interface IMonobankClient
    {
        Task<ClientInfo> GetClientInfoAsync(string token, CancellationToken cancellationToken = default);
        Task<ICollection<Statement>> GetStatementsAsync(string token, int from, int to, string cardId, CancellationToken cancellationToken = default);
        Task<bool> SetWebhookAsync(string url, string token, CancellationToken cancellationToken = default);
        Task<ICollection<CurrencyInfo>> GetCurrenciesAsync(CancellationToken cancellationToken = default);
    }
}