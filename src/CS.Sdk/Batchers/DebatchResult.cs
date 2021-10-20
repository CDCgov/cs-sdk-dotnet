using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CS.Sdk.Batchers
{
    /// <summary>
    /// Metadata for the outcome of a message debatching operation
    /// </summary>
    [DebuggerDisplay("HL7v2 Debatch Result for '{BatchName}' : {ActualMessageCount} actual, {ReportedMessageCount} reported, {Elapsed} ms")]
    public sealed class DebatchResult
    {
        /// <summary>
        /// Unique ID for this debatch result object
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// The transaction Id for the data processing job for which this debatching outcome was a part
        /// </summary>
        public string TransactionId { get; set; } = string.Empty;

        /// <summary>
        /// Timestamp for when the debatching operation completed
        /// </summary>
        public DateTimeOffset DebatchedTimestamp { get; set; } = DateTimeOffset.Now;

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
        /// The number of messages that the batch trailer claimed were in the batch
        /// </summary>
        public int ReportedMessageCount { get; set; } = 0;

        /// <summary>
        /// The number of actual messagess found in this batch
        /// </summary>
        public int ActualMessageCount { get; set; } = 0;

        /// <summary>
        /// The name of the batch
        /// </summary>
        public string BatchName { get; set; } = string.Empty;

        /// <summary>
        /// Any comments about the batch
        /// </summary>
        public string BatchComments { get; set; } = string.Empty;

        /// <summary>
        /// Time in milliseconds that the debatching operation took to complete
        /// </summary>
        public double Elapsed { get; set; } = 0.0;

        /// <summary>
        /// List of messages about specific errors and warnings that were encoutered during the debatch operation
        /// </summary>
        public List<ProcessResultMessage> ProcessResultMessages { get; private set; }

        /// <summary>
        /// List of messages about specific errors and warnings that were encoutered while handling messages after they were debatched
        /// </summary>
        public List<MessageHandleMetadata> HandlerMessages { get; private set; }

        /// <summary>
        /// Hash of the raw HL7v2 batch
        /// </summary>
        public string Hash { get; set; } = string.Empty;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="processMessages">List of messages about specific errors and warnings that were encoutered during the debatch operation</param>
        public DebatchResult(List<ProcessResultMessage> processMessages, List<MessageHandleMetadata> handlerMessages)
        {
            ProcessResultMessages = processMessages;
            HandlerMessages = handlerMessages;
        }
    }
}

