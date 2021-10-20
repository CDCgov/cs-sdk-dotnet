using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace CS.Mmg
{
    /// <summary>
    /// Represents the type of a block. Blocks are loosely defined as
    /// logically-grouped collections of data elements within an MMG
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum BlockType
    {
        /// <summary>
        /// This block can only have one set of values
        /// </summary>
        [EnumMember(Value = "Single")]
        Single,

        /// <summary>
        /// This block may have more than one set of values
        /// </summary>
        [EnumMember(Value = "Repeat")]
        Repeat,

        /// <summary>
        /// This block may have more than one set of values and there is a special
        /// parent/child relationship between some of the elements in this block
        /// </summary>
        [EnumMember(Value = "RepeatParentChild")]
        RepeatParentChild,

        /// <summary>
        /// This is an informational block which will not have elements
        /// </summary>
        [EnumMember(Value = "Info")]
        Info,
    }
}