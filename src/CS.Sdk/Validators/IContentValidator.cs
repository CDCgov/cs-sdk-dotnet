namespace CS.Sdk.Validators
{
    /// <summary>
    /// Interface for validating case notifications
    /// </summary>
    public interface IContentValidator
    {
        /// <summary>
        /// Validates a case notification message
        /// </summary>
        /// <param name="message">The message to validate</param>
        /// <param name="transactionId">An optional transaction ID that can be used for tracking data lineage across stages of a data processing pipeline</param>
        /// <returns>ValidationResult; an object with various properties that explains whether the message is valid or invalid and contains validation metadata</returns>
        ValidationResult Validate(string message, string transactionId = "");
    }
}
