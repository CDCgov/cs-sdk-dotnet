using Newtonsoft.Json;

namespace CS.Mmg.FHIRV4
{
    /// <summary>
    /// FHIR resource interface
    /// </summary>
    public sealed class Resource
    {
        /// <summary>
        /// Type of resource this refers to, should be unique
        /// </summary>
        public ResourceType Type { get; set; }

        /// <summary>
        /// Maturity level of this resource
        /// </summary>
        public Maturity Maturity { get; set; }

        /// <summary>
        /// What is the development status of this resource
        /// </summary>
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public DevelopmentStatus Status { get; set; }
    }
}