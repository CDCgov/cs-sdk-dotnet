using System;
using Newtonsoft.Json;

namespace CS.Mmg.VADS
{
    /// <summary>
    /// Represents a VADS ValueSetVersion
    /// </summary>
    public sealed class ValueSetVersion
    {
        /// <summary>
        /// Gets or sets the value set version identifier.
        /// </summary>
        /// <value>
        /// The value set version identifier.
        /// </value>
        public Guid ValueSetVersionId { get; set; }

        /// <summary>
        /// Gets or sets the value set version number.
        /// </summary>
        /// <value>
        /// The value set version number.
        /// </value>
        public int ValueSetVersionNumber { get; set; } 

        /// <summary>
        /// Gets or sets the value set version description text.
        /// </summary>
        /// <value>
        /// The value set version description text.
        /// </value>
        public string ValueSetVersionDescriptionText { get; set; } 

        /// <summary>
        /// Gets or sets the status code.
        /// </summary>
        /// <value>
        /// The status code.
        /// </value>
        public string StatusCode { get; set; } 

        /// <summary>
        /// Gets or sets the status date.
        /// </summary>
        /// <value>
        /// The status date.
        /// </value>
        public DateTime StatusDate { get; set; }

        /// <summary>
        /// Gets or sets the assigning authority version text.
        /// </summary>
        /// <value>
        /// The assigning authority version text.
        /// </value>
        public string AssigningAuthorityVersionText { get; set; } 

        /// <summary>
        /// Gets or sets the assigning authority release date.
        /// </summary>
        /// <value>
        /// The assigning authority release date.
        /// </value>
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public DateTime? AssigningAuthorityReleaseDate { get; set; }

        /// <summary>
        /// Gets or sets the note text.
        /// </summary>
        /// <value>
        /// The note text.
        /// </value>
        public string NoteText { get; set; } 

        /// <summary>
        /// Gets or sets the effective date.
        /// </summary>
        /// <value>
        /// The effective date.
        /// </value>
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public DateTime? EffectiveDate { get; set; }

        /// <summary>
        /// Gets or sets the expiry date.
        /// </summary>
        /// <value>
        /// The expiry date.
        /// </value>
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public DateTime? ExpiryDate { get; set; }

        /// <summary>
        /// Gets or sets the value set oid.
        /// </summary>
        /// <value>
        /// The value set oid.
        /// </value>
        public string ValueSetOid { get; set; }  // FK - ref ValueSet.ValueSetOID
    }
}