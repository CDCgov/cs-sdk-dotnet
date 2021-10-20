using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CS.Mmg
{
    /// <summary>
    /// Represents a named block of data elements within a message mapping guide
    /// </summary>
    public sealed class Block
    {
        /// <summary>
        /// Gets/sets the internal ID of the block
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets/sets the Id of the message mapping guide this block belongs to, if any.
        /// </summary>
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public Guid? GuideId { get; set; }

        /// <summary>
        /// Condition field:
        /// Indicates the condition for which the data element is included (generally this equates to a specific tab within the MMG).
        /// </summary>
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public string Condition { get; set; }

        /// <summary>
        /// Gets/sets the order this block appears in the message mapping guide
        /// </summary>
        public int Ordinal { get; set; }

        /// <summary>
        /// Template information if this block was pulled from a template
        /// </summary>
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public TemplateInfo Template { get; set; }

        /// <summary>
        /// Gets/sets the type of block.
        /// </summary>
        public BlockType Type { get; set; }

        /// <summary>
        /// Gets/sets the name of the block. Example: "Demographics"
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets/sets the block's description which will appear at the top of the block
        /// when the block is displayed to end users
        /// </summary>
        public string StartingDescription { get; set; }

        /// <summary>
        /// Gets/sets the block's description which will appear at the bottom of the block
        /// when the block is displayed to end users
        /// </summary>
        public string EndingDescription { get; set; }

        /// <summary>
        /// Gets/sets whether the block's name should be displayed. Context-dependent.
        /// </summary>
        public bool ShouldDisplayName { get; set; }

        /// <summary>
        /// Gets/sets the list of data elements that are associated with this block
        /// </summary>
        public List<DataElement> Elements { get; set; }

        /// <summary>
        /// Value for if the block is to render as expanded or not
        /// Defaults to true if not set, used on the frontend
        /// </summary>
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public bool? Expanded { get; set; }

        /// <summary>
        /// Boolean value for if the row is part of the optional lab template
        /// </summary>
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public bool? IsLabTemplate { get; set; }

        /// <summary>
        /// Set a value to have the maximum repeating instances for this if a repeating
        /// block across all test scenarios
        /// </summary>
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public int? MaxInstances { get; set; }
    }
}