# MonobankClient [<img src="https://img.shields.io/nuget/dt/MonobankClient?style=for-the-badge">](https://www.nuget.org/packages/MonobankClient/)<br>
[<img src="https://img.shields.io/github/v/release/TheGarmr/monobank-client?label=Latest%20GitHub%20release&style=for-the-badge">](https://github.com/TheGarmr/monobank-client/releases/latest)
[<img src="https://img.shields.io/nuget/v/MonobankClient?label=Latest%20Nuget%20version&style=for-the-badge">](https://www.nuget.org/packages/MonobankClient/)<br>

### This application helps to integrate [Monobank open API](https://api.monobank.ua)(client) to your application.
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
  * Add MonobankClient to DI via calling method `services.AddMonobankClient();`
