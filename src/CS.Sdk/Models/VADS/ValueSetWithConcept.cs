using System;
using System.Collections.Generic;

namespace CS.Mmg.VADS
{
    /// <summary>
    /// Represents a VADS ValueSetWithConcept
    /// </summary>
    public sealed class ValueSetWithConcept 
    {
        /// <summary>
        /// Gets or sets the value set.
        /// </summary>
        /// <value>
        /// The value set.
        /// </value>
        public ValueSet ValueSet { get; set; }

        /// <summary>
        /// Gets or sets the value set version.
        /// </summary>
        /// <value>
        /// The value set version.
        /// </value>
        public ValueSetVersion ValueSetVersion { get; set; }

        /// <summary>
        /// Gets or sets the concepts count.
        /// </summary>
        /// <value>
        /// The concepts count.
        /// </value>
        public int ConceptsCount { get; set; }
        /// <summary>
        /// Gets or sets the concepts.
        /// </summary>
        /// <value>
        /// The concepts.
        /// </value>
        public IEnumerable<ValueSetConcept> Concepts { get; set; }
    }
}