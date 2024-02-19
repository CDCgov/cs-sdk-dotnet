using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CS.Sdk.Batchers
{
    /// <summary>
    /// Debatches an HL7v2 batch message into a list of individual HL7v2 messages
    /// </summary>
    public sealed class HL7v2StreamingDebatcher
    {
        /// <summary>
        /// Debatch an HL7v2 batch message
        /// </summary>
        /// <param name="hl7v2batch">HL7v2 batch payload</param>
        /// <param name="debatchHandler">Object that can be injected and whose handler method is executed on every
        /// debatched HL7v2 message, as it is debatched. For a batch of 100,000 messages, this function will be called 
        /// 100,000 times.</param>
        /// <param name="transactionId">Optional transaction ID. Leaving this empty will result in a warning.</param>
        /// <returns>A debatch result containing metadata about the debatching operation</returns>
        public async Task<DebatchResult> DebatchAsync(Stream hl7v2batch, IDebatchHandler debatchHandler, string transactionId = "")
        {
            // time the debatching process
            var sw = new System.Diagnostics.Stopwatch();
            sw.Start();

            int messagesFound = 0;

            // create variables for storing batch metadata
            string senderApplication = string.Empty;
            string senderFacility = string.Empty;
            string receiverApplication = string.Empty;
            string receiverFacility = string.Empty;
            string batchName = string.Empty;
            string batchComments = string.Empty;
            int reportedBatchCount = -1;

            MessageDebatchMetadata messageMetadata = new MessageDebatchMetadata()
            {
                TransactionId = transactionId,
                Hash = ""
            };

            // create a container for error/warning/info messages
            List<ProcessResultMessage> processingMessages = new List<ProcessResultMessage>(1);

            // keep a list of any handler calls that failed
            List<MessageHandleMetadata> handlerMessages = new List<MessageHandleMetadata>(1);

            using (StreamReader streamReader = new StreamReader(hl7v2batch))
            {
                StringBuilder messageBuilder = new StringBuilder();

                while (!streamReader.EndOfStream)
                {
                    string line = await streamReader.ReadLineAsync();

                    if (line.Length < 3)
                    {
                        // Error
                    }
                    else if (line[0] == 'F' && line[1] == 'H' && line[2] == 'S')
                    {

                    }
                    else if (line[0] == 'B' && line[1] == 'H' && line[2] == 'S')
                    {
                        (senderApplication, senderFacility, receiverApplication, receiverFacility, batchName) = ExtractMetadataFromBHS(line);

                        messageMetadata.SenderApplication = senderApplication;
                        messageMetadata.SenderFacility = senderFacility;
                        messageMetadata.ReceiverApplication = receiverApplication;
                        messageMetadata.ReceiverFacility = receiverFacility;
                    }
                    else if (line[0] == 'B' && line[1] == 'T' && line[2] == 'S')
                    {
                        if (true /* TODO: add options check here */)
                        {
                            string[] fields = line.Split('|');

                            if (fields.Length >= 3)
                            {
                                batchComments = fields[2].Trim('\r').Trim('\n').Trim('\r');
                            }

                            if (fields.Length >= 2)
                            {
                                bool success = int.TryParse(fields[1], out reportedBatchCount);

                                if (!success)
                                {
                                    reportedBatchCount = -1;
                                    if (true /*options.CheckBatchTrailerCountAgainstActualCount*/)
                                    {
                                        ProcessResultMessage warning = new ProcessResultMessage()
                                        {
                                            ErrorCode = "0001",
                                            Severity = Severity.Warning,
                                            Content = "Batch trailer segment has missing or invalid data for the Batch Message Count field (BTS-1)"
                                        };
                                        processingMessages.Add(warning);
                                    }
                                }
                            }
                        }
                    }
                    else if (line[0] == 'F' && line[1] == 'T' && line[2] == 'S')
                    {

                    }
                    else if (line[0] == 'M' && line[1] == 'S' && line[2] == 'H')
                    {
                        if (messagesFound > 0)
                        {
                            WriteMessage(messageMetadata, messagesFound, debatchHandler, messageBuilder, handlerMessages, sw);
                        }
                        messagesFound++;
                        messageBuilder.Clear();
                        messageBuilder.Append(line + '\n');
                    }
                    else
                    {
                        messageBuilder.Append(line + '\n');
                    }
                }

                // last message TODO: DRY
                if (messagesFound > 0)
                {
                    WriteMessage(messageMetadata, messagesFound, debatchHandler, messageBuilder, handlerMessages, sw);
                }
            }

            sw.Stop();

            DebatchResult result = new DebatchResult(processingMessages, handlerMessages)
            {
                BatchName = batchName,
                BatchComments = batchComments,
                SenderApplication = senderApplication,
                SenderFacility = senderFacility,
                ReceiverApplication = receiverApplication,
                ReceiverFacility = receiverFacility,
                TransactionId = transactionId,
                ActualMessageCount = messagesFound,
                ReportedMessageCount = reportedBatchCount,
                Hash = "",
                Elapsed = sw.Elapsed.TotalMilliseconds
            };

            return result;
        }

        private void WriteMessage(MessageDebatchMetadata messageMetadata, int messagesFound, IDebatchHandler debatchHandler, StringBuilder messageBuilder, List<MessageHandleMetadata> handlerMessages, Stopwatch sw)
        {
            messageMetadata.MessageBatchPosition = messagesFound;
            messageMetadata.MessageDebatchedTimestamp = DateTime.Now;
            messageMetadata.Elapsed = sw.Elapsed.TotalMilliseconds;

            if (debatchHandler != null)
            {
                string message = messageBuilder.ToString();
                MessageHandleMetadata metadata = Callback(message.AsSpan(), debatchHandler, messageMetadata);
                if (metadata != null && (metadata.Severity == Severity.Error || metadata.Severity == Severity.Warning))
                {
                    handlerMessages.Add(metadata);
                }
            }
        }

        /// <summary>
        /// Wrapper for the callback method (if one has been supplied) that gets executed each time a message has been debatched
        /// </summary>
        /// <param name="hl7v2message">HL7v2 message that was debatched</param>
        /// <param name="metadata">Metadata about how the message was debatched and what batch from which it was derived</param>
        private MessageHandleMetadata Callback(ReadOnlySpan<char> hl7v2message, IDebatchHandler debatchHandler, MessageDebatchMetadata metadata)
        {
            hl7v2message = hl7v2message.TrimEnd('\n').TrimEnd('\r'); // trim any newlines from the message
                                                                     //_callback?.Invoke(message, metadata); // if the callback function pointer isn't null, invoke it
            if (debatchHandler != null)
            {
                MessageHandleMetadata result = debatchHandler.HandleDebatch(hl7v2message, metadata);
                return result;
            }
            return default(MessageHandleMetadata);
        }

        /// <summary>
        /// Extract metadata from the BHS segment
        /// </summary>
        /// <param name="bhsSegment">Batch header segment</param>
        /// <returns>Metadata in a standard C# tuple</returns>
        private (string SenderApplication, string SenderFacility, string ReceiverApplication, string ReceiverFacility, string BatchName) ExtractMetadataFromBHS(string bhsSegment)
        {
            char fieldSeparator = bhsSegment.Length >= 5 ? bhsSegment[3] : '|';

            string[] bhsFields = bhsSegment.Split(fieldSeparator);

            if (bhsFields.Length > 4)
            {
                string senderApplication = bhsFields[2];
                string senderFacility = bhsFields[3];
                string receiverApplication = bhsFields[4];
                string receiverFacility = bhsFields[5];

                if (bhsFields.Length >= 9)
                {
                    string batchName = bhsFields[8];
                    return (SenderApplication: senderApplication, SenderFacility: senderFacility, ReceiverApplication: receiverApplication, ReceiverFacility: receiverFacility, BatchName: batchName);
                }
                else
                {
                    return (SenderApplication: senderApplication, SenderFacility: senderFacility, ReceiverApplication: receiverApplication, ReceiverFacility: receiverFacility, BatchName: string.Empty);
                }
            }
            else
            {
                return (SenderApplication: string.Empty, SenderFacility: string.Empty, ReceiverApplication: string.Empty, ReceiverFacility: string.Empty, BatchName: string.Empty);
            }
        }
    }
}