using Microsoft.Extensions.DependencyInjection;

namespace Monobank.Client.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMonobankClient(this IServiceCollection services)
        {
            services.AddHttpClient<IMonobankClient, MonobankClient>();
            var options = new MonobankClientOptions();
            services.AddSingleton(options);
            return services;
        }

        public static IServiceCollection AddMonobankClient(this IServiceCollection services, MonobankClientOptions options)
        {
            services.AddHttpClient<IMonobankClient, MonobankClient>();
            services.AddSingleton(options);
            return services;
        }
    }
}