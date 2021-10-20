using System;
using System.Diagnostics;
using System.Linq;
using Newtonsoft.Json;

namespace CS.Mmg
{
    /// <summary>
    /// Represents a data element
    /// </summary>
    [DebuggerDisplay("DataElementElement, {DataType}, '{Name}'")]
    public class DataElement
    {
        /// <summary>
        /// Gets/sets the internal ID of the element
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Condition field:
        /// Indicates the condition for which the data element is included (generally this equates to a specific tab within the MMG).
        /// </summary>
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public string Condition { get; set; }

        /// <summary>
        /// Gets/sets the Id of the message mapping guide this data element belongs to, if any.
        /// </summary>
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public Guid? GuideId { get; set; }

        /// <summary>
        /// Gets/sets the internal version of the message mapping guide this data element belongs to, if any.
        /// </summary>
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public int? GuideInternalVersion { get; set; }

        /// <summary>
        /// Gets/sets the Id of the block if any.
        /// </summary>
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public Guid? BlockId { get; set; }

        /// <summary>
        /// Gets/sets the row value for export
        /// </summary>
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public int? Row { get; set; }

        /// <summary>
        /// Gets/sets the ordinal value of this element for ordering purposes
        /// </summary>
        public int Ordinal { get; set; }

        /// <summary>
        /// Template information if this element was pulled from a template
        /// </summary>
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public TemplateInfo Template { get; set; }

        /// <summary>
        /// Gets/sets the short name for the data element, which is passed in the message.
        /// Examples: "Local Subject ID", "Birth Date", "Country of Exposure"
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets/sets the description of the data element.  It may not match exactly with the
        /// description in PHIN VADS, because there may be local variations of the description
        /// that do not change the basic concept being mapped to the PHIN Variable.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets/sets the data element's category
        /// </summary>
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public Category? Category { get; set; }

        /// <summary>
        /// Gets/sets the comments for each data element
        /// </summary>
        public string Comments { get; set; }

        /// <summary>
        /// Gets/sets the storage key that points to the specific version of this data element that was used
        /// </summary>
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public Guid? DataElementStorageId { get; set; }

        /// <summary>
        /// Gets/sets the status of this data element
        /// </summary>
        public DataElementStatus Status { get; set; }

        /// <summary>
        /// Gets/sets the data type for the variable response expected by the program area. Data
        /// types are Coded, Numeric, Date or Date/time, and Text.
        /// </summary>
        public DataType DataType { get; set; }

        /// <summary>
        /// Gets/sets the business rules associated with this data element
        /// </summary>
        public string BusinessRules { get; set; }

        /// <summary>
        /// Gets/sets whether this data element represents a unit of measure, such as
        /// age in days, weight in kilograms, height in inches, etc.
        /// </summary>
        public bool IsUnitOfMeasure { get; set; }

        /// <summary>
        /// Gets/sets the Id of another data element that this data element is related to.
        /// The intended use of this is for situations where two data elements might end
        /// up in the same part of an HL7 2.5.1 message (or other messaging format if
        /// HL7 2.5.1 is not used any longer). An example is one data element that fills
        /// in OBX-5, while a separate data element fills in OBX-6 for the same segment.
        /// The software has to know that these two data elements are related in order for
        /// test message generation and sample segment generation to correctly place the
        /// together.
        /// </summary>
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public Guid? RelatedElementId { get; set; }

        /// <summary>
        /// Gets/sets the code system from which the PHIN data element identifier is drawn.
        /// Example values: PHINQUESTION, LOINC, CDCPHINVS, SNOMED-CT
        /// </summary>
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public LegacyCodeSystem? LegacyCodeSystem { get; set; }

        /// <summary>
        /// Gets/sets the code system from which the data element identifier is drawn (e.g.
        /// PHINQUESTION, LOINC, CDCPHINVS, SNOMED-CT).  This reference is sent in the message
        /// for those observations that map as CE (Coded Element) or CWE (Coded With Exceptions)
        /// datatypes in the message. Example values: LN, N/A, PHINQUESTION
        /// </summary>
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public CodeSystem? CodeSystem { get; set; }

        /// <summary>
        /// Gets/sets the CDC priority for the data element. Indicates whether the CDC program specifies
        /// the field as: R: Required - Mandatory for sending the message. If data element is not
        /// present, the message will error out. P: Preferred - This is an optional variable and there is
        /// no requirement to send this information to CDC. However, if this variable is already being
        /// collected by the state/territory, or if the state/territory is planning to collect this
        /// information because it is deemed important for your own programmatic needs, CDC would like this
        /// information sent. CDC preferred variables are the most important of the optional variables to
        /// be earmarked for CDC analysis/assessment, even if sent from a small number of states.
        /// O: Optional - This is an optional variable and there is no requirement to send this information
        /// to CDC.  This variable is considered nice-to-know if the state/territory already collects this
        /// information or is planning to collect this information, but has a lower level of importance to
        /// CDC than the preferred classification of optional data elements.
        /// </summary>
        public LegacyPriority LegacyPriority { get; set; }

        /// <summary>
        /// CDC Priority with the new priority system
        /// </summary>
        public Priority Priority { get; set; }

        /// <summary>
        /// Checkbox for allowing repetitions
        /// </summary>
        public bool IsRepeat { get; set; }

        /// <summary>
        /// The number of times a data element repats if Y/{repetitions}
        /// </summary>
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public int? Repetitions { get; set; }

        /// <summary>
        /// Display only field for the mayRepeat field
        /// ex: Y/3
        /// </summary>
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public string MayRepeat { get; set; }

        /// <summary>
        /// Gets/sets the ID of the VADS value set. For example, "PHVS_YesNoUnknown_CDC". This
        /// should be set to string.Empty if the data element does not have a 'Coded' data type.
        /// </summary>
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public string ValueSetCode { get; set; }

        /// <summary>
        /// Gets or sets the value set version number.
        /// </summary>
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public int? ValueSetVersionNumber { get; set; }

        /// <summary>
        /// Set from a helper function to create a link for the valueSet
        /// should display in markdown [Value Set Name](Value Set URL)
        /// </summary>
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public string ValueSetLink { get; set; }

        /// <summary>
        /// Gets/sets the date this entity was published internally.
        /// @remarks
        /// Internal publishing refers to things made available for other
        /// users of the MMGAT software, but that nevertheless remain
        /// unavailable for the public or anyone else outside of the
        /// software.
        /// </summary>
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public DateTime? InternalPublishDate { get; set; }

        /// <summary>
        /// Public Health Agency (PHA) Implementation
        /// </summary>
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public PublicHealthAgencyImplementation PublicHealthAgencyImplementation { get; set; }

        /// <summary>
        /// Mappings for this Data Element
        /// </summary>
        public DataElementMappings Mappings { get; set; }

        /// <summary>
        /// Default values for this element
        /// </summary>
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public DataElementValue DefaultValue { get; set; }

        /// <summary>
        /// Helper method to get value set
        /// </summary>
        public VADS.ValueSetWithConcept GetValueSet(MessageMappingGuide guide)
        {
            if (DataType != DataType.Coded)
            {
                return null;
            }
            if (guide.ValueSets == null)
            {
                return null;
            }

            string valueSetCode = ValueSetCode;
            if (Name == "Condition Code")
            {
                valueSetCode = "PHVS_NotifiableEvent_Disease_Condition_CDC_NNDSS";
            }

            var valueSet = guide.ValueSets.FirstOrDefault(vs => vs.ValueSet.ValueSetCode == valueSetCode);
            return valueSet;
        }
    }

    /// <summary>
    /// Defines the mapping for the data element
    /// </summary>
    public sealed class DataElementMappings
    {
        /// <summary>
        /// HL7V251 parsing
        /// </summary>
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public HL7V251.DataElementMapping Hl7v251 { get; set; }

        /// <summary>
        /// FHIRV4 parsing
        /// </summary>
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public FHIRV4.DataElementMapping Fhirv4 { get; set; }
    }
}