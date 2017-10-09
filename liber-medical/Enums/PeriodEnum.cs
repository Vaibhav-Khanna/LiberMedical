using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace libermedical.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum PeriodEnum
    {
        [EnumMember(Value = "morning")]
        morning,
        [EnumMember(Value = "lunch")]
        lunch,
        [EnumMember(Value = "afternoon")]
        afternoon,
        [EnumMember(Value = "evening")]
        evening
    }
}
