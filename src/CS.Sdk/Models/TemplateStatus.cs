using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace CS.Mmg
{
    /// <summary>
    /// Represents type of template that this message mapping guide is
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum TemplateStatus
    {
        /// <summary>
        /// Locked. Indicates that this template, when used in a message mapping guide, is not to be
        /// changed from its current state in that guide.
        /// </summary>
        [EnumMember(Value = "L")]
        Locked = 'L',

        /// <summary>
        /// Unlocked. Indicates that this template, when used in a message mapping guide, may be changed
        /// from its current state in that guide.
        /// </summary>
        [EnumMember(Value = "U")]
        Unlocked = 'U',

        /// <summary>
        /// Disallowed. Indicates that this is either not a template or that the template may not be
        /// used in message mapping guides.
        /// </summary>
        [EnumMember(Value = "D")]
        Disallowed = 'D',
    }
}