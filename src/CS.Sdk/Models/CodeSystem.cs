using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace CS.Mmg
{
    /// <summary>
    /// Used for the CodeSystem value
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum CodeSystem
    {
        /// <summary>
        /// CDCPHINVS
        /// </summary>
        [EnumMember(Value = "CDCPHINVS")]
        CDCPHINVS,

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
        /// PLT
        /// </summary>
        [EnumMember(Value = "PLT")]
        PLT,

        /// <summary>
        /// SCT
        /// </summary>
        [EnumMember(Value = "SCT")]
        SCT,

        /// <summary>
        /// TBD
        /// </summary>
        [EnumMember(Value = "TBD")]
        TBD,
    }
}