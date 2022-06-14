using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Monobank.Client.Enums
{
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum CashbackType
    {
        [EnumMember(Value = "")]
        None = 0,

        [EnumMember(Value = "UAH")]
        UAH = 1,

        [EnumMember(Value = "Miles")]
        Miles = 2
    }
}
