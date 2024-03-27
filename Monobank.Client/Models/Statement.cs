using System;
using System.Text.Json.Serialization;
using ISO._18245;
using ISO._4217;
using Monobank.Client.Extensions;

namespace Monobank.Client.Models
{
    public class Statement
    {
        /// <summary>
        /// Унікальний id транзакції
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>
        /// Час транзакції в секундах в форматі Unix time
        /// </summary>
        [JsonPropertyName("time")]
        public long TimeInSeconds { get; set; }

        /// <summary>
        /// Опис транзакцій
        /// </summary>
        [JsonPropertyName("description")]
        public string Description { get; set; }

        /// <summary>
        /// Код типу транзакції (Merchant Category Code), відповідно ISO 18245
        /// </summary>
        [JsonPropertyName("mcc")]
        public int MerchantCategoryCode { get; set; }

        /// <summary>
        /// Оригінальний код типу транзакції (Merchant Category Code), відповідно ISO 18245
        /// </summary>
        [JsonPropertyName("originalMcc")]
        public int OriginalMerchantCategoryCode { get; set; }

        /// <summary>
        /// Статус блокування суми (детальніше у <see href="https://en.wikipedia.org/wiki/Authorization_hold">wiki</see>)
        /// </summary>
        [JsonPropertyName("hold")]
        public bool IsHold { get; set; }

        /// <summary>
        /// Сума у валюті рахунку в мінімальних одиницях валюти (копійках, центах)
        /// </summary>
        [JsonPropertyName("amount")]
        public long Amount { get; set; }

        /// <summary>
        /// Сума у валюті транзакції в мінімальних одиницях валюти (копійках, центах)
        /// </summary>
        [JsonPropertyName("operationAmount")]
        public long OperationAmount { get; set; }

        /// <summary>
        /// Код валюти рахунку відповідно ISO 4217
        /// </summary>
        [JsonPropertyName("currencyCode")]
        public int CurrencyCode { get; set; }

        /// <summary>
        /// Розмір комісії в мінімальних одиницях валюти (копійках, центах)
        /// </summary>
        [JsonPropertyName("commissionRate")]
        public long ComissionRate { get; set; }

        /// <summary>
        /// Розмір кешбеку в мінімальних одиницях валюти (копійках, центах)
        /// </summary>
        [JsonPropertyName("cashbackAmount")]
        public long CashbackAmount { get; set; }

        /// <summary>
        /// Баланс рахунку в мінімальних одиницях валюти (копійках, центах)
        /// </summary>
        [JsonPropertyName("balance")]
        public long Balance { get; set; }

        /// <summary>
        /// Коментар до переказу, уведений користувачем. Якщо не вказаний, поле буде відсутнім
        /// </summary>
        [JsonPropertyName("comment")]
        public string Comment { get; set; }

        /// <summary>
        /// Номер квитанції для check.gov.ua. Поле може бути відсутнім
        /// </summary>
        [JsonPropertyName("receiptId")]
        public string ReceiptId { get; set; }

        /// <summary>
        /// Номер квитанції ФОПа, приходить у випадку якщо це операція із зарахуванням коштів
        /// </summary>
        [JsonPropertyName("invoiceId")]
        public string InvoiceId { get; set; }

        /// <summary>
        /// ЄДРПОУ контрагента, присутній лише для елементів виписки рахунків ФОП
        /// </summary>
        [JsonPropertyName("counterEdrpou")]
        public string CounterEdrpou { get; set; }

        /// <summary>
        /// IBAN контрагента, присутній лише для елементів виписки рахунків ФОП
        /// </summary>
        [JsonPropertyName("counterIban")]
        public string CounterIban { get; set; }

        /// <summary>
        /// Найменування контрагента
        /// </summary>
        [JsonPropertyName("counterName")]
        public string CounterName { get; set; }

        #region Custom properties

        /// <summary>
        /// Назва валюти
        /// </summary>
        [JsonIgnore]
        public string CurrencyName => CurrencyCodesResolver.GetCodeByNumber(CurrencyCode);

        /// <summary>
        /// Баланс у вигляді грошової суми
        /// </summary>
        [JsonIgnore]
        public double BalanceAsMoney => ParseMoney(Balance);

        /// <summary>
        /// Час транзакції у форматі DateTime, отриманий з секунд Unix time.
        /// </summary>
        [JsonIgnore]
        public DateTime Time => TimeInSeconds.ToDateTime();

        /// <summary>
        /// Категорія торговця
        /// </summary>
        [JsonIgnore]
        public string MerchantCategory => MerchantCategoryCodesResolver.GetByCode(MerchantCategoryCode.ToString("D3")).Description;

        #endregion

        internal static double ParseMoney(long input)
        {
            var value = input.ToString();

            if (string.IsNullOrWhiteSpace(value) || value.Length < 3)
            {
                return 0;
            }

            var balance = value.Insert(value.Length - 2, ".");
            var parsed = double.TryParse(balance, out var parsedValue);
            return parsed ? parsedValue : 0;
        }
    }
}
