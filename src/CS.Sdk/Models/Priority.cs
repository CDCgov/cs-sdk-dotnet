using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace CS.Mmg
{
    /// <summary>
    /// Represents the CDC Priority of the data element
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Priority
    {
        /// <summary>
        /// Unknown
        /// </summary>
        [EnumMember(Value = "U")]
        Unknown = 'U',

        /// <summary>
        /// Required
        /// </summary>
        [EnumMember(Value = "R")]
        Required = 'R',

        /// <summary>
        /// 1
        /// </summary>
        [EnumMember(Value = "1")]
        One = '1',

        /// <summary>
        /// 2
        /// </summary>
        [EnumMember(Value = "2")]
        Two = '2',

        /// <summary>
        /// 3
        /// </summary>
        [EnumMember(Value = "3")]
        Three = '3',
    }
}
