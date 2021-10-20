using System;

namespace CS.Sdk.Batchers
{
    /// <summary>
    /// Class representing metadata for a message that was debatched from an HL7v2 batch
    /// </summary>
    public sealed class MessageDebatchMetadata
    {
        /// <summary>
        /// The transaction Id from the debatch operation
        /// </summary>
        public string TransactionId { get; set; } = string.Empty;

        /// <summary>
        /// Timestamp of when this message was debatched
        /// </summary>
        public DateTimeOffset MessageDebatchedTimestamp { get; set; } = DateTimeOffset.Now;

        /// <summary>
        /// The sending application, extracted from the batch header
        /// </summary>
        public string SenderApplication { get; set; } = string.Empty;

        /// <summary>
        /// The sending facility, extracted from the batch header
        /// </summary>
        public string SenderFacility { get; set; } = string.Empty;

        /// <summary>
        /// The receiving application, extracted from the batch header
        /// </summary>
        public string ReceiverApplication { get; set; } = string.Empty;

        /// <summary>
        /// The receiving facility, extracted from the batch header
        /// </summary>
        public string ReceiverFacility { get; set; } = string.Empty;

        /// <summary>
        /// The position in the batch that this message was found at. For example,
        /// the 5th HL7v2 message in an HL7v2 batch would have '5' for this value.
        /// </summary>
        public int MessageBatchPosition { get; set; } = 0;

        /// <summary>
        /// The name of the batch
        /// </summary>
        public string BatchName { get; set; } = string.Empty;

        /// <summary>
        /// Time in milliseconds before the debatcher reached this message
        /// </summary>
        public double Elapsed { get; set; } = 0.0;

        /// <summary>
        /// The batch's Hash
        /// </summary>
        public string Hash { get; set; } = string.Empty;
    }
}

