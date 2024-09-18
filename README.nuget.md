# MonobankClient

### This library helps to integrate [Monobank open API](https://api.monobank.ua)(client) to your application.
### Full API documentation can be found here: [Monobank open API](https://api.monobank.ua/docs/)

### Functionality
  * [Obtaining exchange rates](https://api.monobank.ua/docs/#tag/Publichni-dani/paths/~1bank~1currency/get)
  * [Information about the client](https://api.monobank.ua/docs/#tag/Kliyentski-personalni-dani/paths/~1personal~1client-info/get)
  * [Set up WebHook](https://api.monobank.ua/docs/#tag/Kliyentski-personalni-dani/paths/~1personal~1webhook/post)
  * [Extract transactions](https://api.monobank.ua/docs/#tag/Kliyentski-personalni-dani/paths/~1personal~1statement~1{account}~1{from}~1{to}/get)

### API limitations:
  * You can receive information about a client once per a minute
  * Information about currencies refreshes once per 5 minutes

### Quickstart:
  * Go to your [personal profile](https://api.monobank.ua/)
  * Create a token
  * Install the package from [Nuget.org](https://www.nuget.org/packages/MonobankClient/)
  * Add a client using Dependency Injection

### Adding a client using Dependency Injection
You can use this method in DI if you have only one client or need only a currencies client.
Where the `monobank-api` is the section in your appsettings.json file.
```
private static IServiceCollection AddMonobankService(this IServiceCollection services, IConfiguration configuration)
{
    services.AddMonobankSingleClientService(options => configuration.GetSection("monobank-api").Bind(options));
    return services;
}
```

You can use this method in DI if you have multiple clients or need only a currency client.
Where the `monobank-api` is the section in your appsettings.json file.
```
private static IServiceCollection AddMonobankService(this IServiceCollection services, IConfiguration configuration)
{
    services.AddMonobankMultiClientsService(options => configuration.GetSection("monobank-api").Bind(options));
    return services;
}
```

After that you will have the ability to inject a corresponding class with the following interfaces:
  * IMonobankSingleClientService
  * IMonobankMultiClientsService

The difference between these clients is that the `IMonobankSingleClientService` uses the `ApiToken` property from the configuration section and the IMonobankMultiClientsService needs a token each time you call its methods.