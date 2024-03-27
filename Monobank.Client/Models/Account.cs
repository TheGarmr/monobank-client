using System.Text.Json.Serialization;
using ISO._4217;
using Monobank.Client.Enums;
using Monobank.Client.Extensions;

namespace Monobank.Client.Models
{
    public class Account
    {
        /// <summary>
        /// Ідентифікатор рахунку
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>
        /// Ідентифікатор для сервісу https://send.monobank.ua/{sendId}
        /// </summary>
        [JsonPropertyName("sendId")]
        public string SendId { get; set; }

        /// <summary>
        /// Баланс рахунку в мінімальних одиницях валюти (копійках, центах)
        /// </summary>
        [JsonPropertyName("balance")]
        public long Balance { get; set; }

        /// <summary>
        /// Кредитний ліміт
        /// </summary>
        [JsonPropertyName("creditLimit")]
        public long CreditLimit { get; set; }

        /// <summary>
        /// Тип рахунку <see cref="AccountTypes"/>
        /// </summary>
        [JsonPropertyName("type")]
        public AccountTypes Type { get; set; }

        /// <summary>
        /// Код валюти рахунку відповідно ISO 4217
        /// </summary>
        [JsonPropertyName("currencyCode")]
        public int CurrencyCode { get; set; }

        /// <summary>
        /// Тип кешбеку який нараховується на рахунок <see cref="CashbackType"/>
        /// </summary>
        [JsonPropertyName("cashbackType")]
        public CashbackType CashbackType { get; set; }

        /// <summary>
        /// Перелік замаскованних номерів карт (більше одного може бути у преміальних карт)
        /// </summary>
        [JsonPropertyName("maskedPan")]
        public string[] MaskedPan { get; set; }

        /// <summary>
        /// IBAN рахунку
        /// </summary>
        [JsonPropertyName("iban")]
        public string IBan { get; set; }

        #region Custom properties

        /// <summary>
        /// Назва валюти.
        /// </summary>
        public string CurrencyName => CurrencyCodesResolver.GetCodeByNumber(CurrencyCode);

        /// <summary>
        /// Баланс рахунку без кредитного ліміту у грошовому форматі.
        /// </summary>
        public double BalanceWithoutCreditLimit => (Balance - CreditLimit).AsMoney();

        /// <summary>
        /// Кредитний ліміт у грошовому форматі.
        /// </summary>
        public double CreditLimitAsMoney => CreditLimit.AsMoney();

        #endregion
    }
}
