using System;
using Newtonsoft.Json;

namespace CS.Mmg
{
    /// <summary>
    /// Information used when referencing templates
    /// </summary>
    public sealed class TemplateInfo
    {
        /// <summary>
        /// Reference to the template ID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The internal version of the template to help determine if outdated
        /// </summary>
        public int InternalVersion { get; set; }

        /// <summary>
        /// Optional value to display the template name
        /// </summary>
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public string Name { get; set; }

        /// <summary>
        /// Optional value to show the template status
        /// </summary>
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public TemplateStatus? Status { get; set; }

        /// <summary>
        /// If in a block which block was pulled from the template
        /// </summary>
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public Guid? BlockId { get; set; }

        /// <summary>
        /// If in an element which element was pulled from the template
        /// </summary>
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public Guid? ElementId { get; set; }
    }

}