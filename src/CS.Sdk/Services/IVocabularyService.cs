namespace CS.Sdk.Services
{
    /// <summary>
    /// Interface for interacting with a vocabulary (terminology) service that can be
    /// used to aid in vocabulary content validation.
    /// </summary>
    public interface IVocabularyService
    {
        /// <summary>
        /// Determines if a given concept is valid, using a specified value set. That is, the service should be queried to determine if the
        /// concept exists in the value set, and if it does not, then a validation failure will be returned.
        /// </summary>
        /// <param name="conceptCode">The concept code to check</param>
        /// <param name="conceptName">The concept name to check</param>
        /// <param name="conceptCodeSystem">The code system of the concept</param>
        /// <param name="valueSetCode">The value set to check against</param>
        /// <returns>VocabularyValidationResult</returns>
        VocabularyValidationResult IsValid(string conceptCode, string conceptName, string conceptCodeSystem, string valueSetCode);
    }
}
