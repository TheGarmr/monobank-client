using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Monobank.Client.Exceptions;
using Monobank.Client.Extensions;
using Monobank.Client.Interfaces;
using Monobank.Client.Models;
using Monobank.Client.Options;

namespace Monobank.Client.Services
{
    internal class MonobankSingleClientService : IMonobankSingleClientService
    {
        private readonly IMonobankClient _client;
        private readonly double _maxStatementRangeInSeconds;
        private readonly string _token;

        public MonobankSingleClientService(IMonobankClient client, IOptions<MonobankClientOptions> options)
        {
            _client = client;
            _maxStatementRangeInSeconds = options.Value.MaxStatementRange.TotalSeconds;
            _token = options.Value.ApiToken;
        }

        public Task<ClientInfo> GetClientInfoAsync(CancellationToken cancellationToken = default)
        {
            return _client.GetClientInfoAsync(_token, cancellationToken);
        }

        public Task<bool> SetWebhookAsync(string url, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                throw MonobankClientInvalidWebhookException.Create(url);
            }

            return _client.SetWebhookAsync(url, _token, cancellationToken);
        }

        public Task<ICollection<Statement>> GetStatementsAsync(DateTime from, DateTime to, string cardId, CancellationToken cancellationToken = default)
        {

            if (string.IsNullOrWhiteSpace(cardId))
            {
                throw MonobankClientInvalidCardIdException.Create(cardId);
            }

            var dateFromInUnixTime = from.ToUnixTime();
            var dateToInUnixTime = to.ToUnixTime();

            if (dateToInUnixTime - dateFromInUnixTime >= _maxStatementRangeInSeconds)
            {
                throw MonobankClientInvalidDateRange.Create(from, to);
            }

            return _client.GetStatementsAsync(_token, dateFromInUnixTime, dateToInUnixTime, cardId, cancellationToken);
        }
    }
}