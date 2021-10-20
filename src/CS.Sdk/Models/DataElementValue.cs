using Newtonsoft.Json;

namespace CS.Mmg
{
    /// <summary>
    /// Represents a value type for an element
    /// </summary>
    public sealed class DataElementValue
    {
        /// <summary>
        /// This is the value or if a Coded element this is the code and should be stored
        /// label and preferredLabel are used only in the frontend and should not get saved in storage
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// This is the label in code=concept
        /// </summary>
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public string Label { get; set; }

        /// <summary>
        /// This a human readable version, such as {value}={label}
        /// </summary>
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public string PreferredLabel { get; set; }

        /// <summary>
        /// otherValue should exist in the following condition:
        /// if (value == "OTH" || value == "LAB608" || value == "PHC1306")
        /// </summary>
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public string OtherValue { get; set; }

        /// <summary>
        /// If in a repeating group this is the value to determine which instance this element belongs to
        /// </summary>
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public int? SubId { get; set; }
    }
}