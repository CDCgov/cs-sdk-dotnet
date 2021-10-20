using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace CS.Mmg.HL7V251
{
    /// <summary>
    /// Gets/sets whether this data element is part of a repeating group. The PRIMARY/PARENT
    /// observation is marked to serve as the anchor for the repeating group, and must be present
    /// for the group to process correctly. The CHILD notation denotes that the variable is a
    /// child observation in the repeating group and must have the same OBX-4 Observation Sub-id
    /// value as the PRIMARY/PARENT observation to be considered in the same group.  YES indicates
    /// that this variable is considered to be part of a repeating group, but the PARENT/CHILD
    /// relationship does not apply to the elements in the repeating group.  NO indicates that this
    /// variable is not considered part of a repeating group and will not be processed as such.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum RepeatingGroupElementType
    {
        /// <summary>
        /// YES
        /// </summary>
        [EnumMember(Value = "YES")]
        Yes,

        /// <summary>
        /// PRIMARY/PARENT
        /// </summary>
        [EnumMember(Value = "PRIMARY/PARENT")]
        PrimaryParent,

        /// <summary>
        /// PARENT
        /// </summary>
        [EnumMember(Value = "PARENT")]
        Parent,

        /// <summary>
        /// CHILD
        /// </summary>
        [EnumMember(Value = "CHILD")]
        Child,

        /// <summary>
        /// NO
        /// </summary>
        [EnumMember(Value = "NO")]
        No,
    }
}