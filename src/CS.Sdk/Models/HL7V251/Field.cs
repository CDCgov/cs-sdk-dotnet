using Newtonsoft.Json;

namespace CS.Mmg.HL7V251
{
    /// <summary>
    /// Sequence of the field in the segment
    /// </summary>
    public sealed class Field
    {
        /// <summary>
        /// Sequence of the field in the segment
        /// </summary>
        public int Sequence { get; set; }

        /// <summary>
        /// The segment type for the field
        /// </summary>
        public SegmentType Type { get; set; }

        /// <summary>
        /// Readable name of the field
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The data type for the field
        /// </summary>
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public DataType? DataType { get; set; }
    }
}