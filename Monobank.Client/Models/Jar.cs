using ISO._4217;
using System.Text.Json.Serialization;
using Monobank.Client.Extensions;

namespace Monobank.Client.Models
{
    public class Jar
    {
        /// <summary>
        /// Ідентифікатор банки
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>
        /// Ідентифікатор для сервісу https://send.monobank.ua/{sendId}
        /// </summary>
        [JsonPropertyName("sendId")]
        public string SendId { get; set; }

        /// <summary>
        /// Назва банки
        /// </summary>
        [JsonPropertyName("title")]
        public string Title { get; set; }

        /// <summary>
        /// Опис банки
        /// </summary>
        [JsonPropertyName("description")]
        public string Description { get; set; }

        /// <summary>
        /// Код валюти банки відповідно ISO 4217
        /// </summary>
        [JsonPropertyName("currencyCode")]
        public int CurrencyCode { get; set; }

        /// <summary>
        /// Баланс банки в мінімальних одиницях валюти (копійках, центах)
        /// </summary>
        [JsonPropertyName("balance")]
        public long Balance { get; set; }

        /// <summary>
        /// Цільова сума для накопичення в банці в мінімальних одиницях валюти (копійках, центах)
        /// </summary>
        [JsonPropertyName("goal")]
        public long? Goal { get; set; }

        #region Custom properties

        /// <summary>
        /// Назва валюти банки, отримана за допомогою розпізнавача кодів валют.
        /// </summary>
        public string CurrencyName => CurrencyCodesResolver.GetCodeByNumber(CurrencyCode);

        /// <summary>
        /// Баланс банки у грошовому форматі.
        /// </summary>
        public double BalanceAsMoney => Balance.AsMoney();

        /// <summary>
        /// Цільова сума для накопичення в банці у грошовому форматі.
        /// </summary>
        public double? GoalAsMoney => Goal?.AsMoney();

        #endregion
    }
}