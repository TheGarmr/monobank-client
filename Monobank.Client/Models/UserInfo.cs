﻿using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Monobank.Client.Models
{
    public class UserInfo
    {
        [JsonPropertyName("clientId")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("webHookUrl")]
        public string WebHookUrl { get; set; }

        [JsonPropertyName("accounts")]
        public ICollection<Account> Accounts { get; set; }

        [JsonPropertyName("jars")]
        public ICollection<Jar> Jars { get; set; }
    }
}
