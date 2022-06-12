using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Monobank.Client.Models;

namespace Monobank.Client
{
    public interface IMonobankClient
    {
        Task<UserInfo> GetClientInfoAsync(string token, CancellationToken cancellationToken);
        Task<ICollection<Statement>> GetStatementsAsync(DateTime from, DateTime to, string account = "0");
        Task<bool> SetWebhookAsync(string url, string token, CancellationToken cancellationToken);
        Task<ICollection<CurrencyInfo>> GetCurrenciesAsync(CancellationToken cancellationToken);
    }
}