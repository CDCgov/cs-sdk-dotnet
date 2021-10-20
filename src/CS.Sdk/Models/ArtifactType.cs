namespace CS.Mmg
{
    /// <summary>
    /// These are the types of artifacts from exporting an MMG
    /// </summary>
    public enum ArtifactType
    {
        /// <summary>
        /// This is used for the Test Case Scenario Worksheet
        /// </summary>
        TestCaseScenarioWorksheet,

        /// <summary>
        /// This is the standard Message Mapping Guide artifact with Test Scenarios included
        /// </summary>
        StandardIncludeTestScenarios,

        /// <summary>
        /// This is the standard Message Mapping Guide artifact
        /// </summary>
        Standard,
    }
}