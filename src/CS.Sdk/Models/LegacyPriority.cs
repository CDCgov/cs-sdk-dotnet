using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace CS.Mmg
{
    /// <summary>
    /// Represents the 'CDC Legacy Priority' of a data element.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum LegacyPriority
    {
        /// <summary>
        /// Optional. This is an optional variable and there is
        /// no requirement to send this information to CDC. However, if this variable is already being
        /// collected by the state/territory, or if the state/territory is planning to collect this
        /// information because it is deemed important for your own programmatic needs, CDC would like this
        /// information sent. CDC preferred variables are the most important of the optional variables to
        /// be earmarked for CDC analysis/assessment, even if sent from a small number of states.
        /// </summary>
        [EnumMember(Value = "O")]
        Optional = 'O',

        /// <summary>
        /// Preferred. This is an optional variable and there is no requirement to send this information
        /// to CDC.  This variable is considered nice-to-know if the state/territory already collects this
        /// information or is planning to collect this information, but has a lower level of importance to
        /// CDC than the preferred classification of optional data elements.
        /// </summary>
        [EnumMember(Value = "P")]
        Preferred = 'P',

        /// <summary>
        /// Required. Means the data element is mandatory for sending the message. If data element is not
        /// present, the message will error out.
        /// </summary>
        [EnumMember(Value = "R")]
        Required = 'R',
    }
}
