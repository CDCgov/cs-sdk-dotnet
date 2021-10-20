namespace CS.Sdk.Batchers
{
    /// <summary>
    /// Class representing the outcome of attempting to handle a message debatch
    /// </summary>
    public class MessageHandleMetadata
    {
        /// <summary>
        /// The position in the batch that this message was found at. For example,
        /// the 5th HL7v2 message in an HL7v2 batch would have '5' for this value.
        /// </summary>
        public int MessageBatchPosition { get; set; } = 0;

        /// <summary>
        /// Error code associated with this metadata, if any
        /// </summary>
        public string ErrorCode { get; set; }

        /// <summary>
        /// The content of this message. Typically this will be a human-readable description of this metadata
        /// </summary>
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// The severity associated with this message
        /// </summary>
        public Severity Severity { get; set; } = Severity.Information;
    }
}
