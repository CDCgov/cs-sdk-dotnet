using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace CS.Mmg
{
    /// <summary>
    /// Used for the CodeSystem value
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum LegacyCodeSystem
    {
        /// <summary>
        /// LN
        /// </summary>
        [EnumMember(Value = "LN")]
        LN,

        /// <summary>
        /// N/A
        /// </summary>
        [EnumMember(Value = "N/A")]
        NA,

        /// <summary>
        /// PHINQUESTION
        /// </summary>
        [EnumMember(Value = "PHINQUESTION")]
        PHINQUESTION,

        /// <summary>
        /// SCT
        /// </summary>
        [EnumMember(Value = "SCT")]
        SCT,
    }
}