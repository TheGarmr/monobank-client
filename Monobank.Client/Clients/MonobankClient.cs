using Monobank.Client.Exceptions;
using Monobank.Client.Models;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using System;
using System.Collections.Generic;
using System.Text.Json;
using Monobank.Client.Interfaces;
using System.Text;

namespace Monobank.Client.Clients
{
    internal class MonobankClient : IMonobankClient
    {
        private const string ClientInfoEndpoint = "personal/client-info";
        private const string StatementEndpoint = "personal/statement";
        private const string WebhookEndpoint = "personal/webhook";
        private const string CurrencyEndpoint = "bank/currency";
        private const string TokenHeader = "X-Token";
        private readonly HttpClient _httpClient;

        public MonobankClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        
        public async Task<ClientInfo> GetClientInfoAsync(string token, CancellationToken cancellationToken = default)
        {
            UpdateAuthHeader(token);

            var requestUri = new Uri(ClientInfoEndpoint, UriKind.Relative);
            var response = await _httpClient.GetAsync(requestUri, cancellationToken);
            return await HandleResponse<ClientInfo>(response);
        }

        public async Task<bool> SetWebhookAsync(string url, string token, CancellationToken cancellationToken = default)
        {
            UpdateAuthHeader(token);

            var requestUri = new Uri(WebhookEndpoint, UriKind.Relative);
            var request = JsonSerializer.Serialize(new { webHookUrl = url });
            var content = new StringContent(request, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(requestUri, content, cancellationToken);

            return response.IsSuccessStatusCode;
        }

        public async Task<ICollection<Statement>> GetStatementsAsync(string token, int from, int to, string cardId, CancellationToken cancellationToken = default)
        {
            UpdateAuthHeader(token);

            var uri = new Uri($"{StatementEndpoint}/{cardId}/{from}/{to}", UriKind.Relative);
            var response = await _httpClient.GetAsync(uri, cancellationToken);
            return await HandleResponse<ICollection<Statement>>(response);
        }

        public async Task<ICollection<CurrencyInfo>> GetCurrenciesAsync(CancellationToken cancellationToken = default)
        {
            var uri = new Uri($"{CurrencyEndpoint}", UriKind.Relative);
            var response = await _httpClient.GetAsync(uri, cancellationToken);
            return await HandleResponse<ICollection<CurrencyInfo>>(response);
        }

        private void UpdateAuthHeader(string token)
        {
            _httpClient.DefaultRequestHeaders.Remove(TokenHeader);
            _httpClient.DefaultRequestHeaders.Add(TokenHeader, token);
        }

        private static async Task<T> HandleResponse<T>(HttpResponseMessage response)
        {
            var responseString = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                var error = JsonSerializer.Deserialize<MonobankApiError>(responseString);
                throw MonobankClientApiException.Create(error?.Description);
            }

            return JsonSerializer.Deserialize<T>(responseString);
        }
    }
}