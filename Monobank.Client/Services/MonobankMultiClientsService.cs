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
    internal class MonobankMultiClientsService : IMonobankMultiClientsService
    {
        private readonly IMonobankClient _client;
        private readonly double _maxStatementRangeInSeconds;

        public MonobankMultiClientsService(IMonobankClient client, IOptions<MonobankClientOptions> options)
        {
            _client = client;
            _maxStatementRangeInSeconds = options.Value.MaxStatementRange.TotalSeconds;
        }

        public Task<ClientInfo> GetClientInfoAsync(string token, CancellationToken cancellationToken = default)
        {
            ValidateToken(token);

            return _client.GetClientInfoAsync(token, cancellationToken);
        }

        public Task<bool> SetWebhookAsync(string url, string token, CancellationToken cancellationToken = default)
        {
            ValidateToken(token);

            if (string.IsNullOrWhiteSpace(url))
            {
                throw MonobankClientInvalidWebhookException.Create(url);
            }

            return _client.SetWebhookAsync(url, token, cancellationToken);
        }

        public Task<ICollection<Statement>> GetStatementsAsync(string token, DateTime from, DateTime to, string cardId, CancellationToken cancellationToken = default)
        {
            ValidateToken(token);

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

            return _client.GetStatementsAsync(token, dateFromInUnixTime, dateToInUnixTime, cardId, cancellationToken);
        }

        private static void ValidateToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw MonobankClientInvalidTokenException.Create(token);
            }
        }
    }
}