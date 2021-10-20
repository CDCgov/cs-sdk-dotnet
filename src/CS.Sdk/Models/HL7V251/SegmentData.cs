using System.Collections.Generic;
using Newtonsoft.Json;

namespace CS.Mmg.HL7V251
{
    /// <summary>
    /// Data for a parsed segment
    /// </summary>
    public sealed class SegmentData
    {
        /// <summary>
        /// The raw value for the segment
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// The segment type
        /// </summary>
        public SegmentType SegmentType { get; set; }

        /// <summary>
        /// The sequence of this segment
        /// </summary>
        public int Sequence { get; set; }

        /// <summary>
        /// OBR Position
        /// This should only exist for OBX
        /// </summary>
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public int? ObrPosition { get; set; }

        /// <summary>
        /// The data type if we have it
        /// </summary>
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public DataType? DataType { get; set; }

        /// <summary>
        /// The fields for this segment
        /// </summary>
        public List<FieldData> Fields { get; set; }
    }
}