using ISO._4217;
using System.Text.Json.Serialization;
using Monobank.Client.Extensions;

namespace Monobank.Client.Models
{
    public class Jar
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("sendId")]
        public string SendId { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("currencyCode")]
        public int CurrencyCode { get; set; }

        [JsonPropertyName("balance")]
        public long Balance { get; set; }

        [JsonPropertyName("goal")]
        public long? Goal { get; set; }

        #region Custom properties

        public string CurrencyName => CurrencyCodesResolver.GetCodeByNumber(CurrencyCode);

        public double BalanceAsMoney => Balance.AsMoney();

        public double GoalAsMoney => Goal.AsMoney();

        #endregion
    }
}