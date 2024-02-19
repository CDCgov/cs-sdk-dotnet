using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace CS.Sdk.Batchers
{
    /// <summary>
    /// Debatches an HL7v2 batch message into a list of individual HL7v2 messages
    /// </summary>
    public sealed class HL7v2Debatcher
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
        public DebatchResult Debatch(string hl7v2batch, IDebatchHandler debatchHandler, string transactionId = "")
        {
            DebatchOptions options = new DebatchOptions(
                doPreProcessSanityChecks: true,
                checkFileSegments: true,
                checkBatchSegments: true,
                checkBatchTrailerCountAgainstActualCount: true,
                checkBatchForEmptiness: true
            );

            return Debatch(hl7v2batch, options, debatchHandler, transactionId);
        }

        /// <summary>
        /// Debatch an HL7v2 batch message
        /// </summary>
        /// <param name="hl7v2batch">HL7v2 batch payload</param>
        /// <paramref name="options">Options object for configuring behavior of the debatcher</param>
        /// <param name="debatchHandler">Object that can be injected and whose handler method is executed on every
        /// debatched HL7v2 message, as it is debatched. For a batch of 100,000 messages, this function will be called 
        /// 100,000 times.</param>
        /// <param name="transactionId">Optional transaction ID. Leaving this empty will result in a warning.</param>
        /// <returns>A debatch result containing metadata about the debatching operation</returns>
        public DebatchResult Debatch(string hl7v2batch, DebatchOptions options, IDebatchHandler debatchHandler, string transactionId = "")
        {
            // time the debatching process
            var sw = new System.Diagnostics.Stopwatch();
            sw.Start();

            // hash the batch
            string hash = string.Empty;
            byte[] data = Encoding.UTF8.GetBytes(hl7v2batch);
            using (var alg = SHA512.Create())
            {
                alg.ComputeHash(data);
                hash = BitConverter.ToString(alg.Hash);
            }

            // create a container for error/warning/info messages
            List<ProcessResultMessage> processingMessages = new List<ProcessResultMessage>(1);

            // keep a list of any handler calls that failed
            List<MessageHandleMetadata> handlerMessages = new List<MessageHandleMetadata>(1);

            // get a stack-allocated "view" of the string
            ReadOnlySpan<char> message = hl7v2batch.AsSpan();

            // do some sanity checks
            if (options.DoPreProcessSanityChecks)
            {
                processingMessages.AddRange(DoPreProcessSanityChecks(message));
            }
            if (processingMessages.Any(m => m.Severity == Severity.Error))
            {
                return new DebatchResult(processingMessages, handlerMessages)
                {
                    TransactionId = transactionId,
                    Hash = hash
                };
            }

            // we need to check to make sure that the batch segments exist in correct order, meaning we should see FHS->BHS->[normal HL7v2 segments]->BTS->FTS. Let's set up a data
            // structure to keep track of what segments we encounter so we can verify later that this is what we expected. The contents of this List<string> ought to be FHS, BHS,
            // BTS, and FTS. If any of them are missing then there's a structural problem with the batch.
            List<string> segmentCheckerList = new List<string>();

            int offset = 0;
            bool collecting = false;
            int startCollectionOffset = -1;
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
                Hash = hash
            };

            // iterate over every character in the message looking for newlines, which would indiciate we just finished
            // iterating over a segment. Every new segment is a decision point, potentially. If the new segment is an MSH
            // segment, for example, then we want to take the last set of segments prior to the current location in the
            // iteration and treat it as an HL7v2 message.
            for (int i = 0; i <= message.Length; i++)
            {
                // get the current character
                ReadOnlySpan<char> currentCharacter = i == message.Length ? "\n".AsSpan() : message.Slice(i, 1);

                // if we're on a newline, then we may need to make a decision 
                if (currentCharacter[0] == '\n' || currentCharacter[0] == '\r')
                {
                    // what's the length of the segment?
                    int segmentLength = (i - offset);

                    // only proceed if the segment has stuff in it
                    if (segmentLength > 3)
                    {
                        // we've detected a new line, this is a segment, and there's stuff in the segment. So let's go...

                        // Get the type of segment that this is, e.g. MSH, BHS, PID, OBR...
                        ReadOnlySpan<char> segmentType = message.Slice(offset, 3);

                        // file header segment should be the first thing in the batch
                        if (segmentType[0] == 'F' && segmentType[1] == 'H' && segmentType[2] == 'S')
                        {
                            segmentCheckerList.Add("FHS");
                            collecting = false;
                            startCollectionOffset = offset;
                        }
                        // batch header segment is right after FHS segment
                        else if (segmentType[0] == 'B' && segmentType[1] == 'H' && segmentType[2] == 'S')
                        {
                            segmentCheckerList.Add("BHS");

                            // validate FHS segment
                            if (options.CheckFileSegments)
                            {
                                ReadOnlySpan<char> fhsSegment = message.Slice(startCollectionOffset, offset - startCollectionOffset);
                                processingMessages.AddRange(ValidateFHS(fhsSegment));
                            }

                            collecting = false;
                            startCollectionOffset = offset;
                        }
                        // message header segment - each one of these denotes a brand new HL7v2 message within the batch, so handle specially
                        else if (segmentType[0] == 'M' && segmentType[1] == 'S' && segmentType[2] == 'H')
                        {
                            if (collecting)
                            {
                                // since we detected that the cursor is just starting to process a new MSH segment, it's time to write
                                // all the stuff from the last detected MSH segment up to the current point
                                ReadOnlySpan<char> messageSpan = message.Slice(startCollectionOffset, offset - startCollectionOffset);
                                messagesFound++;

                                messageMetadata.MessageBatchPosition = messagesFound;
                                messageMetadata.MessageDebatchedTimestamp = DateTime.Now;
                                messageMetadata.Elapsed = sw.Elapsed.TotalMilliseconds;

                                if (debatchHandler != null)
                                {
                                    MessageHandleMetadata metadata = Callback(messageSpan, debatchHandler, messageMetadata);
                                    if (metadata != null && (metadata.Severity == Severity.Error || metadata.Severity == Severity.Warning))
                                    {
                                        handlerMessages.Add(metadata);
                                    }
                                }
                            }
                            else
                            {
                                // this is the first MSH, so let's process the preceding BHS segment because the BHS segment
                                // has important batch metadata
                                if (options.CheckBatchSegments)
                                {
                                    ReadOnlySpan<char> bhsSegment = message.Slice(startCollectionOffset, offset - startCollectionOffset);
                                    processingMessages.AddRange(ValidateBHS(bhsSegment));

                                    try
                                    {
                                        (senderApplication, senderFacility, receiverApplication, receiverFacility, batchName) = ExtractMetadataFromBHS(bhsSegment);

                                        messageMetadata.SenderApplication = senderApplication;
                                        messageMetadata.SenderFacility = senderFacility;
                                        messageMetadata.ReceiverApplication = receiverApplication;
                                        messageMetadata.ReceiverFacility = receiverFacility;
                                    }
                                    catch (Exception ex)
                                    {
                                        if (options.CheckBatchSegments)
                                        {
                                            ProcessResultMessage warning = new ProcessResultMessage()
                                            {
                                                ErrorCode = "0011",
                                                Severity = Severity.Warning,
                                                Content = "Batch header segment is malformed and could not be parsed. Sender application, sender facility, receiver application, and receiver facility metadata could not be retrieved. Message: " + ex.Message
                                            };
                                            processingMessages.Add(warning);
                                        }
                                    }
                                }
                            }

                            collecting = true;
                            startCollectionOffset = offset;
                        }
                        // batch trailer segment
                        else if (segmentType[0] == 'B' && segmentType[1] == 'T' && segmentType[2] == 'S')
                        {
                            segmentCheckerList.Add("BTS");

                            if (collecting)
                            {
                                // write everything up to this point
                                ReadOnlySpan<char> messageSpan = message.Slice(startCollectionOffset, offset - startCollectionOffset);
                                messagesFound++;

                                messageMetadata.MessageBatchPosition = messagesFound;
                                messageMetadata.MessageDebatchedTimestamp = DateTime.Now;
                                messageMetadata.Elapsed = sw.Elapsed.TotalMilliseconds;

                                if (debatchHandler != null)
                                {
                                    MessageHandleMetadata metadata = Callback(messageSpan, debatchHandler, messageMetadata);
                                    if (metadata != null && (metadata.Severity == Severity.Error || metadata.Severity == Severity.Warning))
                                    {
                                        handlerMessages.Add(metadata);
                                    }
                                }
                            }

                            collecting = false;
                            startCollectionOffset = offset;
                        }
                        // file trailer segment
                        else if (segmentType[0] == 'F' && segmentType[1] == 'T' && segmentType[2] == 'S')
                        {
                            segmentCheckerList.Add("FTS");

                            if (collecting)
                            {
                                // we should never get here unless BTS segment was missing ... so write everything up to this point
                                ReadOnlySpan<char> messageSpan = message.Slice(startCollectionOffset, offset - startCollectionOffset);
                                messagesFound++;

                                messageMetadata.MessageBatchPosition = messagesFound;
                                messageMetadata.MessageDebatchedTimestamp = DateTime.Now;
                                messageMetadata.Elapsed = sw.Elapsed.TotalMilliseconds;

                                if (debatchHandler != null)
                                {
                                    MessageHandleMetadata metadata = Callback(messageSpan, debatchHandler, messageMetadata);
                                    if (metadata != null && (metadata.Severity == Severity.Error || metadata.Severity == Severity.Warning))
                                    {
                                        handlerMessages.Add(metadata);
                                    }
                                }
                            }

                            collecting = false;

                            // get batch commands from BTS
                            ReadOnlySpan<char> btsSpan = message.Slice(startCollectionOffset, offset - startCollectionOffset);
                            string[] fields = btsSpan.ToString().Split('|');

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
                                    if (options.CheckBatchTrailerCountAgainstActualCount)
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
                        else
                        {
                            if (segmentCheckerList.Count != 2 || !segmentCheckerList[1].Equals("BHS", StringComparison.OrdinalIgnoreCase))
                            {
                                segmentCheckerList.Add("000");
                            }
                        }
                    }

                    // do one last check for 'collecting' if we're on a newline? e.g. if BTS and FTS are both missing?

                    offset = i + 1;
                }
            }

            #region Check for errors/warnings
            if (options.CheckFileSegments && segmentCheckerList.Count >= 1 && segmentCheckerList[0] != "FHS")
            {
                // error
                ProcessResultMessage processingMessage = new ProcessResultMessage()
                {
                    ErrorCode = "0003",
                    Severity = Severity.Error,
                    Content = "Batch does not start with the required FHS segment"
                };
                processingMessages.Add(processingMessage);
            }
            if (options.CheckBatchSegments && segmentCheckerList.Count >= 2 && segmentCheckerList[1] != "BHS")
            {
                // error
                ProcessResultMessage processingMessage = new ProcessResultMessage()
                {
                    ErrorCode = "0004",
                    Severity = Severity.Error,
                    Content = "The required BHS segment is missing"
                };
                processingMessages.Add(processingMessage);
            }
            if (options.CheckBatchSegments && segmentCheckerList.Count >= 3 && segmentCheckerList[2] != "BTS")
            {
                // error
                ProcessResultMessage processingMessage = new ProcessResultMessage()
                {
                    ErrorCode = "0006",
                    Severity = Severity.Error,
                    Content = "The required BTS segment is missing"
                };
                processingMessages.Add(processingMessage);
            }
            if (options.CheckFileSegments && segmentCheckerList.Count >= 4 && segmentCheckerList[3] != "FTS")
            {
                // error
                ProcessResultMessage processingMessage = new ProcessResultMessage()
                {
                    ErrorCode = "0005",
                    Severity = Severity.Error,
                    Content = "The required FTS segment is missing"
                };
                processingMessages.Add(processingMessage);
            }
            // the actual # of messages debatched is not the same as the number the sender said they included
            if (options.CheckBatchTrailerCountAgainstActualCount && messagesFound != reportedBatchCount)
            {
                ProcessResultMessage warning = new ProcessResultMessage()
                {
                    ErrorCode = "0002",
                    Severity = Severity.Warning,
                    Content = "Batch trailer segment reports a count of " + reportedBatchCount + " but the actual number of messages retrieved from the batch is " + messagesFound
                };
                processingMessages.Add(warning);
            }
            // no messages were found in the batch!
            if (options.CheckBatchForEmptiness && messagesFound == 0)
            {
                ProcessResultMessage error = new ProcessResultMessage()
                {
                    ErrorCode = "0007",
                    Severity = Severity.Error,
                    Content = "No messages are present in the batch"
                };
                processingMessages.Add(error);
            }
            #endregion // Check for errors/warnings

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
                Hash = hash,
                Elapsed = sw.Elapsed.TotalMilliseconds
            };

            return result;
        }

        /// <summary>
        /// Executes basic sanity checks on the HL7v2 batch *before* processing starts
        /// </summary>
        /// <param name="hl7v2batch">HL7v2 batch payload</param>
        /// <returns>List of errors and warnings found during the preprocessing check</returns>
        private List<ProcessResultMessage> DoPreProcessSanityChecks(ReadOnlySpan<char> hl7v2batch)
        {
            List<ProcessResultMessage> processingMessages = new List<ProcessResultMessage>(1);


            ReadOnlySpan<char> firstThree = hl7v2batch.Slice(0, 3);

            // check that first three characters are FHS (file header segment)
            if (!firstThree.SequenceEqual("FHS".AsSpan()))
            {
                ProcessResultMessage message = new ProcessResultMessage()
                {
                    ErrorCode = "0003",
                    Severity = Severity.Error,
                    Content = "Batch does not start with the required FHS segment"
                };
                processingMessages.Add(message);
            }

            return processingMessages;
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
        /// Validates the batch header segment (BHS)
        /// </summary>
        /// <param name="bhsSegment">Batch header segment</param>
        /// <returns>List of errors and warnings found during the BHS validation check</returns>
        private List<ProcessResultMessage> ValidateBHS(ReadOnlySpan<char> bhsSegment)
        {
            var errorMessages = new List<ProcessResultMessage>();

            // validate BHS (batch header) segment
            if (bhsSegment.Length >= 8)
            {
                ReadOnlySpan<char> bhsEncodingCharacters = bhsSegment.Slice(3, 5);

                if (bhsEncodingCharacters[0] != '|' && bhsEncodingCharacters[1] != '^' && bhsEncodingCharacters[2] != '~' && bhsEncodingCharacters[3] != '/' && bhsEncodingCharacters[4] != '&')
                {
                    ProcessResultMessage errorMessage = new ProcessResultMessage()
                    {
                        ErrorCode = "0003",
                        Severity = Severity.Error,
                        Content = @"BHS segment does not contain the required encoding characters. Expected: |^~\&"
                    };
                    errorMessages.Add(errorMessage);
                }
            }
            else
            {
                ProcessResultMessage errorMessage = new ProcessResultMessage()
                {
                    ErrorCode = "0003",
                    Severity = Severity.Error,
                    Content = @"BHS segment is malformed"
                };
                errorMessages.Add(errorMessage);
            }

            return errorMessages;
        }

        /// <summary>
        /// Validates the file header segment (FHS)
        /// </summary>
        /// <param name="fhsSegment">File header segment</param>
        /// <returns>List of errors and warnings found during the FHS validation check</returns>
        private List<ProcessResultMessage> ValidateFHS(ReadOnlySpan<char> fhsSegment)
        {
            var errorMessages = new List<ProcessResultMessage>();

            // validate FHS (file header) segment
            if (fhsSegment.Length >= 8)
            {
                ReadOnlySpan<char> fhsEncodingCharacters = fhsSegment.Slice(3, 5);

                if (fhsEncodingCharacters[0] != '|' && fhsEncodingCharacters[1] != '^' && fhsEncodingCharacters[2] != '~' && fhsEncodingCharacters[3] != '/' && fhsEncodingCharacters[4] != '&')
                {
                    ProcessResultMessage errorMessage = new ProcessResultMessage()
                    {
                        ErrorCode = "0003",
                        Severity = Severity.Error,
                        Content = @"FHS segment does not contain the required encoding characters. Expected: |^~\&"
                    };
                    errorMessages.Add(errorMessage);
                }
            }
            else
            {
                ProcessResultMessage errorMessage = new ProcessResultMessage()
                {
                    ErrorCode = "0003",
                    Severity = Severity.Error,
                    Content = @"FHS segment is malformed"
                };
                errorMessages.Add(errorMessage);
            }

            return errorMessages;
        }

        /// <summary>
        /// Extract metadata from the BHS segment
        /// </summary>
        /// <param name="bhsSegment">Batch header segment</param>
        /// <returns>Metadata in a standard C# tuple</returns>
        private (string SenderApplication, string SenderFacility, string ReceiverApplication, string ReceiverFacility, string BatchName) ExtractMetadataFromBHS(ReadOnlySpan<char> bhsSegment)
        {
            char fieldSeparator = bhsSegment.Length >= 5 ? bhsSegment[3] : '|';

            string[] bhsFields = bhsSegment.ToString().Split(fieldSeparator);

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
