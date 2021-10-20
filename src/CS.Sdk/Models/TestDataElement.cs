using System.Collections.Generic;
using Newtonsoft.Json;

namespace CS.Mmg
{
    /// <summary>
    /// Represents a test scenario data element value
    /// </summary>
    public sealed class TestDataElement
    {
        /// <summary>
        /// Values for the element in this test scenario, repetitions go in this array
        /// </summary>
        public List<DataElementValue> Values { get; set; }

        /// <summary>
        /// Expected values for the PHA for this test scenario
        /// </summary>
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public List<DataElementValue> PublicHealthAgencyValues { get; set; }

        /// <summary>
        /// Values from parsing the HL7
        /// </summary>
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public List<DataElementValue> ResultValues { get; set; }

        /// <summary>
        /// The similarity percentage of the expected vs the result
        /// </summary>
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public int? Similarity { get; set; }

        /// <summary>
        /// The HL7 segment that was used to provide the result
        /// </summary>
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public string Segment { get; set; }
    }
}