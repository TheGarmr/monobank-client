using System.Text.Json.Serialization;

namespace Monobank.Client.Models
{
    public class WebHookData
    {
        [JsonPropertyName("account")]
        public string Account { get; set; }
        [JsonPropertyName("statementItem")]
        public Statement StatementItem { get; set; }
    }
}