using Newtonsoft.Json;

namespace CS.Mmg
{
    /// <summary>
    /// MMG Data Column Interface
    /// Based on Header as used in SuperTable on the FDNS UI React Library
    /// </summary>
    public sealed class DataColumn
    {
        /// <summary>
        /// Column Label
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// JSON Path used to map the data from the Data Element
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Width for the column
        /// </summary>
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public int? Width { get; set; }

        /// <summary>
        /// Icon for the column
        /// </summary>
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public string[] Icon { get; set; }

        /// <summary>
        /// Color of the Icon in the column
        /// </summary>
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public string[] IconColor { get; set; }

        /// <summary>
        /// Is the Column Visible
        /// </summary>
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public bool? IsVisible { get; set; }
    }
}