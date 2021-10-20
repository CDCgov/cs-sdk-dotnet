using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace CS.Mmg.HL7V251
{
    /// <summary>
    /// Usage field:
    /// Indicates if the field is required, optional, or conditional in a segment. The only values that appear in the Message Mapping are:
    /// R – Required.  Must always be populated.
    /// RE - Required but may be Empty.  This variable indicates that the message receiver must be prepared to process the variable, but it may be absent from a particular message instance.
    /// O – Optional.  May optionally be populated.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Usage
    {
        /// <summary>
        /// Required
        /// </summary>
        [EnumMember(Value = "R")]
        Required,

        /// <summary>
        /// Required Empty
        /// </summary>
        [EnumMember(Value = "RE")]
        RequiredEmpty,

        /// <summary>
        /// Optional
        /// </summary>
        [EnumMember(Value = "O")]
        Optional,

        /// <summary>
        /// Empty
        /// </summary>
        [EnumMember(Value = "X")]
        Empty,
    }
}