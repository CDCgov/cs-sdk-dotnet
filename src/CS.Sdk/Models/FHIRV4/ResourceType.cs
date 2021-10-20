using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace CS.Mmg.FHIRV4
{
    /// <summary>
    /// Enum for Resource
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ResourceType
    {
        /// <summary>
        /// Account
        /// </summary>
        [EnumMember(Value = "Account")]
        Account,
        
        /// <summary>
        /// ActivityDefin
        /// </summary>
        [EnumMember(Value = "ActivityDefinition")]
        ActivityDefinition,
        
        /// <summary>
        /// AdverseEvent
        /// </summary>
        [EnumMember(Value = "AdverseEvent")]
        AdverseEvent,
        
        /// <summary>
        /// AllergyIntole
        /// </summary>
        [EnumMember(Value = "AllergyIntolerance")]
        AllergyIntolerance,
        
        /// <summary>
        /// Appointment
        /// </summary>
        [EnumMember(Value = "Appointment")]
        Appointment,
        
        /// <summary>
        /// AppointmentRe
        /// </summary>
        [EnumMember(Value = "AppointmentResponse")]
        AppointmentResponse,
        
        /// <summary>
        /// AuditEvent
        /// </summary>
        [EnumMember(Value = "AuditEvent")]
        AuditEvent,
        
        /// <summary>
        /// Basic
        /// </summary>
        [EnumMember(Value = "Basic")]
        Basic,
        
        /// <summary>
        /// Binary
        /// </summary>
        [EnumMember(Value = "Binary")]
        Binary,
        
        /// <summary>
        /// BiologicallyD
        /// </summary>
        [EnumMember(Value = "BiologicallyDerivedP")]
        BiologicallyDerivedProduct,
        
        /// <summary>
        /// BodyStructure
        /// </summary>
        [EnumMember(Value = "BodyStructure")]
        BodyStructure,
        
        /// <summary>
        /// Bundle
        /// </summary>
        [EnumMember(Value = "Bundle")]
        Bundle,
        
        /// <summary>
        /// CapabilitySta
        /// </summary>
        [EnumMember(Value = "CapabilityStatement")]
        CapabilityStatement,
        
        /// <summary>
        /// CarePlan
        /// </summary>
        [EnumMember(Value = "CarePlan")]
        CarePlan,
        
        /// <summary>
        /// CareTeam
        /// </summary>
        [EnumMember(Value = "CareTeam")]
        CareTeam,
        
        /// <summary>
        /// CatalogEntry
        /// </summary>
        [EnumMember(Value = "CatalogEntry")]
        CatalogEntry,
        
        /// <summary>
        /// ChargeItem
        /// </summary>
        [EnumMember(Value = "ChargeItem")]
        ChargeItem,
        
        /// <summary>
        /// ChargeItemDef
        /// </summary>
        [EnumMember(Value = "ChargeItemDefinition")]
        ChargeItemDefinition,
        
        /// <summary>
        /// Claim
        /// </summary>
        [EnumMember(Value = "Claim")]
        Claim,
        
        /// <summary>
        /// ClaimResponse
        /// </summary>
        [EnumMember(Value = "ClaimResponse")]
        ClaimResponse,
        
        /// <summary>
        /// ClinicalImpre
        /// </summary>
        [EnumMember(Value = "ClinicalImpression")]
        ClinicalImpression,
        
        /// <summary>
        /// CodeSystem
        /// </summary>
        [EnumMember(Value = "CodeSystem")]
        CodeSystem,
        
        /// <summary>
        /// Communication
        /// </summary>
        [EnumMember(Value = "Communication")]
        Communication,
        
        /// <summary>
        /// Communication
        /// </summary>
        [EnumMember(Value = "CommunicationRequest")]
        CommunicationRequest,
        
        /// <summary>
        /// CompartmentDe
        /// </summary>
        [EnumMember(Value = "CompartmentDefinitio")]
        CompartmentDefinition,
        
        /// <summary>
        /// Composition
        /// </summary>
        [EnumMember(Value = "Composition")]
        Composition,
        
        /// <summary>
        /// ConceptMap
        /// </summary>
        [EnumMember(Value = "ConceptMap")]
        ConceptMap,
        
        /// <summary>
        /// Condition
        /// </summary>
        [EnumMember(Value = "Condition")]
        Condition,
        
        /// <summary>
        /// Consent
        /// </summary>
        [EnumMember(Value = "Consent")]
        Consent,
        
        /// <summary>
        /// Contract
        /// </summary>
        [EnumMember(Value = "Contract")]
        Contract,
        
        /// <summary>
        /// Coverage
        /// </summary>
        [EnumMember(Value = "Coverage")]
        Coverage,
        
        /// <summary>
        /// CoverageEligi
        /// </summary>
        [EnumMember(Value = "CoverageEligibilityR")]
        CoverageEligibilityRequest,
        
        /// <summary>
        /// CoverageEligi
        /// </summary>
        [EnumMember(Value = "CoverageEligibilityR")]
        CoverageEligibilityResponse,
        
        /// <summary>
        /// DetectedIssue
        /// </summary>
        [EnumMember(Value = "DetectedIssue")]
        DetectedIssue,
        
        /// <summary>
        /// Device
        /// </summary>
        [EnumMember(Value = "Device")]
        Device,
        
        /// <summary>
        /// DeviceDefinit
        /// </summary>
        [EnumMember(Value = "DeviceDefinition")]
        DeviceDefinition,
        
        /// <summary>
        /// DeviceMetric
        /// </summary>
        [EnumMember(Value = "DeviceMetric")]
        DeviceMetric,
        
        /// <summary>
        /// DeviceRequest
        /// </summary>
        [EnumMember(Value = "DeviceRequest")]
        DeviceRequest,
        
        /// <summary>
        /// DeviceUseStat
        /// </summary>
        [EnumMember(Value = "DeviceUseStatement")]
        DeviceUseStatement,
        
        /// <summary>
        /// DiagnosticRep
        /// </summary>
        [EnumMember(Value = "DiagnosticReport")]
        DiagnosticReport,
        
        /// <summary>
        /// DocumentManif
        /// </summary>
        [EnumMember(Value = "DocumentManifest")]
        DocumentManifest,
        
        /// <summary>
        /// DocumentRefer
        /// </summary>
        [EnumMember(Value = "DocumentReference")]
        DocumentReference,
        
        /// <summary>
        /// EffectEvidenc
        /// </summary>
        [EnumMember(Value = "EffectEvidenceSynthe")]
        EffectEvidenceSynthesis,
        
        /// <summary>
        /// Encounter
        /// </summary>
        [EnumMember(Value = "Encounter")]
        Encounter,
        
        /// <summary>
        /// Endpoint
        /// </summary>
        [EnumMember(Value = "Endpoint")]
        Endpoint,
        
        /// <summary>
        /// EnrollmentReq
        /// </summary>
        [EnumMember(Value = "EnrollmentRequest")]
        EnrollmentRequest,
        
        /// <summary>
        /// EnrollmentRes
        /// </summary>
        [EnumMember(Value = "EnrollmentResponse")]
        EnrollmentResponse,
        
        /// <summary>
        /// EpisodeOfCare
        /// </summary>
        [EnumMember(Value = "EpisodeOfCare")]
        EpisodeOfCare,
        
        /// <summary>
        /// EventDefiniti
        /// </summary>
        [EnumMember(Value = "EventDefinition")]
        EventDefinition,
        
        /// <summary>
        /// Evidence
        /// </summary>
        [EnumMember(Value = "Evidence")]
        Evidence,
        
        /// <summary>
        /// EvidenceVaria
        /// </summary>
        [EnumMember(Value = "EvidenceVariable")]
        EvidenceVariable,
        
        /// <summary>
        /// ExampleScenar
        /// </summary>
        [EnumMember(Value = "ExampleScenario")]
        ExampleScenario,
        
        /// <summary>
        /// ExplanationOf
        /// </summary>
        [EnumMember(Value = "ExplanationOfBenefit")]
        ExplanationOfBenefit,
        
        /// <summary>
        /// FamilyMemberH
        /// </summary>
        [EnumMember(Value = "FamilyMemberHistory")]
        FamilyMemberHistory,
        
        /// <summary>
        /// Flag
        /// </summary>
        [EnumMember(Value = "Flag")]
        Flag,
        
        /// <summary>
        /// Goal
        /// </summary>
        [EnumMember(Value = "Goal")]
        Goal,
        
        /// <summary>
        /// GraphDefiniti
        /// </summary>
        [EnumMember(Value = "GraphDefinition")]
        GraphDefinition,
        
        /// <summary>
        /// Group
        /// </summary>
        [EnumMember(Value = "Group")]
        Group,
        
        /// <summary>
        /// GuidanceRespo
        /// </summary>
        [EnumMember(Value = "GuidanceResponse")]
        GuidanceResponse,
        
        /// <summary>
        /// HealthcareSer
        /// </summary>
        [EnumMember(Value = "HealthcareService")]
        HealthcareService,
        
        /// <summary>
        /// ImagingStudy
        /// </summary>
        [EnumMember(Value = "ImagingStudy")]
        ImagingStudy,
        
        /// <summary>
        /// Immunization
        /// </summary>
        [EnumMember(Value = "Immunization")]
        Immunization,
        
        /// <summary>
        /// ImmunizationE
        /// </summary>
        [EnumMember(Value = "ImmunizationEvaluati")]
        ImmunizationEvaluation,
        
        /// <summary>
        /// ImmunizationR
        /// </summary>
        [EnumMember(Value = "ImmunizationRecommen")]
        ImmunizationRecommendation,
        
        /// <summary>
        /// Implementatio
        /// </summary>
        [EnumMember(Value = "ImplementationGuide")]
        ImplementationGuide,
        
        /// <summary>
        /// InsurancePlan
        /// </summary>
        [EnumMember(Value = "InsurancePlan")]
        InsurancePlan,
        
        /// <summary>
        /// Invoice
        /// </summary>
        [EnumMember(Value = "Invoice")]
        Invoice,
        
        /// <summary>
        /// Library
        /// </summary>
        [EnumMember(Value = "Library")]
        Library,
        
        /// <summary>
        /// Linkage
        /// </summary>
        [EnumMember(Value = "Linkage")]
        Linkage,
        
        /// <summary>
        /// List
        /// </summary>
        [EnumMember(Value = "List")]
        List,
        
        /// <summary>
        /// Location
        /// </summary>
        [EnumMember(Value = "Location")]
        Location,
        
        /// <summary>
        /// Measure
        /// </summary>
        [EnumMember(Value = "Measure")]
        Measure,
        
        /// <summary>
        /// MeasureReport
        /// </summary>
        [EnumMember(Value = "MeasureReport")]
        MeasureReport,
        
        /// <summary>
        /// Media
        /// </summary>
        [EnumMember(Value = "Media")]
        Media,
        
        /// <summary>
        /// Medication
        /// </summary>
        [EnumMember(Value = "Medication")]
        Medication,
        
        /// <summary>
        /// MedicationAdm
        /// </summary>
        [EnumMember(Value = "MedicationAdministra")]
        MedicationAdministration,
        
        /// <summary>
        /// MedicationDis
        /// </summary>
        [EnumMember(Value = "MedicationDispense")]
        MedicationDispense,
        
        /// <summary>
        /// MedicationKno
        /// </summary>
        [EnumMember(Value = "MedicationKnowledge")]
        MedicationKnowledge,
        
        /// <summary>
        /// MedicationReq
        /// </summary>
        [EnumMember(Value = "MedicationRequest")]
        MedicationRequest,
        
        /// <summary>
        /// MedicationSta
        /// </summary>
        [EnumMember(Value = "MedicationStatement")]
        MedicationStatement,
        
        /// <summary>
        /// MedicinalProd
        /// </summary>
        [EnumMember(Value = "MedicinalProduct")]
        MedicinalProduct,
        
        /// <summary>
        /// MedicinalProd
        /// </summary>
        [EnumMember(Value = "MedicinalProductAuth")]
        MedicinalProductAuthorization,
        
        /// <summary>
        /// MedicinalProd
        /// </summary>
        [EnumMember(Value = "MedicinalProductCont")]
        MedicinalProductContraindication,
        
        /// <summary>
        /// MedicinalProd
        /// </summary>
        [EnumMember(Value = "MedicinalProductIndi")]
        MedicinalProductIndication,
        
        /// <summary>
        /// MedicinalProd
        /// </summary>
        [EnumMember(Value = "MedicinalProductIngr")]
        MedicinalProductIngredient,
        
        /// <summary>
        /// MedicinalProd
        /// </summary>
        [EnumMember(Value = "MedicinalProductInte")]
        MedicinalProductInteraction,
        
        /// <summary>
        /// MedicinalProd
        /// </summary>
        [EnumMember(Value = "MedicinalProductManu")]
        MedicinalProductManufactured,
        
        /// <summary>
        /// MedicinalProd
        /// </summary>
        [EnumMember(Value = "MedicinalProductPack")]
        MedicinalProductPackaged,
        
        /// <summary>
        /// MedicinalProd
        /// </summary>
        [EnumMember(Value = "MedicinalProductPhar")]
        MedicinalProductPharmaceutical,
        
        /// <summary>
        /// MedicinalProd
        /// </summary>
        [EnumMember(Value = "MedicinalProductUnde")]
        MedicinalProductUndesirableEffect,
        
        /// <summary>
        /// MessageDefini
        /// </summary>
        [EnumMember(Value = "MessageDefinition")]
        MessageDefinition,
        
        /// <summary>
        /// MessageHeader
        /// </summary>
        [EnumMember(Value = "MessageHeader")]
        MessageHeader,
        
        /// <summary>
        /// MolecularSequ
        /// </summary>
        [EnumMember(Value = "MolecularSequence")]
        MolecularSequence,
        
        /// <summary>
        /// NamingSystem
        /// </summary>
        [EnumMember(Value = "NamingSystem")]
        NamingSystem,
        
        /// <summary>
        /// NutritionOrde
        /// </summary>
        [EnumMember(Value = "NutritionOrder")]
        NutritionOrder,
        
        /// <summary>
        /// Observation
        /// </summary>
        [EnumMember(Value = "Observation")]
        Observation,
        
        /// <summary>
        /// ObservationDe
        /// </summary>
        [EnumMember(Value = "ObservationDefinitio")]
        ObservationDefinition,
        
        /// <summary>
        /// OperationDefi
        /// </summary>
        [EnumMember(Value = "OperationDefinition")]
        OperationDefinition,
        
        /// <summary>
        /// OperationOutc
        /// </summary>
        [EnumMember(Value = "OperationOutcome")]
        OperationOutcome,
        
        /// <summary>
        /// Organization
        /// </summary>
        [EnumMember(Value = "Organization")]
        Organization,
        
        /// <summary>
        /// OrganizationA
        /// </summary>
        [EnumMember(Value = "OrganizationAffiliat")]
        OrganizationAffiliation,
        
        /// <summary>
        /// Parameters
        /// </summary>
        [EnumMember(Value = "Parameters")]
        Parameters,
        
        /// <summary>
        /// Patient
        /// </summary>
        [EnumMember(Value = "Patient")]
        Patient,
        
        /// <summary>
        /// PaymentNotice
        /// </summary>
        [EnumMember(Value = "PaymentNotice")]
        PaymentNotice,
        
        /// <summary>
        /// PaymentReconc
        /// </summary>
        [EnumMember(Value = "PaymentReconciliatio")]
        PaymentReconciliation,
        
        /// <summary>
        /// Person
        /// </summary>
        [EnumMember(Value = "Person")]
        Person,
        
        /// <summary>
        /// PlanDefinitio
        /// </summary>
        [EnumMember(Value = "PlanDefinition")]
        PlanDefinition,
        
        /// <summary>
        /// Practitioner
        /// </summary>
        [EnumMember(Value = "Practitioner")]
        Practitioner,
        
        /// <summary>
        /// PractitionerR
        /// </summary>
        [EnumMember(Value = "PractitionerRole")]
        PractitionerRole,
        
        /// <summary>
        /// Procedure
        /// </summary>
        [EnumMember(Value = "Procedure")]
        Procedure,
        
        /// <summary>
        /// Provenance
        /// </summary>
        [EnumMember(Value = "Provenance")]
        Provenance,
        
        /// <summary>
        /// Questionnaire
        /// </summary>
        [EnumMember(Value = "Questionnaire")]
        Questionnaire,
        
        /// <summary>
        /// Questionnaire
        /// </summary>
        [EnumMember(Value = "QuestionnaireRespons")]
        QuestionnaireResponse,
        
        /// <summary>
        /// RelatedPerson
        /// </summary>
        [EnumMember(Value = "RelatedPerson")]
        RelatedPerson,
        
        /// <summary>
        /// RequestGroup
        /// </summary>
        [EnumMember(Value = "RequestGroup")]
        RequestGroup,
        
        /// <summary>
        /// ResearchDefin
        /// </summary>
        [EnumMember(Value = "ResearchDefinition")]
        ResearchDefinition,
        
        /// <summary>
        /// ResearchEleme
        /// </summary>
        [EnumMember(Value = "ResearchElementDefin")]
        ResearchElementDefinition,
        
        /// <summary>
        /// ResearchStudy
        /// </summary>
        [EnumMember(Value = "ResearchStudy")]
        ResearchStudy,
        
        /// <summary>
        /// ResearchSubje
        /// </summary>
        [EnumMember(Value = "ResearchSubject")]
        ResearchSubject,
        
        /// <summary>
        /// RiskAssessmen
        /// </summary>
        [EnumMember(Value = "RiskAssessment")]
        RiskAssessment,
        
        /// <summary>
        /// RiskEvidenceS
        /// </summary>
        [EnumMember(Value = "RiskEvidenceSynthesi")]
        RiskEvidenceSynthesis,
        
        /// <summary>
        /// Schedule
        /// </summary>
        [EnumMember(Value = "Schedule")]
        Schedule,
        
        /// <summary>
        /// SearchParamet
        /// </summary>
        [EnumMember(Value = "SearchParameter")]
        SearchParameter,
        
        /// <summary>
        /// ServiceReques
        /// </summary>
        [EnumMember(Value = "ServiceRequest")]
        ServiceRequest,
        
        /// <summary>
        /// Slot
        /// </summary>
        [EnumMember(Value = "Slot")]
        Slot,
        
        /// <summary>
        /// Specimen
        /// </summary>
        [EnumMember(Value = "Specimen")]
        Specimen,
        
        /// <summary>
        /// SpecimenDefin
        /// </summary>
        [EnumMember(Value = "SpecimenDefinition")]
        SpecimenDefinition,
        
        /// <summary>
        /// StructureDefi
        /// </summary>
        [EnumMember(Value = "StructureDefinition")]
        StructureDefinition,
        
        /// <summary>
        /// StructureMap
        /// </summary>
        [EnumMember(Value = "StructureMap")]
        StructureMap,
        
        /// <summary>
        /// Subscription
        /// </summary>
        [EnumMember(Value = "Subscription")]
        Subscription,
        
        /// <summary>
        /// Substance
        /// </summary>
        [EnumMember(Value = "Substance")]
        Substance,
        
        /// <summary>
        /// SubstancePoly
        /// </summary>
        [EnumMember(Value = "SubstancePolymer")]
        SubstancePolymer,
        
        /// <summary>
        /// SubstanceRefe
        /// </summary>
        [EnumMember(Value = "SubstanceReferenceIn")]
        SubstanceReferenceInformation,
        
        /// <summary>
        /// SubstanceSpec
        /// </summary>
        [EnumMember(Value = "SubstanceSpecificati")]
        SubstanceSpecification,
        
        /// <summary>
        /// SupplyDeliver
        /// </summary>
        [EnumMember(Value = "SupplyDelivery")]
        SupplyDelivery,
        
        /// <summary>
        /// SupplyRequest
        /// </summary>
        [EnumMember(Value = "SupplyRequest")]
        SupplyRequest,
        
        /// <summary>
        /// Task
        /// </summary>
        [EnumMember(Value = "Task")]
        Task,
        
        /// <summary>
        /// TerminologyCa
        /// </summary>
        [EnumMember(Value = "TerminologyCapabilit")]
        TerminologyCapabilities,
        
        /// <summary>
        /// TestReport
        /// </summary>
        [EnumMember(Value = "TestReport")]
        TestReport,
        
        /// <summary>
        /// TestScript
        /// </summary>
        [EnumMember(Value = "TestScript")]
        TestScript,
        
        /// <summary>
        /// ValueSet
        /// </summary>
        [EnumMember(Value = "ValueSet")]
        ValueSet,
        
        /// <summary>
        /// VerificationR
        /// </summary>
        [EnumMember(Value = "VerificationResult")]
        VerificationResult,
        
        /// <summary>
        /// VisionPrescri
        /// </summary>
        [EnumMember(Value = "VisionPrescription")]
        VisionPrescription,
    }
}