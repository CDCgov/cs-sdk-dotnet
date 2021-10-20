using Newtonsoft.Json;

namespace CS.Mmg.HL7V251
{
    /// <summary>
    /// Component
    /// </summary>
    public sealed class Component
    {
        /// <summary>
        /// Sequence of the component in the field
        /// </summary>
        public int Sequence { get; set; }

        /// <summary>
        /// Readable name of the component
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The data type for the component
        /// </summary>
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public DataType DataType { get; set; }
    }
}