using System.Text.Json.Serialization;

namespace Monobank.Client.Models
{
    public class WebHook
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }
        [JsonPropertyName("data")]
        public WebHookData Data { get; set; }
    }
}
