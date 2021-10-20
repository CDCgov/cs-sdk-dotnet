using System.Collections.Generic;
using Newtonsoft.Json;

namespace CS.Mmg.HL7V251
{
    /// <summary>
    /// HL7 Parsing
    /// </summary>
    public sealed class Parsing
    {
        /// <summary>
        /// the parsed segment data
        /// </summary>
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public List<SegmentData> Data { get; set; }

        /// <summary>
        /// The segments that were parsed in the HL7
        /// </summary>
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public string[] Segments { get; set; }

        /// <summary>
        /// The unmatched segments for this analyzed test scenario
        /// </summary>
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public string[] UnmatchedSegments { get; set; }
    }
}