using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Monobank.Client.Extensions;
using Monobank.Client.Models;

namespace Monobank.Client
{
    public class MonobankClient : IMonobankClient
    {
        private const string ResponseMediaType = "application/json";
        private const string ClientInfoEndpoint = "personal/client-info";
        private const string StatementEndpoint = "personal/statement";
        private const string WebhookEndpoint = "personal/webhook";
        private const string CurrencyEndpoint = "bank/currency";
        private const string TokenHeader = "X-Token";
        private readonly HttpClient _httpClient;
        private readonly double _maxStatementRangeInSeconds;

        public MonobankClient(HttpClient client, MonobankClientOptions options, ILogger<MonobankClient> logger)
        {
            if (string.IsNullOrWhiteSpace(options.ApiBaseUrl))
            {
                logger.LogCritical($"Critical error: {nameof(options.ApiBaseUrl)} config is not provided.");
                throw new MonobankClientException($"Critical error: {nameof(options.ApiBaseUrl)} config is not provided.");
            }

            if (options.MaxStatementRange.TotalSeconds <= 0)
            {
                logger.LogWarning($"{nameof(options.MaxStatementRange)} config is not provided.");
                throw new MonobankClientException($"Critical error: {nameof(options.ApiBaseUrl)} config is not provided.");
            }

            _httpClient = client;
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(ResponseMediaType));
            _httpClient.BaseAddress = new Uri(options.ApiBaseUrl);
            _maxStatementRangeInSeconds = options.MaxStatementRange.TotalSeconds;
        }

        public async Task<UserInfo> GetClientInfoAsync(string token, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new MonobankClientException("User token is invalid.");
            }

            _httpClient.DefaultRequestHeaders.Remove(TokenHeader);
            _httpClient.DefaultRequestHeaders.Add(TokenHeader, token);

            var requestUri = new Uri(ClientInfoEndpoint, UriKind.Relative);
            var response = await _httpClient.GetAsync(requestUri, cancellationToken);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                var error = JsonSerializer.Deserialize<MonobankApiError>(responseString);
                throw new MonobankClientException(error?.Description);
            }
            return JsonSerializer.Deserialize<UserInfo>(responseString);
        }

        public async Task<bool> SetWebhookAsync(string url, string token, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new MonobankClientException("User token is invalid.");
            }

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new MonobankClientException("Webhook url is invalid.");
            }

            _httpClient.DefaultRequestHeaders.Remove(TokenHeader);
            _httpClient.DefaultRequestHeaders.Add(TokenHeader, token);

            var requestUri = new Uri(WebhookEndpoint, UriKind.Relative);
            var request = JsonSerializer.Serialize(new { WebHook = url });
            var content = new StringContent(request, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(requestUri, content, cancellationToken);

            return response.IsSuccessStatusCode;
        }

        public async Task<ICollection<Statement>> GetStatementsAsync(string token, DateTime from, DateTime to, string cardId, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new MonobankClientException("User token is invalid.");
            }

            if (string.IsNullOrWhiteSpace(cardId))
            {
                throw new MonobankClientException("Card id is invalid.");
            }

            var dateFromInUnixTime = to.ToUnixTime();
            var dateToInUnixTime = to.ToUnixTime();

            _httpClient.DefaultRequestHeaders.Remove(TokenHeader);
            _httpClient.DefaultRequestHeaders.Add(TokenHeader, token);

            if (dateToInUnixTime - dateFromInUnixTime >= _maxStatementRangeInSeconds)
            {
                throw new MonobankClientException("Time range exceeded. Difference between 'from' and 'to' should be less than 31 day + 1 hour.");
            }

            var uri = new Uri($"{StatementEndpoint}/{cardId}/{dateFromInUnixTime}/{dateToInUnixTime}", UriKind.Relative);
            var response = await _httpClient.GetAsync(uri, cancellationToken);
            var responseString = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                var error = JsonSerializer.Deserialize<MonobankApiError>(responseString);
                throw new MonobankClientException(error?.Description);
            }

            return JsonSerializer.Deserialize<ICollection<Statement>>(responseString);
        }

        public async Task<ICollection<CurrencyInfo>> GetCurrenciesAsync(CancellationToken cancellationToken = default)
        {
            var uri = new Uri($"{CurrencyEndpoint}", UriKind.Relative);
            var response = await _httpClient.GetAsync(uri, cancellationToken);
            var responseString = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                var error = JsonSerializer.Deserialize<MonobankApiError>(responseString);
                throw new MonobankClientException(error?.Description);
            }

            return JsonSerializer.Deserialize<ICollection<CurrencyInfo>>(responseString);
        }
    }
}