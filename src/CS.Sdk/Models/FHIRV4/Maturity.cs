namespace CS.Mmg.FHIRV4
{
    /// <summary>
    /// Maturity Level
    /// </summary>
    public enum Maturity
    {
        /// <summary>
        /// the resource or profile (artifact) has been published on the current build. This level is synonymous with Draft
        /// </summary>
        Draft = '0',

        /// <summary>
        /// the artifact produces no warnings during the build process and the responsible WG has indicated that they consider
        /// the artifact substantially complete and ready for implementation. For resources, profiles and implementation guides,
        /// the FHIR Management Group has approved the underlying resource/profile/IG proposal
        /// </summary>
        FMM1 = '1',

        /// <summary>
        /// the artifact has been tested and successfully supports interoperability among at least three independently developed
        /// systems leveraging most of the scope (e.g. at least 80% of the core data elements) using semi-realistic data and scenarios
        /// based on at least one of the declared scopes of the artifact (e.g. at a connectathon). These interoperability results must
        /// have been reported to and accepted by the FMG
        /// </summary>
        FMM2 = '2',

        /// <summary>
        /// the artifact has been verified by the work group as meeting the Conformance Resource Quality Guidelines;
        /// has been subject to a round of formal balloting; has at least 10 distinct implementer comments recorded in
        /// the tracker drawn from at least 3 organizations resulting in at least one substantive change
        /// </summary>
        FMM3 = '3',

        /// <summary>
        /// the artifact has been tested across its scope (see below), published in a formal publication (e.g. Trial-Use),
        /// and implemented in multiple prototype projects. As well, the responsible work group agrees the artifact is
        /// sufficiently stable to require implementer consultation for subsequent non-backward compatible changes
        /// </summary>
        FMM4 = '4',

        /// <summary>
        /// the artifact has been published in two formal publication release cycles at FMM1+
        /// (i.e. Trial-Use level) and has been implemented in at least 5 independent production systems in more than one country
        /// </summary>
        FMM5 = '5',

        /// <summary>
        /// the artifact is now considered stable
        /// </summary>
        Normative = 'N',
    }
}