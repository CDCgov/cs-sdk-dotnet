using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace CS.Mmg.FHIRV4
{
    /// <summary>
    /// Development Status
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum DevelopmentStatus
    {
        /// <summary>
        /// This portion of the specification is not considered to be complete enough or sufficiently
        /// reviewed to be safe for implementation. It may have known issues or still be in the "in development" stage.
        /// It is included in the publication as a place-holder, to solicit feedback from the implementation community
        /// and/orto give implementers some insight as to functionality likely to be included in future versions of the
        /// specification. Content at this level should only be implemented by the brave or desperate and is very much
        /// "use at your own risk". The content that is Draft that will usually be elevated to Trial Use once review and
        /// correction is complete after it has been subjected to ballot
        /// </summary>
        [EnumMember(Value = "Draft")]
        Draft,

        /// <summary>
        /// This content has been well reviewed and is considered by the authors to be ready for use in production systems.
        /// It has been subjected to ballot and approved as an official standard. However, it has not yet seen widespread
        /// use in production across the full spectrum of environments it is intended to be used in. In some cases, there
        /// may be documented known issues that require implementation experience to determine appropriate resolutions for.
        /// Future versions of FHIR may make significant changes to Trial Use content that are not compatible with previously
        /// published content.
        /// </summary>
        [EnumMember(Value = "TrialUse")]
        TrialUse,

        /// <summary>
        /// This content has been subject to review and production implementation in a wide variety of environments.
        /// The content is considered to be stable and has been 'locked', subjecting it to FHIR Inter-version Compatibility Rules.
        /// While changes are possible, they are expected to be infrequent and are tightly constrained.
        /// </summary>
        [EnumMember(Value = "Normative")]
        Normative,

        /// <summary>
        /// This portion of the specification is provided for implementer assistance and does not make rules that implementers are
        /// required to follow. Typical examples of this content in the FHIR specification are tables of contents, registries, examples,
        /// and implementer advice
        /// </summary>
        [EnumMember(Value = "Informative")]
        Informative,

        /// <summary>
        /// This portion of the specification is outdated and may be withdrawn in a future version. Implementers who already support
        /// it should continue to do so for backward compatibility. Implementers should avoid adding new uses of this portion of the
        /// specification. The specification should include guidance on what implementers should use instead of the deprecated portion
        /// </summary>
        [EnumMember(Value = "Deprecated")]
        Deprecated,
    }
}