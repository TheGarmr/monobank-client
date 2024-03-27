using System.Text.Json.Serialization;

namespace Monobank.Client.Models
{
    public class MonobankApiError
    {
        /// <summary>
        /// Опис помилки.
        /// </summary>
        [JsonPropertyName("errorDescription")]
        public string Description { get; set; }
    }
}
