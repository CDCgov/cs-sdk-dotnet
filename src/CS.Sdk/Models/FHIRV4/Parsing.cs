using Newtonsoft.Json;

namespace CS.Mmg.FHIRV4
{
    /// <summary>
    /// FHIR Parsing
    /// </summary>
    public sealed class Parsing
    {
        /// <summary>
        /// the parsed resources
        /// </summary>
        public string Data { get; set; }

        /// <summary>
        /// The unmatched segments for this analyzed test scenario
        /// </summary>
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public string[] UnmatchedResources { get; set; }
    }
}