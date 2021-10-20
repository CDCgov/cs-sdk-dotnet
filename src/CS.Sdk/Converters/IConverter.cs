namespace CS.Sdk.Converters
{
    /// <summary>
    /// Interface for converting case notifications from one data type to another. For example, a conversion from 
    /// HL7v2 to Json.
    /// </summary>
    public interface IConverter
    {
        /// <summary>
        /// Converts a case notification message from one format into another format
        /// </summary>
        /// <param name="message">The message to convert</param>
        /// <param name="transactionId">Optional transaction ID. Leaving this empty will result in a warning.</param>
        /// <returns>A conversion result containing the resulting Json and some metadata about the conversion</returns>
        ConversionResult Convert(string message, string transactionId = "");
    }
}
