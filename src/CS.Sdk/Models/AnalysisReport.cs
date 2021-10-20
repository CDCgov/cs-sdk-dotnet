using System;
using Newtonsoft.Json;

namespace CS.Mmg
{
    /// <summary>
    /// Analysis Report is a copy of a test scenario to display the results of the analysis in onboarding
    /// </summary>
    public sealed class AnalysisReport : TestScenario
    {
        /// <summary>
        /// A letter grade
        /// Grade is calculated from a percentage: ((total - errors.length) / total) /// 100
        /// </summary>
        public string Grade { get; set; }

        /// <summary>
        /// Total number of analyzed data elements
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// Errors as a list
        /// </summary>
        public string[] Errors { get; set; }

        /// <summary>
        /// Handle different parsing based on the data type
        /// </summary>
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        ParsingMapping Parsing { get; set; }
    }

    /// <summary>
    /// Defines the mapping for the parsing
    /// </summary>
    public sealed class ParsingMapping
    {
        /// <summary>
        /// HL7V251 parsing
        /// </summary>
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public HL7V251.Parsing Hl7v251 { get; set; }

        /// <summary>
        /// FHIRV4 parsing
        /// </summary>
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public FHIRV4.Parsing Fhirv4 { get; set; }
    }
}