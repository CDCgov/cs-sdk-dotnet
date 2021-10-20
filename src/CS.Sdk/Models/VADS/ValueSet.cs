using System;
using Newtonsoft.Json;

namespace CS.Mmg.VADS
{
    /// <summary>
    /// Represents a VADS ValueSet
    /// </summary>
    public sealed class ValueSet
    {
       /// <summary>
        /// Gets or sets the value set identifier.
        /// </summary>
        /// <value>
        /// The value set identifier.
        /// </value>
        public Guid ValueSetId { get; set; }

        /// <summary>
        /// Gets or sets the value set oid.
        /// </summary>
        /// <value>
        /// The value set oid.
        /// </value>
        public string ValueSetOid { get; set; }

        /// <summary>
        /// Gets or sets the name of the value set.
        /// </summary>
        /// <value>
        /// The name of the value set.
        /// </value>
        public string ValueSetName { get; set; }

        /// <summary>
        /// Gets or sets the value set code.
        /// </summary>
        /// <value>
        /// The value set code.
        /// </value>
        public string ValueSetCode { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the status date.
        /// </summary>
        /// <value>
        /// The status date.
        /// </value>
        public DateTime StatusDate { get; set; }

        /// <summary>
        /// Gets or sets the definition text.
        /// </summary>
        /// <value>
        /// The definition text.
        /// </value>
        public string DefinitionText { get; set; }

        /// <summary>
        /// Gets or sets the scope note text.
        /// </summary>
        /// <value>
        /// The scope note text.
        /// </value>
        public string ScopeNoteText { get; set; }

        /// <summary>
        /// Gets or sets the assigning authority identifier.
        /// </summary>
        /// <value>
        /// The assigning authority identifier.
        /// </value>
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public Guid? AssigningAuthorityId { get; set; }

        /// <summary>
        /// Gets or sets the legacy flag.
        /// </summary>
        /// <value>
        /// The legacy flag.
        /// </value>
        public string LegacyFlag { get; set; }
    }
}