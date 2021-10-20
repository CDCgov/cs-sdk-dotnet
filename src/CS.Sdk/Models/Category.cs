using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace CS.Mmg
{
    /// <summary>
    /// Broad categories to which data elements can be grouped into
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Category
    {
        /// <summary>
        /// Demographics
        /// </summary>
        [EnumMember(Value = "Demographics")]
        Demographics,

        /// <summary>
        /// Clinical, e.g. signs and symptoms, complications, past medical history, vital signs
        /// </summary>
        [EnumMember(Value = "Clinical")]
        Clinical,

        /// <summary>
        /// Laboratory
        /// </summary>
        [EnumMember(Value = "Laboratory")]
        Laboratory,

        /// <summary>
        ///  Vaccine
        /// </summary>
        [EnumMember(Value = "Vaccine")]
        Vaccine,

        /// <summary>
        /// Treatment
        /// </summary>
        [EnumMember(Value = "Treatment")]
        Treatment,

        /// <summary>
        /// Epidemiology
        /// </summary>
        [EnumMember(Value = "Epidemiology")]
        Epidemiology
    }
}
