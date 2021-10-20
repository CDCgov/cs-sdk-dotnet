using System.Collections.Generic;
using Newtonsoft.Json;

namespace CS.Mmg.HL7V251
{
    /// <summary>
    /// MMG Data Element Mapping for HL7 2.5.1 Interface
    /// </summary>
    public sealed class DataElementMapping
    {
        /// <summary>
        /// Gets/sets the PHIN data element identifier. Examples: DEM197, NOT115
        /// </summary>
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public string LegacyIdentifier { get; set; }

        /// <summary>
        /// Gets/sets the Data Element Identifier to be sent in HL7 message. Examples: null,
        /// "PID-11.4", "11368-8", "OBX-6"
        /// </summary>
        public string Identifier { get; set; }

        /// <summary>
        /// HL7 Message Context field:
        /// Specific HL7 segment and field mapping for the element
        /// </summary>
        public string MessageContext { get; set; }

        /// <summary>
        /// HL7 Data Type field:
        /// HL7 data type used by PHIN to express the variable.
        /// Examples of data types expected are CWE, SN, TS, DT, ST, TX, XPN, XON, or XAD,
        /// depending on the type of data being passed. The specific HL7 datatype allowed
        /// in the field is consistent with the HL7 2.5.1 Standard for this message.
        /// </summary>
        public DataType DataType { get; set; }

        /// <summary>
        /// Segment Type field
        /// </summary>
        public SegmentType SegmentType { get; set; }

        /// <summary>
        /// OBR Position field
        /// This should only exist for OBX
        /// </summary>
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public int? ObrPosition { get; set; }

        /// <summary>
        /// Field Position field
        /// </summary>
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public int? FieldPosition { get; set; }

        /// <summary>
        /// Component Position field
        /// </summary>
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public int? ComponentPosition { get; set; }

        /// <summary>
        /// Usage field
        /// </summary>
        public Usage Usage { get; set; }

        /// <summary>
        /// Cardinality field:
        /// Cardinality identifies the minimum and maximum number of repetitions for a particular field.
        /// When a field repeats, the values are sent in the same field with the instances separated by the tilde (~).
        /// Examples:  [1..1] means that the field is required and will not repeat.
        /// [0..1] means that the field is optional and will not repeat if it is present.
        /// [0..*] means the field is optional and may repeat an unlimited number of times.
        /// Cardinality is a the field level and does not indicate whether the data element is part of a repeating group.
        /// </summary>
        public string Cardinality { get; set; }

        /// <summary>
        /// Gets/sets the key-value pair of literal values. The 'key' is an integer and refers to the
        /// field position in the segment to which the literal belongs. The value is a string and is
        /// simply the literal value. This is used, for example, in Varicella (see data element 67453-1)
        /// where there's a conformance statement like this: "CONFORMANCE STATEMENT: OBX-6 SHALL
        /// contain the literal value 'd^day^UCUM'"
        /// </summary>
        public IDictionary<int, string> LiteralFieldValues { get; set; }

        /// <summary>
        /// Repeating Group Element field:
        /// This column describes whether the data element is part of a repeating group.
        /// The PRIMARY/PARENT observation is marked to serve as the anchor for the repeating group, and must be present for the group to process correctly.
        /// The CHILD notation denotes that the variable is a child observation in the repeating group and must have the same OBX-4 Observation Sub-id value
        /// as the PRIMARY/PARENT observation to be considered in the same group. YES indicates that this variable is considered to be part of a repeating group, 
        /// but the PARENT/CHILD relationship does not apply to the elements in the repeating group.
        /// NO indicates that this variable is not considered part of a repeating group and will not be processed as such.
        /// </summary>
        public RepeatingGroupElementType RepeatingGroupElementType { get; set; }

        /// <summary>
        /// HL7 Implementation Notes field:
        /// Related implementation comments
        /// </summary>
        public string ImplementationNotes { get; set; }

        /// <summary>
        /// Sample Segment field;
        /// Sample segment to provide guidance on how this data element is conveyed in a message.
        /// </summary>
        public string SampleSegment { get; set; }
    }
}