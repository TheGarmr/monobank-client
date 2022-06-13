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
        private const int RequestLimit = 60; // seconds
        private const int MaxStatementRange = 2682000; // 31 day + 1 hour
        private readonly HttpClient _httpClient;
        private DateTime _previousRequestTimestamp = DateTime.UtcNow.AddSeconds(-RequestLimit);

        public MonobankClient(HttpClient client, MonobankClientOptions options, ILogger<MonobankClient> logger)
        {
            if (string.IsNullOrWhiteSpace(options.ApiBaseUrl))
            {
                logger.LogCritical("Critical error: ApiBaseUrl config is not provided.");
                throw new ArgumentException("Critical error: ApiBaseUrl config is not provided.", nameof(options.ApiBaseUrl));
            }

            _httpClient = client;
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(ResponseMediaType));
            _httpClient.BaseAddress = new Uri(options.ApiBaseUrl);
        }

        public async Task<UserInfo> GetClientInfoAsync(string token, CancellationToken cancellationToken = default)
        {
            _httpClient.DefaultRequestHeaders.Remove(TokenHeader);
            _httpClient.DefaultRequestHeaders.Add(TokenHeader, token);

            var requestUri = new Uri(ClientInfoEndpoint, UriKind.Relative);
            var response = await _httpClient.GetAsync(requestUri, cancellationToken);
            response.EnsureSuccessStatusCode();
            var responseStr = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<UserInfo>(responseStr);
        }

        public async Task<bool> SetWebhookAsync(string url, string token, CancellationToken cancellationToken = default)
        {
            _httpClient.DefaultRequestHeaders.Remove(TokenHeader);
            _httpClient.DefaultRequestHeaders.Add(TokenHeader, token);

            var requestUri = new Uri(WebhookEndpoint, UriKind.Relative);
            string request = JsonSerializer.Serialize(new { WebHook = url });
            var content = new StringContent(request, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(requestUri, content, cancellationToken);

            return response.IsSuccessStatusCode;
        }

        public async Task<ICollection<Statement>> GetStatementsAsync(string token, DateTime from, DateTime to, string account = "0", CancellationToken cancellationToken = default)
        {
            _httpClient.DefaultRequestHeaders.Remove(TokenHeader);
            _httpClient.DefaultRequestHeaders.Add(TokenHeader, token);

            if (to.ToUnixTime() - from.ToUnixTime() >= MaxStatementRange)
            {
                throw new Exception("Time range exceeded. Difference between 'from' and 'to' should be less than 31 day + 1 hour.");
            }

            if ((DateTime.UtcNow - _previousRequestTimestamp).TotalSeconds <= RequestLimit)
            {
                throw new Exception($"Request limit exceeded. Only 1 request per {RequestLimit} seconds allowed.");
            }

            var uri = new Uri($"{StatementEndpoint}/{account}/{from.ToUnixTime()}/{to.ToUnixTime()}", UriKind.Relative);
            var response = await _httpClient.GetAsync(uri, cancellationToken);
            var responseString = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                var error = JsonSerializer.Deserialize<MonobankApiError>(responseString);
                throw new Exception(error.Description);
            }
            _previousRequestTimestamp = DateTime.UtcNow;
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
                throw new Exception(error.Description);
            }

            return JsonSerializer.Deserialize<ICollection<CurrencyInfo>>(responseString);
        }
    }
}