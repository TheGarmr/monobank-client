using System.Text.Json.Serialization;

namespace Monobank.Client.Models
{
    public class MonobankApiError
    {
        [JsonPropertyName("errorDescription")]
        public string Description { get; set; }
    }
}
