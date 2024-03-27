using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Monobank.Client.Models
{
    public class ClientInfo
    {
        /// <summary>
        /// Ідентифікатор клієнта (збігається з id для send.monobank.ua)
        /// </summary>
        [JsonPropertyName("clientId")]
        public string Id { get; set; }

        /// <summary>
        /// Ім'я клієнта
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// URL для надсиляння подій по зміні балансу рахунку
        /// </summary>
        [JsonPropertyName("webHookUrl")]
        public string WebHookUrl { get; set; }

        /// <summary>
        /// Перелік прав, які які надає сервіс (1 літера на 1 permission).
        /// </summary>
        [JsonPropertyName("permissions")]
        public string Permissions { get; set; }

        /// <summary>
        /// Перелік доступних рахунків <see cref="Account"/>
        /// </summary>
        [JsonPropertyName("accounts")]
        public ICollection<Account> Accounts { get; set; }

        /// <summary>
        /// Перелік банок <see cref="Jar"/>
        /// </summary>
        [JsonPropertyName("jars")]
        public ICollection<Jar> Jars { get; set; }
    }
}
