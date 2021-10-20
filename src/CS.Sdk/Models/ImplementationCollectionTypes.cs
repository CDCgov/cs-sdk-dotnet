using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace CS.Mmg
{
    /// <summary>
    /// Enums for MMG PHA Implementation
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ImplementationCollectionTypes
    {
        /// <summary>
        /// Yes
        /// </summary>
        [EnumMember(Value = "Yes")]
        Yes,

        /// <summary>
        /// Yes - currently collected
        /// </summary>
        [EnumMember(Value = "Yes - currently collected")]
        YesCurrentlyCollected,

        /// <summary>
        /// Yes - adding to system
        /// </summary>
        [EnumMember(Value = "Yes - adding to system")]
        YesAddingToSystem,

        /// <summary>
        /// Only certain conditions
        /// </summary>
        [EnumMember(Value = "Only certain conditions")]
        OnlyCertainConditions,

        /// <summary>
        /// No
        /// </summary>
        [EnumMember(Value = "No")]
        No,
    }
}
