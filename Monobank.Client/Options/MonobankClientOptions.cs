using System;

namespace Monobank.Client.Options
{
    public class MonobankClientOptions
    {
        internal string ApiBaseUrl { get; set; } = "https://api.monobank.ua/";
        internal TimeSpan MaxStatementRange => TimeSpan.FromDays(31) + TimeSpan.FromHours(1);
        internal int HttpClientTimeoutInSeconds { get; set; } = 30;

        /// <summary>
        /// Token for personal access to the API.
        /// </summary>
        internal string ApiToken { get; set; } = string.Empty;
    }
}