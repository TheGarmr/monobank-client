using System.Text.Json.Serialization;
using ISO._4217;

namespace Monobank.Client.Models
{
    public sealed class CurrencyInfo
    {
        /// <summary>
        /// Код валюти рахунку A відповідно ISO 4217
        /// </summary>
        [JsonPropertyName("currencyCodeA")]
        public int CurrencyCodeA { get; set; }

        /// <summary>
        /// Код валюти рахунку B відповідно ISO 4217
        /// </summary>
        [JsonPropertyName("currencyCodeB")]
        public int CurrencyCodeB { get; set; }

        /// <summary>
        /// Час курсу в секундах в форматі Unix time
        /// </summary>
        [JsonPropertyName("date")]
        public long Date { get; set; }

        /// <summary>
        /// Курс продажу валюти A
        /// </summary>
        [JsonPropertyName("rateSell")]
        public float RateSell { get; set; }

        /// <summary>
        /// Курс купівлі валюти A
        /// </summary>
        [JsonPropertyName("rateBuy")]
        public float RateBuy { get; set; }

        /// <summary>
        /// Курс обміну між валютами A та B
        /// </summary>
        [JsonPropertyName("rateCross")]
        public float RateCross { get; set; }

        #region Custom properties

        /// <summary>
        /// Назва валюти рахунку A.
        /// </summary>
        public string CurrencyNameA => CurrencyCodesResolver.GetCodeByNumber(CurrencyCodeA);

        /// <summary>
        /// Назва валюти рахунку B.
        /// </summary>
        public string CurrencyNameB => CurrencyCodesResolver.GetCodeByNumber(CurrencyCodeB);

        #endregion
    }
}
