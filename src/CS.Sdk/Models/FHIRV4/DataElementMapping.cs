namespace CS.Mmg.FHIRV4
{
    /// <summary>
    /// MMG Data Element Mapping for FHIR Interface
    /// </summary>
    public sealed class DataElementMapping
    {
        /// <summary>
        /// Resource Type
        /// </summary>
        public string ResourceType { get; set; }

        /// <summary>
        /// Identifier for observation or other resource
        /// </summary>
        public string Identifier { get; set; }

        /// <summary>
        /// A JSON Path representation of the value in the schema based on the resource
        /// </summary>
        public string Path { get; set; }
    }
}