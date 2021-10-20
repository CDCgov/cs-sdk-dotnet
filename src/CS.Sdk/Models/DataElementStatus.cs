using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace CS.Mmg
{
    /// <summary>
    /// Represents the status of a data element
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum DataElementStatus
    {
        /// <summary>
        /// For internal use only at this time
        /// </summary>
        [EnumMember(Value = "Development")]
        Development,

        /// <summary>
        /// OMB proposed
        /// </summary>
        [EnumMember(Value = "Proposed")]
        Proposed,

        /// <summary>
        /// Final
        /// </summary>
        [EnumMember(Value = "Final")]
        Final,
    }
}