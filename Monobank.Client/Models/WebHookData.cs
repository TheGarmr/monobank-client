using System.Text.Json.Serialization;

namespace Monobank.Client.Models
{
    public class WebHookData
    {
        /// <summary>
        /// Ідентифікатор рахунку.
        /// </summary>
        [JsonPropertyName("account")]
        public string Account { get; set; }

        /// <summary>
        /// Виписка з рахунку <see cref="Statement"/>
        /// </summary>
        [JsonPropertyName("statementItem")]
        public Statement StatementItem { get; set; }
    }
}