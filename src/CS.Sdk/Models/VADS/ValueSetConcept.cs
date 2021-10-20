using System;
using Newtonsoft.Json;

namespace CS.Mmg.VADS
{
    /// <summary>
    /// Represents a VADS ValueSetConcept
    /// </summary>
    public sealed class ValueSetConcept
    {
        /// <summary>
        /// Gets or sets the value set concept identifier.
        /// </summary>
        /// <value>
        /// The value set concept identifier.
        /// </value>
        public Guid ValueSetConceptId { get; set; }

        /// <summary>
        /// Gets or sets the name of the code system concept.
        /// </summary>
        /// <value>
        /// The name of the code system concept.
        /// </value>
        public string CodeSystemConceptName { get; set; }

        /// <summary>
        /// Gets or sets the value set concept status code.
        /// </summary>
        /// <value>
        /// The value set concept status code.
        /// </value>
        public string ValueSetConceptStatusCode { get; set; }  // FK3 - VocabularyObjectStatusLookup.StatusCode

        /// <summary>
        /// Gets or sets the value set concept status date.
        /// </summary>
        /// <value>
        /// The value set concept status date.
        /// </value>
        public DateTime ValueSetConceptStatusDate { get; set; }

        /// <summary>
        /// Gets or sets the value set concept definition text.
        /// </summary>
        /// <value>
        /// The value set concept definition text.
        /// </value>
        public string ValueSetConceptDefinitionText { get; set; }

        /// <summary>
        /// Gets or sets the CDC preferred designation.
        /// </summary>
        /// <value>
        /// The CDC preferred designation.
        /// </value>
        public string CdcPreferredDesignation { get; set; }

        /// <summary>
        /// Gets or sets the scope note text.
        /// </summary>
        /// <value>
        /// The scope note text.
        /// </value>
        public string ScopeNoteText { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the value set version identifier.
        /// </summary>
        /// <value>
        /// The value set version identifier.
        /// </value>
        public Guid ValueSetVersionId { get; set; } // FK2 - ValueSetVersion.ValueSetVersionID

        /// <summary>
        /// Gets or sets the code system oid.
        /// </summary>
        /// <value>
        /// The code system oid.
        /// </value>
        public string CodeSystemOid { get; set; } // FK1 - CodeSystemConcept.CodeSystemOID

        /// <summary>
        /// Gets or sets the concept code.
        /// </summary>
        /// <value>
        /// The concept code.
        /// </value>
        public string ConceptCode { get; set; } // FK1 - CodeSystemConcept.ConceptCode

        /// <summary>
        /// Gets or sets the sequence.
        /// </summary>
        /// <value>
        /// The sequence.
        /// </value>
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public int? Sequence { get; set; }

        /// <summary>
        /// Gets or sets the h L70396 identifier.
        /// </summary>
        /// <value>
        /// The h L70396 identifier.
        /// </value>
        public string HL70396Identifier { get; set; }
    }
}