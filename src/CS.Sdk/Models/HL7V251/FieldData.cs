using System.Collections.Generic;
using Newtonsoft.Json;

namespace CS.Mmg.HL7V251
{
    /// <summary>
    /// Data for a parsed field
    /// </summary>
    public sealed class FieldData
    {
        /// <summary>
        /// The raw value for the field
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Repetitions, split on ~ if we have it
        /// </summary>
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public string[] Repetitions { get; set; }

        /// <summary>
        /// The sequence for field position
        /// </summary>
        public int Sequence { get; set; }

        /// <summary>
        /// The data type if we have it
        /// </summary>
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public DataType DataType { get; set; }

        /// <summary>
        /// The components found in the field
        /// </summary>
        public List<ComponentData> Components { get; set; }

    }
}