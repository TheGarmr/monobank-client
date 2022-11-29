using System.Text.Json.Serialization;
using ISO._4217;
using Monobank.Client.Enums;
using Monobank.Client.Extensions;

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

        public double BalanceWithoutCreditLimit => (Balance - CreditLimit).AsMoney();

        public double CreditLimitAsMoney => CreditLimit.AsMoney();
        #endregion
    }
}
