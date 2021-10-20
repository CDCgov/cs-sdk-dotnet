using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace CS.Mmg
{
    /// <summary>
    ///  What is the type of the guide either guide or template
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum GuideType
    {
        /// <summary>
        ///  Guide
        /// </summary>
        [EnumMember(Value = "Guide")]
        Guide,

        /// <summary>
        ///  Template
        /// </summary>
        [EnumMember(Value = "Template")]
        Template,
    }
}