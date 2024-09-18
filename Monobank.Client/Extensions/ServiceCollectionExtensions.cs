using Microsoft.Extensions.DependencyInjection;
using Monobank.Client.Interfaces;
using Monobank.Client.Options;
using System;
using System.Net.Http;
using Monobank.Client.Clients;
using Monobank.Client.Services;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Monobank.Client.HealthChecks;

namespace Monobank.Client.Extensions
{
    public static class ServiceCollectionExtensions
    {
        private const string ApiBaseUrl = "https://api.monobank.ua/";
        private const string CurrencyEndpoint = "bank/currency";
        private const string HealthCheckName = "MonobankApi";

        /// <summary>
        /// Adds a Monobank service to the service collection, configured to inject the API token from the configuration and exclusively utilize it.
        /// Also, adds the IMonobankSingleClientService
        /// </summary>
        /// <param name="services">The service collection to which the Monobank single-client service will be added.</param>
        /// <param name="optionsFactory">An action to configure the Monobank client options.</param>
        /// <returns>The modified service collection with the Monobank single-client service added.</returns>
        public static IServiceCollection AddMonobankSingleClientService(this IServiceCollection services, Action<MonobankClientOptions> optionsFactory)
        {
            var monobankOptions = new MonobankClientOptions();
            optionsFactory(monobankOptions);
            services
                .AddOptions<MonobankClientOptions>()
                .Configure(optionsFactory)
                .Validate(options => ValidateOptions(options, true), GetOptionsValidationFailMessage(monobankOptions, true))
                .ValidateOnStart();

            services.AddHttpClient<IMonobankClient, MonobankClient>(HttpClientSetup(monobankOptions));
            services.AddScoped<IMonobankCurrenciesService, MonobankCurrenciesService>();
            services.AddScoped<IMonobankSingleClientService, MonobankSingleClientService>();

            return services;
        }

        /// <summary>
        /// Adds a Monobank service with the ability to use multiple clients/tokens to the service collection.
        /// Also, adds the IMonobankSingleClientService
        /// </summary>
        /// <param name="services">The service collection to which the Monobank multi-clients service will be added.</param>
        /// <param name="optionsFactory">An action to configure the Monobank client options.</param>
        /// <returns>The modified service collection with the Monobank multi-clients service added.</returns>
        public static IServiceCollection AddMonobankMultiClientsService(this IServiceCollection services, Action<MonobankClientOptions> optionsFactory)
        {
            var monobankOptions = new MonobankClientOptions();
            optionsFactory(monobankOptions);
            services
                .AddOptions<MonobankClientOptions>()
                .Configure(optionsFactory)
                .Validate(options => ValidateOptions(options, false), GetOptionsValidationFailMessage(monobankOptions, false))
                .ValidateOnStart();

            services.AddHttpClient<IMonobankClient, MonobankClient>(HttpClientSetup(monobankOptions));
            services.AddScoped<IMonobankCurrenciesService, MonobankCurrenciesService>();
            services.AddScoped<IMonobankMultiClientsService, MonobankMultiClientsService>();

            return services;
        }

        /// <summary>
        /// Adds a health check for the Monobank API to the health checks builder.
        /// </summary>
        /// <param name="builder">The health checks builder.</param>
        /// <param name="baseUri">The base URI of the Monobank API.</param>
        /// <param name="healthEndpoint">The health check endpoint of the Monobank API (currency endpoint may be used).</param>
        /// <param name="healthCheckName">The health check name that will be used to show on the health status page.</param>
        /// <returns>The health checks builder with the Monobank API health check added.</returns>
        public static IHealthChecksBuilder AddHealthCheckMonobankApi(this IHealthChecksBuilder builder,
            string baseUri = ApiBaseUrl,
            string healthEndpoint = CurrencyEndpoint,
            string healthCheckName = HealthCheckName)
        {
            builder.Add(new HealthCheckRegistration(
                healthCheckName,
                sp =>
                {
                    var httpClient = new HttpClient { BaseAddress = new Uri(baseUri) };
                    return new MonobankApiHealthCheck(httpClient, healthEndpoint);
                },
                failureStatus: HealthStatus.Unhealthy,
                tags: new[] { "ready" }));

            return builder;
        }

        private static Action<HttpClient> HttpClientSetup(MonobankClientOptions options)
        {
            return httpClient =>
            {
                httpClient.BaseAddress = new Uri(options.ApiBaseUrl);
                httpClient.Timeout = new TimeSpan(0, 0, options.HttpClientTimeoutInSeconds);
            };
        }

        private static bool ValidateOptions(MonobankClientOptions options, bool singleClient)
        {
            return !string.IsNullOrWhiteSpace(options.ApiBaseUrl)
                   && options.HttpClientTimeoutInSeconds >= 1
                   && !singleClient || !string.IsNullOrWhiteSpace(options.ApiToken);
        }

        private static string GetOptionsValidationFailMessage(MonobankClientOptions options, bool singleClient)
        {
            var message = $"{nameof(MonobankClientOptions)} are invalid." + Environment.NewLine;

            if (string.IsNullOrWhiteSpace(options.ApiBaseUrl))
            {
                message += $"{nameof(MonobankClientOptions.ApiBaseUrl)} can't be empty, but does" + Environment.NewLine;
            }

            if (options.HttpClientTimeoutInSeconds <= 0)
            {
                message += $"{nameof(MonobankClientOptions.HttpClientTimeoutInSeconds)} should have at least 1 second, but have {options.HttpClientTimeoutInSeconds}" + Environment.NewLine;
            }

            if (singleClient)
            {
                if (string.IsNullOrWhiteSpace(options.ApiToken))
                {
                    message += $"{nameof(MonobankClientOptions.ApiToken)} can't be empty, but does";
                }
            }

            return message;
        }
    }
}