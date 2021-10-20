using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace CS.Mmg.HL7V251
{
    /// <summary>
    /// Enum for HL7 v2.5.1 segment types
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum SegmentType
    {
        /// <summary>
        /// Abstract
        /// </summary>
        [EnumMember(Value = "ABS")]
        ABS,

        /// <summary>
        /// Accident
        /// </summary>
        [EnumMember(Value = "ACC")]
        ACC,

        /// <summary>
        /// Addendum
        /// </summary>
        [EnumMember(Value = "ADD")]
        ADD,

        /// <summary>
        /// Professional affiliation
        /// </summary>
        [EnumMember(Value = "AFF")]
        AFF,

        /// <summary>
        /// Appointment Information - General Resource
        /// </summary>
        [EnumMember(Value = "AIG")]
        AIG,

        /// <summary>
        /// Appointment Information - Location Resource
        /// </summary>
        [EnumMember(Value = "AIL")]
        AIL,

        /// <summary>
        /// Appointment Information - Personnel Resource
        /// </summary>
        [EnumMember(Value = "AIP")]
        AIP,

        /// <summary>
        /// Appointment information
        /// </summary>
        [EnumMember(Value = "AIS")]
        AIS,

        /// <summary>
        /// Patient allergy information
        /// </summary>
        [EnumMember(Value = "AL1")]
        AL1,

        /// <summary>
        /// Appointment Preferences
        /// </summary>
        [EnumMember(Value = "APR")]
        APR,

        /// <summary>
        /// Appointment request
        /// </summary>
        [EnumMember(Value = "ARQ")]
        ARQ,

        /// <summary>
        /// Authorization Information
        /// </summary>
        [EnumMember(Value = "AUT")]
        AUT,

        /// <summary>
        /// Batch Header
        /// </summary>
        [EnumMember(Value = "BHS")]
        BHS,

        /// <summary>
        /// Blood code
        /// </summary>
        [EnumMember(Value = "BLC")]
        BLC,

        /// <summary>
        /// Billing
        /// </summary>
        [EnumMember(Value = "BLG")]
        BLG,

        /// <summary>
        /// Blood product order
        /// </summary>
        [EnumMember(Value = "BPO")]
        BPO,

        /// <summary>
        /// Blood product dispense status
        /// </summary>
        [EnumMember(Value = "BPX")]
        BPX,

        /// <summary>
        /// Batch trailer
        /// </summary>
        [EnumMember(Value = "BTS")]
        BTS,

        /// <summary>
        /// Blood Product Transfusion/Disposition
        /// </summary>
        [EnumMember(Value = "BTX")]
        BTX,

        /// <summary>
        /// Charge Description Master
        /// </summary>
        [EnumMember(Value = "CDM")]
        CDM,

        /// <summary>
        /// Certificate Detail
        /// </summary>
        [EnumMember(Value = "CER")]
        CER,

        /// <summary>
        /// Clinical Study Master
        /// </summary>
        [EnumMember(Value = "CM0")]
        CM0,

        /// <summary>
        /// Clinical Study Phase Master
        /// </summary>
        [EnumMember(Value = "CM1")]
        CM1,

        /// <summary>
        /// Clinical Study Schedule Master
        /// </summary>
        [EnumMember(Value = "CM2")]
        CM2,

        /// <summary>
        /// Clear notification
        /// </summary>
        [EnumMember(Value = "CNS")]
        CNS,

        /// <summary>
        /// Consent segment
        /// </summary>
        [EnumMember(Value = "CON")]
        CON,

        /// <summary>
        /// Clinical study phase
        /// </summary>
        [EnumMember(Value = "CSP")]
        CSP,

        /// <summary>
        /// Clinical study registration
        /// </summary>
        [EnumMember(Value = "CSR")]
        CSR,

        /// <summary>
        /// Clinuial study data schedule segment
        /// </summary>
        [EnumMember(Value = "CSS")]
        CSS,

        /// <summary>
        /// Contact data
        /// </summary>
        [EnumMember(Value = "CTD")]
        CTD,

        /// <summary>
        /// Clinical trial identification
        /// </summary>
        [EnumMember(Value = "CTI")]
        CTI,

        /// <summary>
        /// Disability
        /// </summary>
        [EnumMember(Value = "DB1")]
        DB1,

        /// <summary>
        /// Diagnosis
        /// </summary>
        [EnumMember(Value = "DG1")]
        DG1,

        /// <summary>
        /// Diagnosis Related Group
        /// </summary>
        [EnumMember(Value = "DRG")]
        DRG,

        /// <summary>
        /// Continuation pointer
        /// </summary>
        [EnumMember(Value = "DSC")]
        DSC,

        /// <summary>
        /// Display data
        /// </summary>
        [EnumMember(Value = "DSP")]
        DSP,

        /// <summary>
        /// Equipment Command
        /// </summary>
        [EnumMember(Value = "ECD")]
        ECD,

        /// <summary>
        /// Equipment Command response
        /// </summary>
        [EnumMember(Value = "ECR")]
        ECR,

        /// <summary>
        /// Educational Detail
        /// </summary>
        [EnumMember(Value = "EDU")]
        EDU,

        /// <summary>
        /// Embedded Query Language
        /// </summary>
        [EnumMember(Value = "EQL")]
        EQL,

        /// <summary>
        /// Equipment/log Service
        /// </summary>
        [EnumMember(Value = "EQP")]
        EQP,

        /// <summary>
        /// Equipment Detail
        /// </summary>
        [EnumMember(Value = "EQU")]
        EQU,

        /// <summary>
        /// Event replay query
        /// </summary>
        [EnumMember(Value = "ERQ")]
        ERQ,

        /// <summary>
        /// Error
        /// </summary>
        [EnumMember(Value = "ERR")]
        ERR,

        /// <summary>
        /// Event Type
        /// </summary>
        [EnumMember(Value = "EVN")]
        EVN,

        /// <summary>
        /// Facility
        /// </summary>
        [EnumMember(Value = "FAC")]
        FAC,

        /// <summary>
        /// File header
        /// </summary>
        [EnumMember(Value = "FHS")]
        FHS,

        /// <summary>
        /// Financial Transaction
        /// </summary>
        [EnumMember(Value = "FT1")]
        FT1,

        /// <summary>
        /// File trailer
        /// </summary>
        [EnumMember(Value = "FTS")]
        FTS,

        /// <summary>
        /// Goal detail
        /// </summary>
        [EnumMember(Value = "GOL")]
        GOL,

        /// <summary>
        /// Grouping/Reimbursement - Visit
        /// </summary>
        [EnumMember(Value = "GP1")]
        GP1,

        /// <summary>
        /// Grouping/Reimbursement - Procedure Line Item
        /// </summary>
        [EnumMember(Value = "GP2")]
        GP2,

        /// <summary>
        /// Guarantor
        /// </summary>
        [EnumMember(Value = "GT1")]
        GT1,

        /// <summary>
            ///
        /// </summary>
        [EnumMember(Value = "Hxx")]
        Hxx,

        /// <summary>
        /// Patient Adverse Reaction Information
        /// </summary>
        [EnumMember(Value = "IAM")]
        IAM,

        /// <summary>
        /// Inventory Item Master
        /// </summary>
        [EnumMember(Value = "IIM")]
        IIM,

        /// <summary>
        /// Insurance
        /// </summary>
        [EnumMember(Value = "IN1")]
        IN1,

        /// <summary>
        /// Insurance Additional Information
        /// </summary>
        [EnumMember(Value = "IN2")]
        IN2,

        /// <summary>
        /// Insurance Additional Information, Certification
        /// </summary>
        [EnumMember(Value = "IN3")]
        IN3,

        /// <summary>
        /// Inventory detail
        /// </summary>
        [EnumMember(Value = "INV")]
        INV,

        /// <summary>
        /// Imaging Procedure Control Segment
        /// </summary>
        [EnumMember(Value = "IPC")]
        IPC,

        /// <summary>
        /// Interaction Status Detail
        /// </summary>
        [EnumMember(Value = "ISD")]
        ISD,

        /// <summary>
        /// Language Detail
        /// </summary>
        [EnumMember(Value = "LAN")]
        LAN,

        /// <summary>
        /// Location Charge Code
        /// </summary>
        [EnumMember(Value = "LCC")]
        LCC,

        /// <summary>
        /// Location Characteristic
        /// </summary>
        [EnumMember(Value = "LCH")]
        LCH,

        /// <summary>
        /// Location department
        /// </summary>
        [EnumMember(Value = "LDP")]
        LDP,

        /// <summary>
        /// Location identification
        /// </summary>
        [EnumMember(Value = "LOC")]
        LOC,

        /// <summary>
        /// Location relationship
        /// </summary>
        [EnumMember(Value = "LRL")]
        LRL,

        /// <summary>
        /// Master File Acknowledgment
        /// </summary>
        [EnumMember(Value = "MFA")]
        MFA,

        /// <summary>
        /// Master File Entry
        /// </summary>
        [EnumMember(Value = "MFE")]
        MFE,

        /// <summary>
        /// Master File Identification
        /// </summary>
        [EnumMember(Value = "MFI")]
        MFI,

        /// <summary>
        /// Merge Patient Information
        /// </summary>
        [EnumMember(Value = "MRG")]
        MRG,

        /// <summary>
        /// Message Acknowledgment
        /// </summary>
        [EnumMember(Value = "MSA")]
        MSA,

        /// <summary>
        /// Message Header
        /// </summary>
        [EnumMember(Value = "MSH")]
        MSH,

        /// <summary>
        /// System Clock
        /// </summary>
        [EnumMember(Value = "NCK")]
        NCK,

        /// <summary>
        /// Notification Detail
        /// </summary>
        [EnumMember(Value = "NDS")]
        NDS,

        /// <summary>
        /// Next of Kin / Associated Pa
        /// </summary>
        [EnumMember(Value = "NK1")]
        NK1,

        /// <summary>
        /// Bed Status Update
        /// </summary>
        [EnumMember(Value = "NPU")]
        NPU,

        /// <summary>
        /// Application Status Change
        /// </summary>
        [EnumMember(Value = "NSC")]
        NSC,

        /// <summary>
        /// Application control level statistics
        /// </summary>
        [EnumMember(Value = "NST")]
        NST,

        /// <summary>
        /// Notes and Comments
        /// </summary>
        [EnumMember(Value = "NTE")]
        NTE,

        /// <summary>
        /// Observation Request
        /// </summary>
        [EnumMember(Value = "OBR")]
        OBR,

        /// <summary>
        /// Observation/Result
        /// </summary>
        [EnumMember(Value = "OBX")]
        OBX,

        /// <summary>
        /// Dietary Orders, Supplements, and Preferences
        /// </summary>
        [EnumMember(Value = "ODS")]
        ODS,

        /// <summary>
        /// Diet Tray Instructions
        /// </summary>
        [EnumMember(Value = "ODT")]
        ODT,

        /// <summary>
        /// General Segment
        /// </summary>
        [EnumMember(Value = "OM1")]
        OM1,

        /// <summary>
        /// Numeric Observation
        /// </summary>
        [EnumMember(Value = "OM2")]
        OM2,

        /// <summary>
        /// Categorical Service/Test/Observation
        /// </summary>
        [EnumMember(Value = "OM3")]
        OM3,

        /// <summary>
        /// Observations that Require Specimens
        /// </summary>
        [EnumMember(Value = "OM4")]
        OM4,

        /// <summary>
        /// Observation Batteries (Sets)
        /// </summary>
        [EnumMember(Value = "OM5")]
        OM5,

        /// <summary>
        /// Observations that are Calculated from Other Observations
        /// </summary>
        [EnumMember(Value = "OM6")]
        OM6,

        /// <summary>
        /// Additional Basic Attributes
        /// </summary>
        [EnumMember(Value = "OM7")]
        OM7,

        /// <summary>
        /// Common Order
        /// </summary>
        [EnumMember(Value = "ORC")]
        ORC,

        /// <summary>
        /// Practitioner Organization Unit
        /// </summary>
        [EnumMember(Value = "ORG")]
        ORG,

        /// <summary>
        /// Override Segment
        /// </summary>
        [EnumMember(Value = "OVR")]
        OVR,

        /// <summary>
        /// Possible Causal Relationship
        /// </summary>
        [EnumMember(Value = "PCR")]
        PCR,

        /// <summary>
        /// Patient Additional Demographic
        /// </summary>
        [EnumMember(Value = "PD1")]
        PD1,

        /// <summary>
        /// Patient Death and Autopsy
        /// </summary>
        [EnumMember(Value = "PDA")]
        PDA,

        /// <summary>
        /// Product Detail Country
        /// </summary>
        [EnumMember(Value = "PDC")]
        PDC,

        /// <summary>
        /// Product Experience Observation
        /// </summary>
        [EnumMember(Value = "PEO")]
        PEO,

        /// <summary>
        /// Product Experience Sender
        /// </summary>
        [EnumMember(Value = "PES")]
        PES,

        /// <summary>
        /// Patient Identification
        /// </summary>
        [EnumMember(Value = "PID")]
        PID,

        /// <summary>
        /// Procedures
        /// </summary>
        [EnumMember(Value = "PR1")]
        PR1,

        /// <summary>
        /// Practitioner Detail
        /// </summary>
        [EnumMember(Value = "PRA")]
        PRA,

        /// <summary>
        /// Problem Details
        /// </summary>
        [EnumMember(Value = "PRB")]
        PRB,

        /// <summary>
        /// Pricing
        /// </summary>
        [EnumMember(Value = "PRC")]
        PRC,

        /// <summary>
        /// Provider data
        /// </summary>
        [EnumMember(Value = "PRD")]
        PRD,

        /// <summary>
        /// Product summary header
        /// </summary>
        [EnumMember(Value = "PSH")]
        PSH,

        /// <summary>
        /// Pathway
        /// </summary>
        [EnumMember(Value = "PTH")]
        PTH,

        /// <summary>
        /// Patient visit
        /// </summary>
        [EnumMember(Value = "PV1")]
        PV1,

        /// <summary>
        /// Patient Visit - Additional Information
        /// </summary>
        [EnumMember(Value = "PV2")]
        PV2,

        /// <summary>
        /// Query Acknowledgment
        /// </summary>
        [EnumMember(Value = "QAK")]
        QAK,

        /// <summary>
        /// Query Identification
        /// </summary>
        [EnumMember(Value = "QID")]
        QID,

        /// <summary>
        /// Query Parameter Definition
        /// </summary>
        [EnumMember(Value = "QPD")]
        QPD,

        /// <summary>
        /// Original-Style Query Definition
        /// </summary>
        [EnumMember(Value = "QRD")]
        QRD,

        /// <summary>
        /// Original style query filter
        /// </summary>
        [EnumMember(Value = "QRF")]
        QRF,

        /// <summary>
        /// Query Response Instance
        /// </summary>
        [EnumMember(Value = "QRI")]
        QRI,

        /// <summary>
        /// Response Control Parameter
        /// </summary>
        [EnumMember(Value = "RCP")]
        RCP,

        /// <summary>
        /// Table Row Definition
        /// </summary>
        [EnumMember(Value = "RDF")]
        RDF,

        /// <summary>
        /// Table Row Data
        /// </summary>
        [EnumMember(Value = "RDT")]
        RDT,

        /// <summary>
        /// Referral Information
        /// </summary>
        [EnumMember(Value = "RF1")]
        RF1,

        /// <summary>
        /// Resource Group
        /// </summary>
        [EnumMember(Value = "RGS")]
        RGS,

        /// <summary>
        /// Risk Management Incident
        /// </summary>
        [EnumMember(Value = "RMI")]
        RMI,

        /// <summary>
        /// Role
        /// </summary>
        [EnumMember(Value = "ROL")]
        ROL,

        /// <summary>
        /// Requisition Detail-1
        /// </summary>
        [EnumMember(Value = "RQ1")]
        RQ1,

        /// <summary>
        /// Requisition Detail
        /// </summary>
        [EnumMember(Value = "RQD")]
        RQD,

        /// <summary>
        /// Pharmacy/Treatment Administration
        /// </summary>
        [EnumMember(Value = "RXA")]
        RXA,

        /// <summary>
        /// Pharmacy/Treatment Component Order
        /// </summary>
        [EnumMember(Value = "RXC")]
        RXC,

        /// <summary>
        /// Pharmacy/Treatment Dispense
        /// </summary>
        [EnumMember(Value = "RXD")]
        RXD,

        /// <summary>
        /// Pharmacy/Treatment Encoded Order
        /// </summary>
        [EnumMember(Value = "RXE")]
        RXE,

        /// <summary>
        /// Pharmacy/Treatment Encoded Give
        /// </summary>
        [EnumMember(Value = "RXG")]
        RXG,

        /// <summary>
        /// Pharmacy/Treatment Order
        /// </summary>
        [EnumMember(Value = "RXO")]
        RXO,

        /// <summary>
        /// Pharmacy/Treatment Route
        /// </summary>
        [EnumMember(Value = "RXR")]
        RXR,

        /// <summary>
        /// Specimen Container detail
        /// </summary>
        [EnumMember(Value = "SAC")]
        SAC,

        /// <summary>
        /// Scheduling Activity Information
        /// </summary>
        [EnumMember(Value = "SCH")]
        SCH,

        /// <summary>
        /// Software Segment
        /// </summary>
        [EnumMember(Value = "SFT")]
        SFT,

        /// <summary>
        /// Substance Identifier
        /// </summary>
        [EnumMember(Value = "SID")]
        SID,

        /// <summary>
        /// Specimen
        /// </summary>
        [EnumMember(Value = "SPM")]
        SPM,

        /// <summary>
        /// Stored Procedure Request Definition
        /// </summary>
        [EnumMember(Value = "SPR")]
        SPR,

        /// <summary>
        /// Staff Identification
        /// </summary>
        [EnumMember(Value = "STF")]
        STF,

        /// <summary>
        /// Test Code Configuration
        /// </summary>
        [EnumMember(Value = "TCC")]
        TCC,

        /// <summary>
        /// Test Code Detail
        /// </summary>
        [EnumMember(Value = "TCD")]
        TCD,

        /// <summary>
        /// Timing/Quantity
        /// </summary>
        [EnumMember(Value = "TQ1")]
        TQ1,

        /// <summary>
        /// Timing/Quantity Relationship
        /// </summary>
        [EnumMember(Value = "TQ2")]
        TQ2,

        /// <summary>
        /// Transcription Document Header
        /// </summary>
        [EnumMember(Value = "TXA")]
        TXA,

        /// <summary>
        /// UB82
        /// </summary>
        [EnumMember(Value = "UB1")]
        UB1,

        /// <summary>
        /// UB92 Data
        /// </summary>
        [EnumMember(Value = "UB2")]
        UB2,

        /// <summary>
        /// Results/update definition
        /// </summary>
        [EnumMember(Value = "URD")]
        URD,

        /// <summary>
        /// Unsolicited Selection
        /// </summary>
        [EnumMember(Value = "URS")]
        URS,

        /// <summary>
        /// Variance
        /// </summary>
        [EnumMember(Value = "VAR")]
        VAR,

        /// <summary>
        /// Virtual Table Query Request
        /// </summary>
        [EnumMember(Value = "VTQ")]
        VTQ,

        /// <summary>
        /// (proposed example only)
        /// </summary>
        [EnumMember(Value = "ZL7")]
        ZL7,

        /// <summary>
        ///
        /// </summary>
        [EnumMember(Value = "Zxx")]
        Zxx,
    }
}