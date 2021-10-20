namespace CS.Sdk.Services
{
    /// <summary>
    /// Class representing the outcome from validating a data element for vocabulary
    /// </summary>
    public sealed class VocabularyValidationResult
    {
        /// <summary>
        /// Gets/sets the concept code that was checked
        /// </summary>
        public string ConceptCode { get; set; } = string.Empty;

        /// <summary>
        /// Gets/sets the concept name that was checked
        /// </summary>
        public string ConceptName { get; set; } = string.Empty;

        /// <summary>
        /// Gets/sets the concept code system that was checked
        /// </summary>
        public string ConceptCodeSystem { get; set; } = string.Empty;

        /// <summary>
        /// Gets/sets whether the concept code is valid
        /// </summary>
        public bool IsCodeValid { get; private set; } = false;

        /// <summary>
        /// Gets/sets whether the description of the concept is valid
        /// </summary>
        public bool IsNameValid { get; private set; } = false;

        /// <summary>
        /// Gets/sets whether the code system is valid
        /// </summary>
        public bool IsSystemValid { get; private set; } = false;

        public VocabularyValidationResult(bool isCodeValid, bool isNameValid, bool isSystemValid)
        {
            IsCodeValid = isCodeValid;
            IsNameValid = isNameValid;
            IsSystemValid = isSystemValid;
        }
    }
}
