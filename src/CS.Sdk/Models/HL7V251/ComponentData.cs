using Newtonsoft.Json;

namespace CS.Mmg.HL7V251
{
    /// <summary>
    /// Data for a parsed component
    /// </summary>
    public sealed class ComponentData
    {
        /// <summary>
        /// The raw value for the component
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// The sequence for component position
        /// </summary>
        public int Sequence { get; set; }

        /// <summary>
        /// The data type if we have it
        /// </summary>
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public DataType DataType { get; set; }
    }
}