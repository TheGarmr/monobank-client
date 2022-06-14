using System;
using System.Text.Json.Serialization;
using ISO._4217;
using Monobank.Client.Extensions;

namespace Monobank.Client.Models
{
    public class Statement
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("time")]
        public long TimeInSeconds { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("mcc")]
        public int MerchantCategoryCode { get; set; }

        [JsonPropertyName("hold")]
        public bool IsHold { get; set; }

        [JsonPropertyName("amount")]
        public long Amount { get; set; }

        [JsonPropertyName("operationAmount")]
        public long OperationAmount { get; set; }

        [JsonPropertyName("currencyCode")]
        public int CurrencyCode { get; set; }

        [JsonPropertyName("commissionRate")]
        public long ComissionRate { get; set; }

        [JsonPropertyName("cashbackAmount")]
        public long CashbackAmount { get; set; }

        [JsonPropertyName("balance")]
        public long Balance { get; set; }

        [JsonPropertyName("comment")]
        public string Comment { get; set; }

        [JsonPropertyName("receiptId")]
        public string ReceiptId { get; set; }

        [JsonPropertyName("counterEdrpou")]
        public string CounterEdrpou { get; set; }

        [JsonPropertyName("counterIban")]
        public string CounterIban { get; set; }

        #region Custom properties

        [JsonIgnore]
        public string CurrencyName => CurrencyCodesResolver.GetCodeByNumber(CurrencyCode);

        [JsonIgnore]
        public double BalanceAsMoney => ParseMoney(Balance);

        [JsonIgnore]
        public DateTime Time => TimeInSeconds.ToDateTime();

        #endregion
        public static double ParseMoney(long input)
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
