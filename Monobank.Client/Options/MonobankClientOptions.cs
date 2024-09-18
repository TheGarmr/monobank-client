using System;

namespace Monobank.Client.Options
{
    public class MonobankClientOptions
    {
        public string ApiBaseUrl { get; set; } = "https://api.monobank.ua/";
        public TimeSpan MaxStatementRange => TimeSpan.FromDays(31) + TimeSpan.FromHours(1);
        public int HttpClientTimeoutInSeconds { get; set; } = 30;

        /// <summary>
        /// Token for personal access to the API.
        /// </summary>
        public string ApiToken { get; set; } = string.Empty;
    }
}