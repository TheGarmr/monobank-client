using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Monobank.Client.Models;

namespace Monobank.Client
{
    public interface IMonobankClient
    {
        Task<UserInfo> GetClientInfoAsync(string token, CancellationToken cancellationToken = default);
        Task<ICollection<Statement>> GetStatementsAsync(string token, DateTime from, DateTime to, string account = "0", CancellationToken cancellationToken = default);
        Task<bool> SetWebhookAsync(string url, string token, CancellationToken cancellationToken = default);
        Task<ICollection<CurrencyInfo>> GetCurrenciesAsync(CancellationToken cancellationToken = default);
    }
}