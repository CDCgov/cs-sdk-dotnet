using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CS.Mmg
{
    /// <summary>
    /// Represents a test scenario
    /// </summary>
    public class TestScenario
    {
        /// <summary>
        /// Gets/sets the internal ID of the block
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets/sets the ordinal value of this test scenario for ordering purposes
        /// </summary>
        public int Ordinal { get; set; }

        /// <summary>
        /// Gets the ID of the MMG
        /// </summary>
        public Guid GuideId { get; set; }

        /// <summary>
        /// Gets the internal version of the MMG
        /// </summary>
        public int GuideInternalVersion { get; set; }

        /// <summary>
        /// Gets/sets the name of this test scenario
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets/sets the description of this test scenario
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets/sets the raw message content either HL7V251 or FHIRV4
        /// </summary>
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public string Message { get; set; }

        /// <summary>
        /// This is a dictionary of the test blocks
        /// </summary>
        public IDictionary<string, TestBlock> TestBlocks { get; set; }
    }
}
