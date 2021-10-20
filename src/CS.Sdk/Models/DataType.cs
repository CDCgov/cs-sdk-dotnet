using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace CS.Mmg
{
    /// <summary>
    /// Represents the data type of a data element
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum DataType
    {
        /// <summary>
        /// Coded data type
        /// </summary>
        [EnumMember(Value = "Coded")]
        Coded,

        /// <summary>
        /// CodedNoText data type
        /// </summary>
        [EnumMember(Value = "CodedNoText")]
        CodedNoText,

        /// <summary>
        /// Date data type
        /// </summary>
        [EnumMember(Value = "Date")]
        Date,

        /// <summary>
        /// Date/time data type
        /// </summary>
        [EnumMember(Value = "DateTime")]
        DateTime,

        /// <summary>
        /// Numeric data
        /// </summary>
        [EnumMember(Value = "Numeric")]
        Numeric,

        /// <summary>
        /// Textual data
        /// </summary>
        [EnumMember(Value = "Text")]
        Text,

        /// <summary>
        /// No specified data type
        /// </summary>
        [EnumMember(Value = "None")]
        None,

        /// <summary>
        /// Textual data
        /// </summary>
        [EnumMember(Value = "LongText")]
        LongText,

        /// <summary>
        /// Textual data
        /// </summary>
        [EnumMember(Value = "FormattedText")]
        FormattedText,

        /// <summary>
        /// Textual data - custom value for CX types that comes from older templates like VPD
        /// </summary>
        [EnumMember(Value = "VPDText")]
        VPDText,

        /// <summary>
        /// Integer
        /// </summary>
        [EnumMember(Value = "Integer")]
        Integer,

        /// <summary>
        /// Image/document
        /// </summary>
        [EnumMember(Value = "ImageOrDocumentAttachment")]
        ImageOrDocumentAttachment,
    }
}