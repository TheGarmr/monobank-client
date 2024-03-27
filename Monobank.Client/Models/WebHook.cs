using System.Text.Json.Serialization;

namespace Monobank.Client.Models
{
    public class WebHook
    {
        /// <summary>
        /// Тип веб хуку.
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; }

        /// <summary>
        /// Дані веб-хуку <see cref="WebHookData"/>
        /// </summary>
        [JsonPropertyName("data")]
        public WebHookData Data { get; set; }
    }
}
