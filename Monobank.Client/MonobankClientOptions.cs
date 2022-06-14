using System;

namespace Monobank.Client
{
    public class MonobankClientOptions
    {
        public string ApiBaseUrl { get; set; } = "https://api.monobank.ua/";
        public TimeSpan MaxStatementRange { get; set; } = TimeSpan.FromDays(31) + TimeSpan.FromHours(1);
    }
}