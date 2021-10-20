using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace CS.Mmg.HL7V251
{
    /// <summary>
    /// Enum for HL7 v2.5.1 data types
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum DataType
    {
        /// <summary>
        /// Sequence: 1
        /// Address
        /// </summary>
        [EnumMember(Value = "AD")]
        AD,

        /// <summary>
        /// Sequence: 2
        /// Authorization Information
        /// </summary>
        [EnumMember(Value = "AUI")]
        AUI,

        /// <summary>
        /// Sequence: 3
        /// Charge Code and Date
        /// </summary>
        [EnumMember(Value = "CCD")]
        CCD,

        /// <summary>
        /// Sequence: 4
        /// Channel Calibration Parameters
        /// </summary>
        [EnumMember(Value = "CCP")]
        CCP,

        /// <summary>
        /// Sequence: 5
        /// Channel Definition
        /// </summary>
        [EnumMember(Value = "CD")]
        CD,

        /// <summary>
        /// Sequence: 6
        /// Coded Element
        /// </summary>
        [EnumMember(Value = "CE")]
        CE,

        /// <summary>
        /// Sequence: 7
        /// Coded Element with Formatted Values
        /// </summary>
        [EnumMember(Value = "CF")]
        CF,

        /// <summary>
        /// Sequence: 8
        /// Coded with No Exceptions
        /// </summary>
        [EnumMember(Value = "CNE")]
        CNE,

        /// <summary>
        /// Sequence: 9
        /// Composite ID Number and Name Simplified
        /// </summary>
        [EnumMember(Value = "CNN")]
        CNN,

        /// <summary>
        /// Sequence: 10
        /// Composite Price
        /// </summary>
        [EnumMember(Value = "CP")]
        CP,

        /// <summary>
        /// Sequence: 11
        /// Composite Quantity with Units
        /// </summary>
        [EnumMember(Value = "CQ")]
        CQ,

        /// <summary>
        /// Sequence: 12
        /// Channel Sensitivity
        /// </summary>
        [EnumMember(Value = "CSU")]
        CSU,

        /// <summary>
        /// Sequence: 13
        /// Coded with Exceptions
        /// </summary>
        [EnumMember(Value = "CWE")]
        CWE,

        /// <summary>
        /// Sequence: 14
        /// Extended Composite ID with Check Digit
        /// </summary>
        [EnumMember(Value = "CX")]
        CX,

        /// <summary>
        /// Sequence: 15
        /// Daily Deductible Information
        /// </summary>
        [EnumMember(Value = "DDI")]
        DDI,

        /// <summary>
        /// Sequence: 16
        /// Date and Institution Name
        /// </summary>
        [EnumMember(Value = "DIN")]
        DIN,

        /// <summary>
        /// Sequence: 17
        /// Discharge Location and Date
        /// </summary>
        [EnumMember(Value = "DLD")]
        DLD,

        /// <summary>
        /// Sequence: 18
        /// Driver_s License Number
        /// </summary>
        [EnumMember(Value = "DLN")]
        DLN,

        /// <summary>
        /// Sequence: 19
        /// Delta
        /// </summary>
        [EnumMember(Value = "DLT")]
        DLT,

        /// <summary>
        /// Sequence: 20
        /// Date/Time Range
        /// </summary>
        [EnumMember(Value = "DR")]
        DR,

        /// <summary>
        /// Sequence: 21
        /// Date
        /// </summary>
        [EnumMember(Value = "DT")]
        DT,

        /// <summary>
        /// Sequence: 22
        /// Date/Time
        /// </summary>
        [EnumMember(Value = "DTM")]
        DTM,

        /// <summary>
        /// Sequence: 23
        /// Day Type and Number
        /// </summary>
        [EnumMember(Value = "DTN")]
        DTN,

        /// <summary>
        /// Sequence: 24
        /// Encapsulated Data
        /// </summary>
        [EnumMember(Value = "ED")]
        ED,

        /// <summary>
        /// Sequence: 25
        /// Entity Identifier
        /// </summary>
        [EnumMember(Value = "EI")]
        EI,

        /// <summary>
        /// Sequence: 26
        /// Entity Identifier Pair
        /// </summary>
        [EnumMember(Value = "EIP")]
        EIP,

        /// <summary>
        /// Sequence: 27
        /// Error Location and Description
        /// </summary>
        [EnumMember(Value = "ELD")]
        ELD,

        /// <summary>
        /// Sequence: 28
        /// Error Location
        /// </summary>
        [EnumMember(Value = "ERL")]
        ERL,

        /// <summary>
        /// Sequence: 29
        /// Financial Class
        /// </summary>
        [EnumMember(Value = "FC")]
        FC,

        /// <summary>
        /// Sequence: 30
        /// Family Name
        /// </summary>
        [EnumMember(Value = "FN")]
        FN,

        /// <summary>
        /// Sequence: 31
        /// Formatted Text Data
        /// </summary>
        [EnumMember(Value = "FT")]
        FT,

        /// <summary>
        /// Sequence: 32
        /// General Timing Specification
        /// </summary>
        [EnumMember(Value = "GTS")]
        GTS,

        /// <summary>
        /// Sequence: 33
        /// Hierarchic Designator
        /// </summary>
        [EnumMember(Value = "HD")]
        HD,

        /// <summary>
        /// Sequence: 34
        /// Insurance Certification Definition
        /// </summary>
        [EnumMember(Value = "ICD")]
        ICD,

        /// <summary>
        /// Sequence: 35
        /// Coded values for HL7 tables
        /// </summary>
        [EnumMember(Value = "ID")]
        ID,

        /// <summary>
        /// Sequence: 36
        /// Coded value for user-defined tables
        /// </summary>
        [EnumMember(Value = "IS")]
        IS,

        /// <summary>
        /// Sequence: 37
        /// Job Code/Class
        /// </summary>
        [EnumMember(Value = "JCC")]
        JCC,

        /// <summary>
        /// Sequence: 38
        /// Location with Address Variation 1
        /// </summary>
        [EnumMember(Value = "LA1")]
        LA1,

        /// <summary>
        /// Sequence: 39
        /// Location with Address Variation 2
        /// </summary>
        [EnumMember(Value = "LA2")]
        LA2,

        /// <summary>
        /// Sequence: 40
        /// Multiplexed Array
        /// </summary>
        [EnumMember(Value = "MA")]
        MA,

        /// <summary>
        /// Sequence: 41
        /// Money
        /// </summary>
        [EnumMember(Value = "MO")]
        MO,

        /// <summary>
        /// Sequence: 42
        /// Money and Code
        /// </summary>
        [EnumMember(Value = "MOC")]
        MOC,

        /// <summary>
        /// Sequence: 43
        /// Money or Percentage
        /// </summary>
        [EnumMember(Value = "MOP")]
        MOP,

        /// <summary>
        /// Sequence: 44
        /// Message Type
        /// </summary>
        [EnumMember(Value = "MSG")]
        MSG,

        /// <summary>
        /// Sequence: 45
        /// Numeric Array
        /// </summary>
        [EnumMember(Value = "NA")]
        NA,

        /// <summary>
        /// Sequence: 46
        /// Name with Date and Location
        /// </summary>
        [EnumMember(Value = "NDL")]
        NDL,

        /// <summary>
        /// Sequence: 47
        /// Numeric
        /// </summary>
        [EnumMember(Value = "NM")]
        NM,

        /// <summary>
        /// Sequence: 48
        /// Numeric Range
        /// </summary>
        [EnumMember(Value = "NR")]
        NR,

        /// <summary>
        /// Sequence: 49
        /// Occurrence Code and Date
        /// </summary>
        [EnumMember(Value = "OCD")]
        OCD,

        /// <summary>
        /// Sequence: 50
        /// Order Sequence Definition
        /// </summary>
        [EnumMember(Value = "OSD")]
        OSD,

        /// <summary>
        /// Sequence: 51
        /// Occurrence Span Code and Date
        /// </summary>
        [EnumMember(Value = "OSP")]
        OSP,

        /// <summary>
        /// Sequence: 52
        /// Practitioner Institutional Privileges
        /// </summary>
        [EnumMember(Value = "PIP")]
        PIP,

        /// <summary>
        /// Sequence: 53
        /// Person Location
        /// </summary>
        [EnumMember(Value = "PL")]
        PL,

        /// <summary>
        /// Sequence: 54
        /// Practitioner License or Other ID Number
        /// </summary>
        [EnumMember(Value = "PLN")]
        PLN,

        /// <summary>
        /// Sequence: 55
        /// Performing Person Time Stamp
        /// </summary>
        [EnumMember(Value = "PPN")]
        PPN,

        /// <summary>
        /// Sequence: 56
        /// Parent Result Link
        /// </summary>
        [EnumMember(Value = "PRL")]
        PRL,

        /// <summary>
        /// Sequence: 57
        /// Processing Type
        /// </summary>
        [EnumMember(Value = "PT")]
        PT,

        /// <summary>
        /// Sequence: 58
        /// Policy Type and Amount
        /// </summary>
        [EnumMember(Value = "PTA")]
        PTA,

        /// <summary>
        /// Sequence: 59
        /// Query Input Parameter List
        /// </summary>
        [EnumMember(Value = "QIP")]
        QIP,

        /// <summary>
        /// Sequence: 60
        /// Query Selection Criteria
        /// </summary>
        [EnumMember(Value = "QSC")]
        QSC,

        /// <summary>
        /// Sequence: 61
        /// Row Column Definition
        /// </summary>
        [EnumMember(Value = "RCD")]
        RCD,

        /// <summary>
        /// Sequence: 62
        /// Reference Range
        /// </summary>
        [EnumMember(Value = "RFR")]
        RFR,

        /// <summary>
        /// Sequence: 63
        /// Repeat Interval
        /// </summary>
        [EnumMember(Value = "RI")]
        RI,

        /// <summary>
        /// Sequence: 64
        /// Room Coverage
        /// </summary>
        [EnumMember(Value = "RMC")]
        RMC,

        /// <summary>
        /// Sequence: 65
        /// Reference Pointer
        /// </summary>
        [EnumMember(Value = "RP")]
        RP,

        /// <summary>
        /// Sequence: 66
        /// Repeat Pattern
        /// </summary>
        [EnumMember(Value = "RPT")]
        RPT,

        /// <summary>
        /// Sequence: 67
        /// Street Address
        /// </summary>
        [EnumMember(Value = "SAD")]
        SAD,

        /// <summary>
        /// Sequence: 68
        /// Scheduling Class Value Pair
        /// </summary>
        [EnumMember(Value = "SCV")]
        SCV,

        /// <summary>
        /// Sequence: 69
        /// Sequence ID
        /// </summary>
        [EnumMember(Value = "SI")]
        SI,

        /// <summary>
        /// Sequence: 70
        /// Structured Numeric
        /// </summary>
        [EnumMember(Value = "SN")]
        SN,

        /// <summary>
        /// Sequence: 71
        /// Specialty Description
        /// </summary>
        [EnumMember(Value = "SPD")]
        SPD,

        /// <summary>
        /// Sequence: 72
        /// Specimen Source
        /// </summary>
        [EnumMember(Value = "SPS")]
        SPS,

        /// <summary>
        /// Sequence: 73
        /// Sort Order
        /// </summary>
        [EnumMember(Value = "SRT")]
        SRT,

        /// <summary>
        /// Sequence: 74
        /// String Data
        /// </summary>
        [EnumMember(Value = "ST")]
        ST,

        /// <summary>
        /// Sequence: 75
        /// Time
        /// </summary>
        [EnumMember(Value = "TM")]
        TM,

        /// <summary>
        /// Sequence: 76
        /// Timing Quantity
        /// </summary>
        [EnumMember(Value = "TQ")]
        TQ,

        /// <summary>
        /// Sequence: 77
        /// Time Stamp
        /// </summary>
        [EnumMember(Value = "TS")]
        TS,

        /// <summary>
        /// Sequence: 78
        /// Text Data
        /// </summary>
        [EnumMember(Value = "TX")]
        TX,

        /// <summary>
        /// Sequence: 79
        /// UB Value Code and Amount
        /// </summary>
        [EnumMember(Value = "UVC")]
        UVC,

        /// <summary>
        /// Sequence: 80
        /// Variable Datatype
        /// </summary>
        [EnumMember(Value = "VARIES")]
        VARIES,

        /// <summary>
        /// Sequence: 81
        /// Visiting Hours
        /// </summary>
        [EnumMember(Value = "VH")]
        VH,

        /// <summary>
        /// Sequence: 82
        /// Version Identifier
        /// </summary>
        [EnumMember(Value = "VID")]
        VID,

        /// <summary>
        /// Sequence: 83
        /// Value Range
        /// </summary>
        [EnumMember(Value = "VR")]
        VR,

        /// <summary>
        /// Sequence: 84
        /// Virtual Table Query Request
        /// </summary>
        [EnumMember(Value = "VTQ")]
        VTQ,

        /// <summary>
        /// Sequence: 85
        /// Channel Identifier
        /// </summary>
        [EnumMember(Value = "WVI")]
        WVI,

        /// <summary>
        /// Sequence: 86
        /// Waveform Source
        /// </summary>
        [EnumMember(Value = "WVS")]
        WVS,

        /// <summary>
        /// Sequence: 87
        /// Extended Address
        /// </summary>
        [EnumMember(Value = "XAD")]
        XAD,

        /// <summary>
        /// Sequence: 88
        /// Extended Composite ID Number and Name for Persons
        /// </summary>
        [EnumMember(Value = "XCN")]
        XCN,

        /// <summary>
        /// Sequence: 89
        /// Extended Composite Name and Identification Number for Organizations
        /// </summary>
        [EnumMember(Value = "XON")]
        XON,

        /// <summary>
        /// Sequence: 90
        /// Extended Person Name
        /// </summary>
        [EnumMember(Value = "XPN")]
        XPN,

        /// <summary>
        /// Sequence: 91
        /// Extended Telecommunication Number
        /// </summary>
        [EnumMember(Value = "XTN")]
        XTN,
    }
}