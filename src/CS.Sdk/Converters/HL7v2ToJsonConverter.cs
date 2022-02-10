using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using CS.Mmg;
using CS.Sdk.Services;

namespace CS.Sdk.Converters
{
    /// <summary>
    /// Class for converting HL7v2 pipe-delimited ORU_R01 messages into a Json structure defined by a message mapping guide
    /// </summary>
    public sealed class HL7v2ToJsonConverter : IConverter
    {
        private static JsonWriterOptions _jsonWriterOptions = new JsonWriterOptions
        {
            Indented = true
        };

        private const string CODE_SUFFIX = "__code";
        private const string CODE_SYSTEM_SUFFIX = "__code_system";
        private const string ALT_SUFFIX = "__alt";
        private const string SOURCE_FORMAT = "HL7v2";

        private readonly IMmgService _mmgService = new InMemoryMmgService();

        public HL7v2ToJsonConverter()
        {   
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mmgService">Service for retrieving message mapping guides (MMGs)</param>
        public HL7v2ToJsonConverter(IMmgService mmgService)
        {
            _mmgService = mmgService;
        }

        /// <summary>
        /// Converts an HL7v2 pipe-delimited ORU_R01 message into a Json structure defined by a message mapping guide.
        /// </summary>
        /// <param name="hl7v2message">The HL7v2 message to transform</param>
        /// <param name="transactionId">Optional transaction ID. Leaving this empty will result in a warning.</param>
        /// <returns>A conversion result containing the resulting Json and some metadata about the conversion</returns>
        public ConversionResult Convert(string hl7v2message, string transactionId = "")
        {
            if (string.IsNullOrWhiteSpace(hl7v2message))
            {
                return new ConversionResult()
                {
                    ConversionMessages = new List<ProcessResultMessage>()
                    {
                        new ProcessResultMessage()
                        {
                            Severity = Severity.Error,
                            Content = "HL7v2 message is empty or whitespace",
                            ErrorCode = "0002"
                        }
                    },
                    TransactionId = transactionId,
                    Created = DateTime.Now,
                    Elapsed = 0
                };
            }

            var sw = new System.Diagnostics.Stopwatch();
            sw.Start();

            string baseBrofileIdentifier = string.Empty; // this would be the container for 'core' data elements, such as a Generic MMG
            string profileIdentifier = string.Empty; // the condition-specific MMG
            string conditionCode = string.Empty;
            string condition = string.Empty;
            MessageMappingGuide mmg = null;
            bool msh7success = false;
            string uniqueCaseId = string.Empty;
            string localRecordId = string.Empty;
            string nationalReportingJurisdiction = string.Empty;

            List<ProcessResultMessage> messages = new List<ProcessResultMessage>();

            #region Basic sanity/error checks and setup for the Json writing process
            if (!hl7v2message.StartsWith("MSH", StringComparison.OrdinalIgnoreCase))
            {
                messages.Add(new ProcessResultMessage()
                {
                    Severity = Severity.Error,
                    Content = "HL7v2 message appears to be invalid due to missing MSH segment",
                    ErrorCode = "0003"
                });
            }
            else if (!hl7v2message.StartsWith(@"MSH|^~\&", StringComparison.OrdinalIgnoreCase))
            {
                messages.Add(new ProcessResultMessage()
                {
                    Severity = Severity.Error,
                    Content = @"MSH.2 (Encoding Characters) MUST contain the constant '|^~\&'",
                    ErrorCode = "xxxx"
                });
            }

            // if we have errors here, that means we encountered bad enough problems that we must stop further processing
            if (messages.Any(m => m.Severity == Severity.Error))
            {
                return new ConversionResult()
                {
                    ConversionMessages = messages,
                    TransactionId = transactionId,
                    Created = DateTime.Now,
                    Elapsed = sw.Elapsed.TotalMilliseconds
                };
            }

            string hl7v2messageNormalized = hl7v2message.Replace("\r", "\n").Replace("\n\n", "\n");
            char fieldSeparator = hl7v2messageNormalized[3];
            string[] segments = hl7v2messageNormalized.Split('\n');

            string[][] fields = new string[segments.Length][];

            for (int i = 0; i < segments.Length; i++)
            {
                fields[i] = segments[i].Split(fieldSeparator);
            }

            string[] mshFields = segments[0].Split(fieldSeparator);
            string[] obrFields = fields.FirstOrDefault(s => s[0].StartsWith("OBR", StringComparison.OrdinalIgnoreCase));
            string[] pidFields = fields.FirstOrDefault(s => s[0].StartsWith("PID", StringComparison.OrdinalIgnoreCase));
            IEnumerable<string[]> nk1Segments = fields.Where(s => s[0].StartsWith("NK1", StringComparison.OrdinalIgnoreCase));

            bool hasLabTemplateFields = fields.Count(s => s[0].StartsWith("OBR", StringComparison.OrdinalIgnoreCase)) > 1;

            DateTimeOffset dateTimeOfMessage = DateTime.MinValue;

            if (mshFields == null || mshFields.Length < 21 || string.IsNullOrEmpty(mshFields[20]))
            {
                messages.Add(new ProcessResultMessage()
                {
                    Severity = Severity.Error,
                    Content = "HL7v2 message is missing MSH-21 - unable to determine message profile",
                    ErrorCode = "0004"
                });
            }
            else if (obrFields == null || obrFields.Length < 32 || string.IsNullOrEmpty(obrFields[31]))
            {
                messages.Add(new ProcessResultMessage()
                {
                    Severity = Severity.Error,
                    Content = "HL7v2 message is missing OBR-31 (condition code) - unable to determine message profile",
                    ErrorCode = "0005"
                });
            }
            else if (string.IsNullOrWhiteSpace(obrFields[3]))
            {
                messages.Add(new ProcessResultMessage()
                {
                    Severity = Severity.Error,
                    Content = "HL7v2 message is missing OBR-3 (local record ID) - unable to create unique case ID",
                    ErrorCode = "xxxx"
                });
            }
            else
            {
                string[] msh21FieldRepeats = mshFields[20].Split('~');

                if (msh21FieldRepeats.Length == 1)
                {
                    messages.Add(new ProcessResultMessage()
                    {
                        Severity = Severity.Error,
                        Content = "An unsupported literal value was provided for the Message Profile Identifier (NOT115, MSH-21). Replace the unsupported literal value with the required literal value as defined in the conformance statement within the current version of the PHIN Messaging Guide for Case Notification Reporting and the appropriate Message Mapping Guide.",
                        // Path1 = "MSH[1].21[1]"
                        // Value1 = mshFields[20]
                        ErrorCode = "xxxx"
                    });
                }
                else
                {
                    profileIdentifier = msh21FieldRepeats.Last().Split('^').First();
                    baseBrofileIdentifier = msh21FieldRepeats[1].Split('^').First();
                }

                string msh9 = mshFields[8];
                if (string.IsNullOrWhiteSpace(msh9) || !msh9.Equals("ORU^R01^ORU_R01", StringComparison.OrdinalIgnoreCase))
                {
                    messages.Add(new ProcessResultMessage()
                    {
                        Severity = Severity.Error,
                        Content = "MSH-9 (Message Type) does not contain the specified message type. The system expected the literal value 'ORU^R01^ORU_R01'",
                        // Path1 = "MSH[1].9[1]"
                        // Value1 = msh9
                        ErrorCode = "xxxx"
                    });
                }

                string msh12 = mshFields[11];
                if (string.IsNullOrWhiteSpace(msh12) || !msh12.Equals("2.5.1", StringComparison.OrdinalIgnoreCase))
                {
                    messages.Add(new ProcessResultMessage()
                    {
                        Severity = Severity.Error,
                        Content = "MSH-12 (Version ID) does not contain the specified literal '2.5.1'",
                        // Path1 = "MSH[1].12[1]"
                        // Value1 = msh12
                        ErrorCode = "xxxx"
                    });
                }

                localRecordId = obrFields[3].Split('^')[0];

                var obr31components = obrFields[31].Split('^');
                conditionCode = obr31components[0];

                if (obr31components.Length == 1)
                {
                    messages.Add(new ProcessResultMessage()
                    {
                        Severity = Severity.Error,
                        Content = "INV169 (Condition Code) is missing the code system identifier and code description.",
                        // Path1 = "OBR[1].31[1].3"
                        // Value1 = "OBR-31.3"
                        ErrorCode = "xxxx"
                    });
                }
                else
                {
                    condition = obr31components[1];
                }

                string obr4 = obrFields[4];
                if (string.IsNullOrWhiteSpace(obr4) || !obr4.Equals("68991-9^Epidemiologic Information^LN", StringComparison.Ordinal))
                {
                    messages.Add(new ProcessResultMessage()
                    {
                        Severity = Severity.Error,
                        Content = "OBR-4 is required",
                        ErrorCode = "xxxx"
                    });
                }

                // This should never be empty or invalid, but just check anyway
                msh7success = Common.TryConvertHl7DateTimeToUniversalTime(mshFields[6], out dateTimeOfMessage);
                if (!msh7success)
                {
                    messages.Add(new ProcessResultMessage()
                    {
                        Severity = Severity.Warning,
                        Content = "HL7v2 message is missing MSH-7 (date/time of message)",
                        ErrorCode = "0009"
                    });
                }

                try
                {
                    mmg = _mmgService.Get(profileIdentifier, conditionCode);

                    if (mmg == null)
                    {
                        messages.Add(new ProcessResultMessage()
                        {
                            Severity = Severity.Error,
                            Content = "Unable to locate an MMG for this HL7v2 message",
                            ErrorCode = "0006"
                        });
                    }
                }
                catch (Exception ex)
                {
                    messages.Add(new ProcessResultMessage()
                    {
                        Severity = Severity.Error,
                        Content = "Unable to retrieve MMG schema from MMG service due to exception: " + ex.GetType() + " : " + ex.Message,
                        ErrorCode = "0007"
                    });
                }
            }

            // if we have errors here, that means we encountered bad enough problems that we must stop further processing
            if (messages.Any(m => m.Severity == Severity.Error))
            {
                return new ConversionResult()
                {
                    BaseProfile = baseBrofileIdentifier,
                    Profile = profileIdentifier,
                    Condition = condition,
                    ConditionCode = conditionCode,
                    Content = string.Empty,
                    ConversionMessages = messages,
                    TransactionId = transactionId,
                    Created = DateTime.Now,
                    MessageDateTime = dateTimeOfMessage,
                    Elapsed = sw.Elapsed.TotalMilliseconds
                };
            }
            #endregion

            string json = string.Empty;

            using (var stream = new MemoryStream())
            {
                using (var writer = new Utf8JsonWriter(stream, _jsonWriterOptions))
                {
                    int maxObx4 = 0;

                    var obx4Segments = fields
                        .Where(s => s[0] == "OBX")
                        .Where(s => s.Length > 4)
                        .Where(s => int.TryParse(s[4], out _));

                    if (obx4Segments.Count() > 0)
                    {
                        maxObx4 = obx4Segments
                            .Max(s => int.Parse(s[4]));
                    }

                    writer.WriteStartObject();

                    // write the transaction ID that we use to track pipeline processing
                    if (!string.IsNullOrWhiteSpace(transactionId))
                    {
                        writer.WriteString("transaction_id", transactionId);
                    }
                    else
                    {
                        messages.Add(new ProcessResultMessage()
                        {
                            Severity = Severity.Warning,
                            Content = "Conversion function is missing an external transaction identifier as a function argument",
                            ErrorCode = "0008"
                        });
                    }

                    // write the source format, in this case "HL7v2"
                    writer.WriteString("source_format", SOURCE_FORMAT);

                    // write the date/time of the message
                    if (msh7success)
                    {
                        writer.WriteString("datetime_of_message", dateTimeOfMessage.ToString("s"));
                    }

                    Dictionary<string, string[]> repeatSegmentDictionary = new Dictionary<string, string[]>();

                    foreach (string[] segment in fields)
                    {
                        // check for OBX-11
                        if (segment[0].Equals("OBX", StringComparison.OrdinalIgnoreCase) && segment.Length < 11)
                        {
                            messages.Add(new ProcessResultMessage()
                            {
                                Severity = Severity.Error,
                                Content = "OBX-11 (Observation Result Status) is a required data element but is not populated.",
                                ErrorCode = "xxxx"
                            });
                        }

                        if (segment[0].Equals("OBX", StringComparison.OrdinalIgnoreCase) && segment.Length <= 3)
                        {
                            messages.Add(new ProcessResultMessage()
                            {
                                Severity = Severity.Error,
                                Content = "OBX-3 (Observation Identifier) is a required data element and must be populated.",
                                ErrorCode = "xxxx"
                            });
                        }
                        else if (segment[0].Equals("OBX", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrEmpty(segment[4]))
                        {
                            string id = segment[3].Substring(0, segment[3].IndexOf('^')) + "__" + segment[4];
                            repeatSegmentDictionary.Add(id, segment);
                        }

                        if (segment[0].Equals("OBX", StringComparison.OrdinalIgnoreCase) && segment.Length > 3 && segment[3].StartsWith("77968-6^", StringComparison.OrdinalIgnoreCase))
                        {
                            nationalReportingJurisdiction = segment[5].Split('^')[0];
                        }
                    }

                    // write the unique case ID
                    uniqueCaseId = nationalReportingJurisdiction + "_" + localRecordId;
                    writer.WriteString("unique_case_id", uniqueCaseId);

                    /* 
                     * do the conversion - iterate over all data elements in the MMG. For each data element,
                     * find the corresponding HL7v2 segment/field/component, extract it, parse it, and then
                     * write it Json.
                     */
                    foreach (var block in mmg.Blocks
                        .Where(b => b.Type != BlockType.Info)
                        .Where(b => b.Elements.Count > 0)) // some blocks are just for labels and have no elements in them; skip these
                    {
                        if (block.Type == BlockType.Single)
                        {
                            // non-repeating blocks (BlockType.Single) are straight Json properties
                            foreach (var element in block.Elements)
                            {
                                var elementMessages = WriteDataElementInNonRepeatingBlock(element, writer, fields, mshFields, obrFields, pidFields, mmg);
                                messages.AddRange(elementMessages);
                            }
                        }
                        else
                        {
                            // repeating blocks must be represented as Json arrays
                            writer.WriteStartArray(Common.FormatDataElementName(block.Name));

                            int failedWrites = 0;

                            /* The logic here is to iterate from 0 to the maxiumum OBX-4 value we found in the HL7v2 message. For each
                             * integer value, we aggregate all elements that (a) are in this particular MMG block and (b) that have a
                             * segment in the HL7v2 message where the OBX-4 value equals the loop iterator value (that is, the variable i).
                             * If we find a match we add it to a queue. One might wonder why add it to a queue instead of writing it?
                             * The queue is possibly unnecessary, but was designed to handle non-sequential OBX-4 values. If we do a 
                             * 'StartWriteObject' command for the JsonWriter then we can't undo that command later if we find there's nothing
                             * to write. I didn't want to end up with empty objects in the repeating array for some weird edge cases. Hence,
                             * the queue - by using the queue, we can control for situations where, for a given value of 'i', there is
                             * no data to write.
                             */
                            for (int i = 0; i <= maxObx4; i++)
                            {
                                Queue<Tuple<DataElement, string[]>> writeQueue = new Queue<Tuple<DataElement, string[]>>();

                                foreach (var element in block.Elements)
                                {
                                    var mapping = element.Mappings.Hl7v251;
                                    string id = mapping.Identifier + "__" + i;

                                    if (repeatSegmentDictionary.ContainsKey(id))
                                    {
                                        var matchedSegment = repeatSegmentDictionary[id];
                                        writeQueue.Enqueue(new Tuple<DataElement, string[]>(element, matchedSegment));
                                    }
                                }

                                if (writeQueue.Count > 0)
                                {
                                    writer.WriteStartObject();
                                    while (writeQueue.Count > 0)
                                    {
                                        var pair = writeQueue.Dequeue();
                                        DataElement element = pair.Item1;
                                        var mapping = element.Mappings.Hl7v251;

                                        WriteData(element, writer, pair.Item2[mapping.FieldPosition.HasValue ? mapping.FieldPosition.Value : 5]);
                                    }
                                    writer.WriteEndObject();
                                }
                                else if (failedWrites > 200)
                                {
                                    /* OBX-4 values can be non-sequential. That and the optionality of elements in a repeating block
                                     * means it's hard to tell when the algorithm is done. So this is a fail-safe that says, 'Hey if we
                                     * didn't find anything to write for the last few OBX integer values in our loop, just stop.' This
                                     * is important for performance reasons.
                                     */
                                    break;
                                }
                                else
                                {
                                    failedWrites++;
                                }
                            }

                            writer.WriteEndArray();
                        }
                    }

                    if (nk1Segments != null && nk1Segments.Count() > 0)
                    {
                        writer.WriteStartArray("next_of_kin");

                        foreach (var nk1Segment in nk1Segments)
                        {
                            var failMessages = WriteNK1Segment(writer, nk1Segment);
                            messages.AddRange(failMessages);
                        }

                        writer.WriteEndArray();
                    }

                    if (hasLabTemplateFields)
                    {
                        var failMessages = ConvertLabTemplate(writer, fields);
                        messages.AddRange(failMessages);
                    }

                    writer.WriteEndObject();
                }
                json = Encoding.UTF8.GetString(stream.ToArray());
            }
            sw.Stop();

            return new ConversionResult()
            {
                Content = json,
                TransactionId = transactionId,
                BaseProfile = baseBrofileIdentifier,
                Profile = profileIdentifier,
                Condition = condition,
                ConditionCode = conditionCode,
                ConversionMessages = messages,
                MessageDateTime = dateTimeOfMessage,
                NationalReportingJurisdiction = nationalReportingJurisdiction,
                LocalRecordId = localRecordId,
                UniqueCaseId = uniqueCaseId,
                Created = DateTime.Now,
                Elapsed = sw.Elapsed.TotalMilliseconds
            };
        }

        private List<ProcessResultMessage> WriteDataElementInNonRepeatingBlock(DataElement element, Utf8JsonWriter writer, string[][] fields, string[] mshFields, string[] obrFields, string[] pidFields, MessageMappingGuide mmg)
        {
            List<ProcessResultMessage> messages = new List<ProcessResultMessage>(1);

            var mapping = element.Mappings.Hl7v251;
            string identifier = mapping.Identifier;
            string propertyName = Common.FormatDataElementName(element.Name);

            if (mapping.FieldPosition == 5 && mapping.SegmentType == Mmg.HL7V251.SegmentType.OBX)
            {
                if (element.IsRepeat || (element.Repetitions.HasValue && element.Repetitions > 1))
                {
                    string[] segment = GetOBXSegmentForIdentifier(identifier, fields);

                    if (segment != null)
                    {
                        var data = segment[5];
                        writer.WriteStartArray(Common.FormatDataElementName(element.Name));
                        var failMessages = WriteData(element, writer, data, true);
                        writer.WriteEndArray();
                        messages.AddRange(failMessages);
                    }
                }
                else
                {
                    // basic OBX-5 key/value pair
                    (string Data, string Code, string Description, string CodeSystem) = GetObx5FieldData(element, identifier, fields);
                    var obx5FailMessages = WriteData(element: element, writer: writer, data: Data, description: Description, code: Code, codeSystem: CodeSystem, writeString: WriteString, writeNumber: WriteNumber);
                    messages.AddRange(obx5FailMessages);
                }
            }
            else if (mapping.FieldPosition.HasValue && mapping.FieldPosition >= 6 && mapping.SegmentType == Mmg.HL7V251.SegmentType.OBX)
            {
                DataElement relatedElement = mmg.Elements.FirstOrDefault(e => e.Id == element.RelatedElementId);
                if (relatedElement == null) return messages;

                string relatedIdentifier = relatedElement.Mappings.Hl7v251.Identifier;

                string[] segment = GetOBXSegmentForIdentifier(relatedIdentifier, fields);
                string data = string.Empty;
                if (segment != null)
                {
                    data = segment[mapping.FieldPosition.Value];
                }

                var failmessages = WriteData(element, writer, data);
                messages.AddRange(failmessages);
            }
            #region Handle Generic MMG core elements that are not mapped to OBX segments
            else if (mapping.Identifier == "NOT115" || mapping.LegacyIdentifier == "NOT115")
            {
                string msh21 = mshFields[20];
                string[] msh21Repeats = msh21.Split('~');

                string profileIdentifier = msh21Repeats.Last().Split('^').First();
                var failmessages = WriteData(element, writer, profileIdentifier);
                messages.AddRange(failmessages);

                writer.WriteStartArray("message_profile_identifiers");
                foreach (var repeat in msh21Repeats)
                {
                    writer.WriteStringValue(repeat.Split('^').First());
                }
                writer.WriteEndArray();
            }
            else if (pidFields != null && pidFields.Length > 3 && (mapping.Identifier == "DEM197" || mapping.LegacyIdentifier == "DEM197"))
            {
                string data = pidFields[3].Split('^')[0];
                var failMessages = WriteData(element, writer, data);
                messages.AddRange(failMessages);
            }
            else if (pidFields != null && pidFields.Length > 7 && (mapping.Identifier == "DEM115" || mapping.LegacyIdentifier == "DEM115"))
            {
                string data = pidFields[7];
                var failMessages = WriteData(element, writer, data);
                messages.AddRange(failMessages);
            }
            else if (pidFields != null && pidFields.Length > 8 && (mapping.Identifier == "DEM113" || mapping.LegacyIdentifier == "DEM113"))
            {
                string data = pidFields[8];
                var failMessages = WriteData(element, writer, data);
                messages.AddRange(failMessages);
            }
            else if (pidFields != null && pidFields.Length > 10 && (mapping.Identifier == "DEM152" || mapping.LegacyIdentifier == "DEM152"))
            {
                string[] repeats = pidFields[10].Split('~');

                writer.WriteStartArray(propertyName);
                foreach (string repeat in repeats)
                {
                    var failMessages = WriteData(element, writer, repeat, true);
                    messages.AddRange(failMessages);
                }
                writer.WriteEndArray();
            }
            else if (pidFields != null && pidFields.Length > 22 && (mapping.Identifier == "DEM155" || mapping.LegacyIdentifier == "DEM155"))
            {
                string[] repeats = pidFields[22].Split('~');

                //writer.WriteStartArray(propertyName);
                foreach (string repeat in repeats)
                {
                    var failMessages = WriteData(element, writer, repeat, /*true*/ false);
                    messages.AddRange(failMessages);
                }
                //writer.WriteEndArray();
            }
            else if (pidFields != null && (mapping.Identifier == "DEM165" || mapping.LegacyIdentifier == "DEM165"))
            {
                string[] components = pidFields[11].Split('^');
                string data = string.Empty;

                if (components.Length > 8)
                {
                    data = components[8];
                }
                var failMessages = WriteData(element, writer, data);
                messages.AddRange(failMessages);
            }
            else if (pidFields != null && (mapping.Identifier == "DEM161" || mapping.LegacyIdentifier == "DEM161"))
            {
                string[] components = pidFields[11].Split('^');
                string data = string.Empty;

                if (components.Length > 2)
                {
                    data = components[2];
                }
                var failMessages = WriteData(element, writer, data);
                messages.AddRange(failMessages);
            }
            else if (pidFields != null && (mapping.Identifier == "DEM162" || mapping.LegacyIdentifier == "DEM162"))
            {
                string[] components = pidFields[11].Split('^');
                string data = string.Empty;

                if (components.Length > 3)
                {
                    data = components[3];
                }
                var failMessages = WriteData(element, writer, data);
                messages.AddRange(failMessages);
            }
            else if (pidFields != null && (mapping.Identifier == "DEM163" || mapping.LegacyIdentifier == "DEM163"))
            {
                string[] components = pidFields[11].Split('^');
                string data = string.Empty;

                if (components.Length > 4)
                {
                    data = components[4];
                }
                var failMessages = WriteData(element, writer, data);
                messages.AddRange(failMessages);
            }
            else if (mapping.Identifier == "INV168" || mapping.LegacyIdentifier == "INV168")
            {
                string data = obrFields[3].Split('^')[0];
                WriteData(element, writer, data);
            }
            else if (mapping.Identifier == "INV169" || mapping.LegacyIdentifier == "INV169")
            {
                string data = obrFields[31];
                var failMessages = WriteData(element, writer, data);
                messages.AddRange(failMessages);
            }
            else if (pidFields != null && (mapping.Identifier == "INV146" || mapping.LegacyIdentifier == "INV146"))
            {
                string data = string.Empty;
                if (pidFields.Length > 29)
                {
                    data = pidFields[29];
                }
                var failMessages = WriteData(element, writer, data);
                messages.AddRange(failMessages);
            }
            else if (mapping.Identifier == "NOT106" || mapping.LegacyIdentifier == "NOT106")
            {
                string data = obrFields[22];
                var failMessages = WriteData(element, writer, data);
                messages.AddRange(failMessages);
            }
            else if (mapping.Identifier == "NOT118" || mapping.LegacyIdentifier == "NOT118")
            {
                string data = obrFields[25];

                if (data.Equals("F", StringComparison.OrdinalIgnoreCase))
                {
                    data = data + "^Final results; Can only be changed with a corrected result.^HL7";
                }
                else if (data.Equals("C", StringComparison.OrdinalIgnoreCase))
                {
                    data = data + "^Record coming over is a correction and thus replaces a final result^HL7";
                }
                else if (data.Equals("X", StringComparison.OrdinalIgnoreCase))
                {
                    data = data + "^Results cannot be obtained for this observation^HL7";
                }

                var failMessages = WriteData(element, writer, data);
                messages.AddRange(failMessages);
            }
            else if (mapping.Identifier == "NOT103" || mapping.LegacyIdentifier == "NOT103")
            {
                string data = obrFields[7];
                var failMessages = WriteData(element, writer, data);
                messages.AddRange(failMessages);
            }
            #endregion
            else if (mapping.SegmentType == Mmg.HL7V251.SegmentType.PID && mapping.FieldPosition.HasValue && pidFields != null)
            {
                if (pidFields.Length <= mapping.FieldPosition.Value)
                {
                    messages.Add(new ProcessResultMessage()
                    {
                        Content = "Element '" + element.Name + "' (" + element.Mappings.Hl7v251.Identifier + ") is mapped to a PID field. However, this field is absent from the message.",
                        Severity = Severity.Information,
                        ErrorCode = "0010"
                    });
                }
                else
                {
                    string data = pidFields[mapping.FieldPosition.Value];

                    if (mapping.ComponentPosition.HasValue)
                    {
                        data = data.Split('^')[mapping.ComponentPosition.Value - 1];
                    }
                    var failMessages = WriteData(element, writer, data);
                    messages.AddRange(failMessages);
                }
            }
            else if (mapping.SegmentType == Mmg.HL7V251.SegmentType.NK1 && mapping.FieldPosition.HasValue)
            {
                messages.Add(new ProcessResultMessage()
                {
                    Content = "Element '" + element.Name + "' (" + element.Mappings.Hl7v251.Identifier + ") is mapped to an NK1 field. However, NK1 data is contained in a generic next-of-kin property in the Json output.",
                    Severity = Severity.Information,
                    ErrorCode = "0010"
                });
            }
            else
            {
                messages.Add(new ProcessResultMessage()
                {
                    Content = "Element '" + element.Name + "' (" + element.Mappings.Hl7v251.Identifier + ") was not mapped to a Json structure",
                    Severity = Severity.Warning,
                    ErrorCode = "0001"
                });
            }

            return messages;
        }

        private string[] GetOBXSegmentForIdentifier(string identifier, string[][] fields)
        {
            foreach (var segment in fields)
            {
                if (segment.Length >= 5 && segment[3].Split('^')[0] == identifier)
                {
                    return segment;
                }
            }

            return null;
        }

        private List<ProcessResultMessage> WriteData(DataElement element, Utf8JsonWriter writer, string data, bool isRepeatingElement = false)
        {
            List<ProcessResultMessage> messages = new List<ProcessResultMessage>(1);

            (string Data, string Code, string Description, string CodeSystem) = ExtractData(element, data);

            if (isRepeatingElement)
            {
                writer.WriteStartObject();
                var failMessages = WriteData(element: element, writer: writer, data: Data, description: Description, code: Code, codeSystem: CodeSystem, writeString: WriteString, writeNumber: WriteNumber);
                writer.WriteEndObject();
                messages.AddRange(failMessages);
            }
            else
            {
                var failMessages = WriteData(element: element, writer: writer, data: Data, description: Description, code: Code, codeSystem: CodeSystem, writeString: WriteString, writeNumber: WriteNumber);
                messages.AddRange(failMessages);
            }

            return messages;
        }

        private List<ProcessResultMessage> WriteData(
            DataElement element,
            Utf8JsonWriter writer,
            string data,
            string description,
            string code,
            string codeSystem,
            Action<Utf8JsonWriter, string, string> writeString,
            Action<Utf8JsonWriter, string, double> writeNumber)
        {
            // TODO: Revisit the use of function pointers here - may not be necessary. These were added to support
            // repeating elements that were simple types (e.g. text, number)

            List<ProcessResultMessage> messages = new List<ProcessResultMessage>(1);

            string propertyName = Common.FormatDataElementName(element.Name);

            if (!string.IsNullOrEmpty(data))
            {
                if (element.DataType == DataType.Numeric)
                {
                    if (double.TryParse(data, out double numericData))
                    {
                        //writer.WriteNumber(propertyName, numericData);
                        writeNumber(writer, propertyName, numericData);
                    }
                    else
                    {
                        // TODO: Flag this as invalid
                    }
                }
                else if (element.DataType == DataType.Integer)
                {
                    if (int.TryParse(data, out int numericData))
                    {
                        //writer.WriteNumber(propertyName, numericData);
                        writeNumber(writer, propertyName, numericData);
                    }
                    else
                    {
                        // TODO: Flag this as invalid
                    }
                }
                else if (element.DataType == DataType.DateTime || element.DataType == DataType.Date)
                {
                    if (Common.TryConvertHl7DateTimeToUniversalTime(data, out DateTimeOffset dt))
                    {
                        string utcDate = dt.ToString("s");
                        writeString(writer, propertyName, utcDate);
                    }
                    else
                    {
                        writeString(writer, propertyName, data);
                    }

                }
                else
                {
                    writeString(writer, propertyName, data);
                }
            }
            else if (!string.IsNullOrEmpty(description))
            {
                //writer.WriteString(propertyName, description);
                writeString(writer, propertyName, description);
            }

            if (!string.IsNullOrEmpty(code))
            {
                //writer.WriteString(propertyName + CODE_SUFFIX, code);
                writeString(writer, propertyName + CODE_SUFFIX, code);
            }
            if (!string.IsNullOrEmpty(codeSystem))
            {
                writeString(writer, propertyName + CODE_SYSTEM_SUFFIX, codeSystem);
            }

            return messages;
        }

        private void WriteNumber(Utf8JsonWriter writer, string propertyName, int number) => writer.WriteNumber(propertyName, number);

        private void WriteNumber(Utf8JsonWriter writer, string propertyName, double number) => writer.WriteNumber(propertyName, number);

        private void WriteString(Utf8JsonWriter writer, string propertyName, string value) => writer.WriteString(propertyName, Common.UnescapeHL7v2Text(value));

        private void WriteNumberValue(Utf8JsonWriter writer, string propertyName, int number) => writer.WriteNumberValue(number);

        private void WriteNumberValue(Utf8JsonWriter writer, string propertyName, double number) => writer.WriteNumberValue(number);

        private void WriteStringValue(Utf8JsonWriter writer, string propertyName, string value) => writer.WriteStringValue(Common.UnescapeHL7v2Text(value));

        private (string Data, string Code, string Description, string CodeSystem) GetObx5FieldData(DataElement element, string identifier, string[][] fields)
        {
            string[] segment = GetOBXSegmentForIdentifier(identifier, fields);
            if (segment != null)
            { 
                string data = segment[5];
                return ExtractData(element, data);
            }
            return (string.Empty, string.Empty, string.Empty, string.Empty);
        }

        private (string Data, string Code, string Description, string CodeSystem) ExtractData(DataElement element, string data)
        {
            if (element.DataType == DataType.Coded)
            {
                var components = data.Split('^');
                if (components.Length >= 2)
                {
                    return (string.Empty, components[0], components[1], components[2]);
                }
                else if(components.Length >= 1)
                {
                    return (string.Empty, components[0], string.Empty, string.Empty);
                }
                else
                {
                    return (data, string.Empty, string.Empty, string.Empty);
                }
            }
            else if (element.DataType == DataType.Numeric)
            {
                var components = data.Split('^');
                return (components[1], string.Empty, string.Empty, string.Empty);
            }
            else
            {
                return (data, string.Empty, string.Empty, string.Empty);
            }
        }
        
        private List<ProcessResultMessage> ConvertLabTemplate(Utf8JsonWriter writer, string[][] fields)
        {
            List<ProcessResultMessage> messages = new List<ProcessResultMessage>(1);

            //List<string[]> obrSegments = fields
            //    .Where(s => s[0].StartsWith("OBR"))
            //    .Skip(1)
            //    .ToList();

            List<string[]> labSegments = fields
                .SkipWhile(s => !(s[0] + "|" + s[1]).StartsWith("OBR|2", StringComparison.OrdinalIgnoreCase))
                .ToList();

            writer.WriteStartArray("laboratory_information");

            Queue<string[]> obxQueue = new Queue<string[]>(8);
            Queue<string[]> spmQueue = new Queue<string[]>(8);

            for (int i = 0; i < labSegments.Count; i++)
            {
                var labSegment = labSegments[i];

                if (labSegment[0] == "OBR")
                {
                    if (i != 0)
                    {
                        writer.WriteEndObject();
                    }
                    writer.WriteStartObject();

                    if (!string.IsNullOrEmpty(labSegment[2]))
                    {
                        var splitValues = labSegment[2].Split('^');
                        writer.WriteString("placer_order_number", splitValues.Length > 1 ? splitValues[1] : string.Empty);
                        writer.WriteString("placer_order_number" + CODE_SUFFIX, splitValues[0]);
                        writer.WriteString("placer_order_number" + CODE_SYSTEM_SUFFIX, splitValues.Length > 2 ? splitValues[2] : string.Empty);
                    }
                    if (!string.IsNullOrEmpty(labSegment[3]))
                    {
                        var splitValues = labSegment[3].Split('^');
                        writer.WriteString("filler_order_number", splitValues.Length > 1 ? splitValues[1] : string.Empty);
                        writer.WriteString("filler_order_number" + CODE_SUFFIX, splitValues[0]);
                        writer.WriteString("filler_order_number" + CODE_SYSTEM_SUFFIX, splitValues.Length > 2 ? splitValues[2] : string.Empty);
                    }
                    if (!string.IsNullOrEmpty(labSegment[4]))
                    {
                        var splitValues = labSegment[4].Split('^');
                        writer.WriteString("test_ordered_name", splitValues.Length > 1 ? splitValues[1] : string.Empty);
                        writer.WriteString("test_ordered_name" + CODE_SUFFIX, splitValues[0]);
                        writer.WriteString("test_ordered_name" + CODE_SYSTEM_SUFFIX, splitValues.Length > 2 ? splitValues[2] : string.Empty);
                    }
                    if (!string.IsNullOrEmpty(labSegment[7]))
                    {
                        writer.WriteString("observation_datetime", Common.ConvertHl7DateTimeToUniversalTime(labSegment[7]));
                    }
                    if (!string.IsNullOrEmpty(labSegment[11]))
                    {
                        writer.WriteString("specimen_action_code", labSegment[11]);
                    }
                    if (!string.IsNullOrEmpty(labSegment[16]))
                    {
                        writer.WriteStartObject("ordering_provider");

                        var firstRepeat = labSegment[16].Split('~').First();
                        var components = firstRepeat.Split('^');

                        if (components.Length >= 1)
                        {
                            writer.WriteString("person_identifier", components[0]);
                        }

                        if (components.Length >= 9)
                        {
                            writer.WriteString("assigning_authority", components[8]);
                        }
                        if (components.Length >= 10)
                        {
                            writer.WriteString("name_type_code", components[9]);
                        }
                        if (components.Length >= 13)
                        {
                            writer.WriteString("identifier_type_code", components[12]);
                        }

                        if (components.Length >= 14 && !string.IsNullOrEmpty(components[13]))
                        {
                            writer.WriteString("assigning_facility", components[13].Split('&')[1]);
                            writer.WriteString("assigning_facility" + CODE_SUFFIX, components[13].Split('&')[0]);
                            writer.WriteString("assigning_facility" + CODE_SYSTEM_SUFFIX, components[13].Split('&')[2]);
                        }

                        if (components.Length >= 21)
                        {
                            writer.WriteString("professional_suffix", components[20]);
                        }

                        writer.WriteEndObject(); // end write ordering_provider
                    }
                    if (!string.IsNullOrEmpty(labSegment[22]))
                    {
                        writer.WriteString(Common.FormatDataElementName("Results Rpt/Status Chng - Date/Time"), Common.ConvertHl7DateTimeToUniversalTime(labSegment[22]));
                    }
                    if (!string.IsNullOrEmpty(labSegment[25]))
                    {
                        writer.WriteString(Common.FormatDataElementName("Result Status"), labSegment[25]);
                    }
                    if (labSegment.Length >= 32 && !string.IsNullOrEmpty(labSegment[31]))
                    {
                        var repeats = labSegment[31].Split('~');
                        writer.WriteStartArray(Common.FormatDataElementName("Reason for Study"));
                        foreach (var repeat in repeats)
                        {
                            var components = repeat.Split('^');

                            writer.WriteStartObject();
                            writer.WriteString("data", components[1]);
                            writer.WriteString("code", components[0]);
                            writer.WriteString("code_system", components[2]);
                            writer.WriteEndObject();
                        }
                        writer.WriteEndArray(); // end Reason for Study
                    }
                }
                else if (labSegment[0] == "OBX")
                {
                    obxQueue.Enqueue(labSegment);
                }
                else if (labSegment[0] == "SPM")
                {
                    spmQueue.Enqueue(labSegment);
                }
                else if (labSegment[0] == "NTE")
                {
                    obxQueue.Enqueue(labSegment);
                }

                if (obxQueue.Count > 0)
                {
                    writer.WriteStartArray("observation_results");

                    while (obxQueue.Count > 0)
                    {
                        string[] obxSegment = obxQueue.Peek();
                        if (obxSegment[0] == "OBX")
                        {
                            obxSegment = obxQueue.Dequeue();
                        }
                        else
                        {
                            break;
                        }

                        writer.WriteStartObject();

                        // write OBX segment to Json here

                        string datatype = obxSegment[2];

                        if (!string.IsNullOrEmpty(obxSegment[3]))
                        {
                            string fieldName = Common.FormatDataElementName("Test Performed Name");

                            string field = obxSegment[3];

                            string[] components = field.Split('^');
                            messages.AddRange(WriteGenericCodedData(writer, fieldName, components));
                        }
                        if (!string.IsNullOrEmpty(obxSegment[4]))
                        {
                            writer.WriteString(Common.FormatDataElementName("Observation Sub-ID"), obxSegment[4]);
                        }
                        if (!string.IsNullOrEmpty(obxSegment[5]))
                        {
                            string fieldName = Common.FormatDataElementName("Test Result");
                            string field = obxSegment[5];

                            if (datatype == "CWE" || datatype == "CE")
                            {
                                string[] components = field.Split('^');

                                messages.AddRange(WriteGenericCodedData(writer, fieldName, components));
                            }
                            else if (datatype == "NM")
                            {
                                if (double.TryParse(obxSegment[5], out double number))
                                {
                                    writer.WriteNumber(fieldName, number);
                                }
                                else
                                {
                                    // TODO: Raise conversion warning
                                }
                            }
                            else if (datatype == "SN")
                            {
                                string[] components = field.Split('^');
                                writer.WriteString(fieldName + "__comparator", components[0]);
                                if (double.TryParse(components[1], out double number))
                                {
                                    writer.WriteNumber(fieldName, number);
                                }
                                else
                                {
                                    // TODO: Raise conversion warning
                                }
                            }
                            else
                            {
                                writer.WriteString(fieldName, obxSegment[5]);
                            }
                        }
                        if (obxSegment.Length > 6 && !string.IsNullOrEmpty(obxSegment[6]))
                        {
                            string fieldName = Common.FormatDataElementName("Units of Measure");
                            string field = obxSegment[6];
                            string[] components = field.Split('^');
                            messages.AddRange(WriteGenericCodedData(writer, fieldName, components));
                        }
                        if (obxSegment.Length > 8 && !string.IsNullOrEmpty(obxSegment[8]))
                        {
                            writer.WriteString(Common.FormatDataElementName("Test Result -Interpretation Flag"), obxSegment[8]);
                        }
                        if (obxSegment.Length > 11 && !string.IsNullOrEmpty(obxSegment[11]))
                        {
                            writer.WriteString(Common.FormatDataElementName("Observation Result Status"), obxSegment[11]);
                        }
                        if (obxSegment.Length > 14 && !string.IsNullOrEmpty(obxSegment[14]))
                        {
                            writer.WriteString(Common.FormatDataElementName("Specimen Collection Date"), Common.ConvertHl7DateTimeToUniversalTime(obxSegment[14]));
                        }
                        if (obxSegment.Length > 17 && !string.IsNullOrEmpty(obxSegment[17]))
                        {
                            string fieldName = Common.FormatDataElementName("Test Method");
                            string field = obxSegment[17];
                            string[] components = field.Split('^');
                            messages.AddRange(WriteGenericCodedData(writer, fieldName, components));
                        }
                        if (obxSegment.Length > 19 && !string.IsNullOrEmpty(obxSegment[19]))
                        {
                            writer.WriteString(Common.FormatDataElementName("Specimen Analyzed Date"), Common.ConvertHl7DateTimeToUniversalTime(obxSegment[19]));
                        }
                        if (obxSegment.Length > 23 && !string.IsNullOrEmpty(obxSegment[23]))
                        {
                            writer.WriteStartObject("performing_laboratory_name");

                            var firstRepeat = obxSegment[23].Split('~').First();
                            var components = firstRepeat.Split('^');

                            writer.WriteString("organization_name", components[0]);

                            writer.WriteEndObject(); // end write ordering_provider
                        }
                        if (obxSegment.Length > 25 && !string.IsNullOrEmpty(obxSegment[25]))
                        {
                            writer.WriteStartObject("performing_person_name");

                            var firstRepeat = obxSegment[25].Split('~').First();
                            var components = firstRepeat.Split('^');

                            if (components.Length >= 1)
                            {
                                writer.WriteString("person_identifier", components[0]);
                            }

                            if (components.Length >= 9)
                            {
                                writer.WriteString("assigning_authority", components[8]);
                            }
                            if (components.Length >= 10)
                            {
                                writer.WriteString("name_type_code", components[9]);
                            }
                            if (components.Length >= 13)
                            {
                                writer.WriteString("identifier_type_code", components[12]);
                            }

                            if (components.Length >= 14 && !string.IsNullOrEmpty(components[13]))
                            {
                                writer.WriteString("assigning_facility", components[13].Split('&')[1]);
                                writer.WriteString("assigning_facility" + CODE_SUFFIX, components[13].Split('&')[0]);
                                writer.WriteString("assigning_facility" + CODE_SYSTEM_SUFFIX, components[13].Split('&')[2]);
                            }

                            if (components.Length >= 21)
                            {
                                writer.WriteString("professional_suffix", components[20]);
                            }

                            writer.WriteEndObject(); // end write ordering_provider
                        }

                        // check for NTEs...
                        if (obxQueue.Count > 0)
                        {
                            string[] nextObxSegment = obxQueue.Peek();
                            if (nextObxSegment != null && nextObxSegment.Length > 0 && nextObxSegment[0] == "NTE")
                            {
                                writer.WriteStartObject("notes and comments");

                                string[] nteSegment = obxQueue.Dequeue();

                                if (nteSegment.Length > 1 && !string.IsNullOrEmpty(nteSegment[1]))
                                {
                                    writer.WriteString(Common.FormatDataElementName("Set ID - NTE"), nteSegment[1]);
                                }
                                if (nteSegment.Length > 2 && !string.IsNullOrEmpty(nteSegment[2]))
                                {
                                    writer.WriteString(Common.FormatDataElementName("Source of Comment"), nteSegment[2]);
                                }
                                if (nteSegment.Length > 4 && !string.IsNullOrEmpty(nteSegment[4]))
                                {
                                    writer.WriteString(Common.FormatDataElementName("Comment"), nteSegment[3]);
                                }

                                writer.WriteEndObject(); // end write notes and comments
                            }
                        }

                        writer.WriteEndObject(); // end write observation
                    }

                    writer.WriteEndArray(); // end write observation_results
                }

                if (spmQueue.Count > 0)
                {
                    writer.WriteStartArray("specimens");

                    while (spmQueue.Count > 0)
                    {
                        string[] spmSegment = spmQueue.Dequeue();

                        writer.WriteStartObject();

                        if (labSegment.Length > 2 && !string.IsNullOrEmpty(labSegment[2]))
                        {
                            var components = labSegment[2].Split('^');
                            writer.WriteString(Common.FormatDataElementName("Specimen ID Placer Assigned Identifier"), components[0]);
                            writer.WriteString(Common.FormatDataElementName("Specimen ID Filler Assigned Identifier"), components[1]);
                        }
                        if (labSegment.Length > 4 && !string.IsNullOrEmpty(labSegment[4]))
                        {
                            string fieldName = Common.FormatDataElementName("Specimen Type");

                            var components = labSegment[4].Split('^');
                            messages.AddRange(WriteGenericCodedData(writer, fieldName, components));
                        }
                        if (labSegment.Length > 8 && !string.IsNullOrEmpty(labSegment[8]))
                        {
                            string fieldName = Common.FormatDataElementName("Specimen Source Site");

                            var components = labSegment[8].Split('^');
                            messages.AddRange(WriteGenericCodedData(writer, fieldName, components));
                        }
                        if (labSegment.Length > 11 && !string.IsNullOrEmpty(labSegment[11]))
                        {
                            string fieldName = Common.FormatDataElementName("Specimen Role");

                            var components = labSegment[11].Split('^');
                            messages.AddRange(WriteGenericCodedData(writer, fieldName, components));
                        }
                        if (labSegment.Length > 12 && !string.IsNullOrEmpty(labSegment[12]))
                        {
                            string fieldName = Common.FormatDataElementName("Specimen Collection Amount");
                            string[] components = labSegment[12].Split('^');

                            writer.WriteStartObject(fieldName);

                            if (double.TryParse(components[0], out double quantity))
                            {
                                writer.WriteNumber("quantity", quantity);
                            }

                            if (components.Length == 2)
                            {
                                string[] subcomponents = components[1].Split('&');

                                writer.WriteString("units", subcomponents[1]);
                                writer.WriteString("units" + CODE_SUFFIX, subcomponents[0]);
                                writer.WriteString("units" + CODE_SYSTEM_SUFFIX, subcomponents[2]);
                            }

                            writer.WriteEndObject();
                        }
                        if (labSegment.Length > 14 && !string.IsNullOrEmpty(labSegment[14]))
                        {
                            writer.WriteString(Common.FormatDataElementName("Specimen Description"), labSegment[14]);
                        }
                        if (labSegment.Length > 17 && !string.IsNullOrEmpty(labSegment[17]))
                        {
                            writer.WriteString(Common.FormatDataElementName("Specimen Collection Date/Time"), Common.ConvertHl7DateTimeToUniversalTime(labSegment[17]));
                        }
                        if (labSegment.Length > 18 && !string.IsNullOrEmpty(labSegment[18]))
                        {
                            writer.WriteString(Common.FormatDataElementName("Specimen Received Date/Time"), Common.ConvertHl7DateTimeToUniversalTime(labSegment[18]));
                        }

                        writer.WriteEndObject();
                    }
                    
                    writer.WriteEndArray(); // end write specimens
                }
            }

            writer.WriteEndObject();
            writer.WriteEndArray();

            return messages;
        }

        private List<ProcessResultMessage> WriteGenericCodedData(Utf8JsonWriter writer, string fieldName, string[] components)
        {
            List<ProcessResultMessage> messages = new List<ProcessResultMessage>(1);

            fieldName = Common.FormatDataElementName(fieldName);

            if (components.Length >= 2)
            {
                writer.WriteString(fieldName, components[1]);
            }
            writer.WriteString(fieldName + CODE_SUFFIX, components[0]);
            if (components.Length >= 3)
            {
                writer.WriteString(fieldName + CODE_SYSTEM_SUFFIX, components[2]);
            }
            if (components.Length == 6)
            {
                writer.WriteString(fieldName + ALT_SUFFIX, components[4]);
                writer.WriteString(fieldName + ALT_SUFFIX + CODE_SUFFIX, components[3]);
                writer.WriteString(fieldName + ALT_SUFFIX + CODE_SYSTEM_SUFFIX, components[5]);
            }
            if (components.Length >= 7)
            {
                writer.WriteString(fieldName + "__coding_system_version_id", components[7]);
            }
            if (components.Length >= 8)
            {
                writer.WriteString(fieldName + "__alt_coding_system_version_id", components[6]);
            }
            if (components.Length >= 9)
            { 
                writer.WriteString(fieldName + "__original_text", components[8]);
            }

            return messages;
        }

        private List<ProcessResultMessage> WriteNK1Segment(Utf8JsonWriter writer, string[] nk1fields)
        {
            List<ProcessResultMessage> messages = new List<ProcessResultMessage>();

            writer.WriteStartObject();

            if (nk1fields.Length >= 2 && !string.IsNullOrEmpty(nk1fields[1]))
            {
                writer.WriteString("set_id", nk1fields[1]);
            }
            if (nk1fields.Length >= 3 && !string.IsNullOrEmpty(nk1fields[2]))
            {
                writer.WriteString("nk_name", nk1fields[2]);
            }
            if (nk1fields.Length >= 4 && !string.IsNullOrEmpty(nk1fields[3]))
            {
                var components = nk1fields[3].Split('^');
                WriteGenericCodedData(writer, "relationship", components);
            }
            if (nk1fields.Length >= 15 && !string.IsNullOrEmpty(nk1fields[14]))
            {
                var components = nk1fields[14].Split('^');
                WriteGenericCodedData(writer, "marital_status", components);
            }
            if (nk1fields.Length >= 17 && !string.IsNullOrEmpty(nk1fields[16]))
            {
                if (Common.TryConvertHl7DateTimeToUniversalTime(nk1fields[16], out DateTimeOffset dt))
                {
                    writer.WriteString("datetime_of_birth", dt.ToString("s"));
                }
            }
            if (nk1fields.Length >= 29 && !string.IsNullOrEmpty(nk1fields[28]))
            {
                string[] repeats = nk1fields[28].Split('~');

                if (repeats.Length > 0)
                {
                    writer.WriteStartArray("ethnic_group");

                    foreach (var repeat in repeats)
                    {
                        writer.WriteStartObject();

                        var components = repeat.Split('^');
                        WriteGenericCodedData(writer, "ethnic_group", components);

                        writer.WriteEndObject();

                        break;
                    }
                    writer.WriteEndArray();
                }
            }
            if (nk1fields.Length >= 5 && !string.IsNullOrEmpty(nk1fields[4]))
            {
                string[] repeats = nk1fields[4].Split('~');

                if (repeats.Length > 0)
                {
                    writer.WriteStartArray("address");
                    foreach (var repeat in repeats)
                    {
                        writer.WriteStartObject();

                        var components = repeat.Split('^');

                        if (components.Length >= 4) writer.WriteString("state_or_province", components[3]);
                        if (components.Length >= 5) writer.WriteString("zip_or_postal_code", components[4]);
                        if (components.Length >= 6) writer.WriteString("country", components[5]);
                        if (components.Length >= 9) writer.WriteString("county_parish_code", components[8]);

                        writer.WriteEndObject();
                    }
                    writer.WriteEndArray();
                }
            }
            if (nk1fields.Length >= 36 && !string.IsNullOrEmpty(nk1fields[35]))
            {
                string[] repeats = nk1fields[35].Split('~');

                if (repeats.Length > 0)
                {
                    writer.WriteStartArray("race");

                    foreach (var repeat in repeats)
                    {
                        writer.WriteStartObject();

                        var components = repeat.Split('^');
                        WriteGenericCodedData(writer, "race", components);

                        writer.WriteEndObject();
                    }
                    writer.WriteEndArray();
                }
            }

            writer.WriteEndObject();

            return messages;
        }
    }
}
