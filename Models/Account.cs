using System.Text.Json.Serialization;
using ISO._4217;
using Monobank.Client.Enums;

namespace Monobank.Client.Models
{
    public class Account
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("balance")]
        public long Balance { get; set; }

        [JsonPropertyName("creditLimit")]
        public long CreditLimit { get; set; }

        [JsonPropertyName("currencyCode")]
        public int CurrencyCode { get; set; }

        [JsonPropertyName("cashbackType")]
        public CashbackType CashbackType { get; set; }

        [JsonPropertyName("type")]
        public AccountTypes Type { get; set; }

        #region Custom properties

        public string CurrencyName => CurrencyCodesResolver.GetCodeByNumber(CurrencyCode);

        public double BalanceWithoutCreditLimit => ParseMoney(Balance - CreditLimit);

        public double CreditLimitAsMoney => ParseMoney(CreditLimit);
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
