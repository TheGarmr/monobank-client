using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Monobank.Client.HealthChecks
{
    internal class MonobankApiHealthCheck : IHealthCheck
    {
        private readonly HttpClient _httpClient;
        private readonly string _healthEndpoint;

        internal MonobankApiHealthCheck(HttpClient httpClient, string healthEndpoint)
        {
            _httpClient = httpClient;
            _healthEndpoint = healthEndpoint;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.GetAsync(_healthEndpoint, cancellationToken);
            response.EnsureSuccessStatusCode();

            return HealthCheckResult.Healthy();
        }
    }
}