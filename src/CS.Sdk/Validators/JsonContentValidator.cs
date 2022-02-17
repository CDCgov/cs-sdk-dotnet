using System;
using System.Collections.Generic;
using System.Text.Json;
using CS.Mmg;
using CS.Sdk.Services;

namespace CS.Sdk.Validators
{
    /// <summary>
    /// Validates a Json-formatted case notification message against a message mapping guide
    /// </summary>
    public sealed class JsonContentValidator : IContentValidator
    {
        private readonly IVocabularyService _vocabService;
        private readonly IMmgService _mmgService;
        private const string MESSAGE_PROFILE_IDENTIFIER = "message_profile_identifier";
        private const string MESSAGE_PROFILE_IDENTIFIERS = "message_profile_identifiers";
        private readonly JsonSerializerOptions _options = new JsonSerializerOptions()
        {
            WriteIndented = true,
            AllowTrailingCommas = true
        };

        /// <summary>
        /// Constructor
        /// </summary>
        public JsonContentValidator()
        {
            _vocabService = new InMemoryVocabularyService();
            _mmgService = new InMemoryMmgService();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="vocabService">Service for retrieving vocabulary</param>
        /// <param name="mmgService">Service for retrieving message mapping guides (MMGs)</param>
        public JsonContentValidator(IVocabularyService vocabService, IMmgService mmgService)
        {
            _vocabService = vocabService;
            _mmgService = mmgService;
        }

        /// <summary>
        /// Validates a Json object against a message mapping guide
        /// </summary>
        /// <param name="json">The Json object to validate</param>
        /// <param name="transactionId">An optional transaction ID that can be used for tracking data lineage across stages of a data processing pipeline</param>
        /// <returns>ValidationResult; an object with various properties that explains whether the message is valid or invalid and contains validation metadata</returns>
        public ValidationResult Validate(string json, string transactionId = "")
        {
            ValidationResult result = new ValidationResult();
            List<ValidationMessage> validationMessages = new List<ValidationMessage>();
            Dictionary<string, object> properties = new Dictionary<string, object>(0);
            string messageProfileIdentifier = string.Empty;
            List<string> profiles = new List<string>(3);

            var sw = new System.Diagnostics.Stopwatch();
            sw.Start();

            #region Parse Json object into C# dictionary

            // Step 1, parse the Json payload into a dictionary so we can iterate over it
            try
            {
                properties = JsonSerializer.Deserialize<Dictionary<string, object>>(json, _options);
            }
            catch (Exception ex)
            {
                validationMessages.Add(new ValidationMessage(ex));
                result.ValidationMessages = validationMessages;
                return result;
            }
            #endregion

            #region Get message profile identifiers
            // Step 2, check to see if we have an MMG profile name in the Dictionary. This is
            // needed so we can hit the MMGAT API to retrieve the machine-readable MMG.
            if (properties.ContainsKey(MESSAGE_PROFILE_IDENTIFIER) && properties.ContainsKey(MESSAGE_PROFILE_IDENTIFIERS))
            {
                messageProfileIdentifier = ((JsonElement)properties[MESSAGE_PROFILE_IDENTIFIER]).GetString();
                foreach (JsonElement identifier in ((JsonElement)properties[MESSAGE_PROFILE_IDENTIFIERS]).EnumerateArray())
                {
                    profiles.Add(identifier.GetString());
                }
            }
            else
            {
                // If there's no Json property for the MMG profile name in the payload then we can't validate anything
                validationMessages.Add(
                    new ValidationMessage(
                        Severity.Error,
                        ValidationMessageType.Structural,
                        $"Message profile identifier property not found; expected to find a Json property named '" + MESSAGE_PROFILE_IDENTIFIER + "' but one was not present",
                        $""));

                result.ValidationMessages = validationMessages;
                result.Elapsed = sw.Elapsed.TotalMilliseconds;

                return result;
            }

            if (string.IsNullOrWhiteSpace(messageProfileIdentifier) || profiles.Count == 0)
            {
                // If the Json property exists but there's nothing in it, that's a different problem
                validationMessages.Add(
                    new ValidationMessage(
                        Severity.Error,
                        ValidationMessageType.Content,
                        $"Message profile identifier value is blank",
                        $""));
                result.ValidationMessages = validationMessages;
                return result;
            }

            result.Profile = messageProfileIdentifier;
            #endregion

            #region Get condition code
            // Step 3, get the condition code
            if (properties.ContainsKey("condition_code") && properties.ContainsKey("condition_code__code"))
            {
                result.Condition = ((JsonElement)properties["condition_code"]).GetString();
                result.ConditionCode = ((JsonElement)properties["condition_code__code"]).GetString();
            }
            else
            {
                validationMessages.Add(
                    new ValidationMessage(
                        Severity.Error,
                        ValidationMessageType.Structural,
                        $"Condition code Json properties were not found",
                        $""));
                result.ValidationMessages = validationMessages;
                return result;
            }
            #endregion

            #region Get other metadata
            if (properties.ContainsKey("national_reporting_jurisdiction"))
            {
                result.NationalReportingJurisdiction = ((JsonElement)properties["national_reporting_jurisdiction"]).GetString();
            }
            if (properties.ContainsKey("unique_case_id"))
            {
                result.UniqueCaseId = ((JsonElement)properties["unique_case_id"]).GetString();
            }
            if (properties.ContainsKey("local_record_id"))
            {
                result.LocalRecordId = ((JsonElement)properties["local_record_id"]).GetString();
            }
            #endregion

            #region Get MMG C# model object
            if (false == TryGetMessageMappingGuide(string.Join("^", profiles), result.ConditionCode, out MessageMappingGuide messageMappingGuide))
            {
                validationMessages.Add(
                    new ValidationMessage(
                        Severity.Error,
                        ValidationMessageType.Content,
                        "Message mapping guide for " + messageProfileIdentifier + " could not be found by the MMG API",
                        $""));
                result.ValidationMessages = validationMessages;
                return result;
            }
            #endregion

            #region Check constant fields that we know came from HL7v2 but aren't necessarily in the MMG
            if (!properties.ContainsKey("datetime_of_message"))
            {
                validationMessages.Add(
                     new ValidationMessage(
                         Severity.Error,
                         ValidationMessageType.Structural,
                         "Date/time of message was not found",
                         $""));
                result.ValidationMessages = validationMessages;
                return result;
            }

            object dateTimeObj = properties["datetime_of_message"];
            JsonElement dateTimeElement = (JsonElement)dateTimeObj;

            if (dateTimeElement.ValueKind != JsonValueKind.String)
            {
                validationMessages.Add(
                     new ValidationMessage(
                         Severity.Error,
                         ValidationMessageType.Content,
                         "Date/time of message is expeted to be String, but is " + ((JsonElement)properties["datetime_of_message"]).ValueKind,
                         $""));
                result.ValidationMessages = validationMessages;
                return result;
            }
            else if (string.IsNullOrWhiteSpace(dateTimeElement.GetString()))
            {
                validationMessages.Add(
                     new ValidationMessage(
                         Severity.Error,
                         ValidationMessageType.Content,
                         "Date/time of message is blank",
                         $""));
                result.ValidationMessages = validationMessages;
                return result;
            }
            else if (!dateTimeElement.TryGetDateTimeOffset(out _))
            {
                validationMessages.Add(
                     new ValidationMessage(
                         Severity.Error,
                         ValidationMessageType.Content,
                         "Date/time of message is invalid",
                         $""));
                result.ValidationMessages = validationMessages;
                return result;
            }
            #endregion

            #region Validate HL7v2 message content

            foreach (DataElement element in messageMappingGuide.Elements)
            {
                string formattedName = Common.FormatDataElementName(element.Name);

                bool isPresent = properties.ContainsKey(formattedName);

                // check required fields
                if (element.Priority == Priority.Required)
                {
                    if (!isPresent)
                    {
                        validationMessages.Add(
                         new ValidationMessage(
                             Severity.Error,
                             ValidationMessageType.Structural,
                             "Required data element '" + element.Name + "' was not found",
                             $""));
                    }
                    else if (string.IsNullOrWhiteSpace(((JsonElement)properties[formattedName]).GetRawText()))
                    {
                        validationMessages.Add(
                         new ValidationMessage(
                             Severity.Error,
                             ValidationMessageType.Content,
                             "Required data element '" + element.Name + "' is null or empty",
                             $""));
                    }
                }

                if (isPresent)
                {
                    JsonElement property = (JsonElement)properties[formattedName];
                    switch(element.DataType)
                    {
                        case Mmg.DataType.LongText:
                        case Mmg.DataType.Text:
                        case Mmg.DataType.FormattedText:
                            if (property.ValueKind != JsonValueKind.String)
                            {
                                validationMessages.Add(
                                    new ValidationMessage(
                                         Severity.Error,
                                         ValidationMessageType.Content,
                                         "Data element '" + element.Name + "' is supposed to be text but is type " + property.ValueKind,
                                         $""));
                            }
                            break;
                        case Mmg.DataType.DateTime when property.ValueKind != JsonValueKind.String:
                        case Mmg.DataType.Date when property.ValueKind != JsonValueKind.String:
                            validationMessages.Add(
                                new ValidationMessage(
                                    Severity.Error,
                                    ValidationMessageType.Content,
                                    "Data element '" + element.Name + "' is supposed to be a date but is type " + property.ValueKind,
                                    $""));
                            break;
                        case Mmg.DataType.DateTime:
                        case Mmg.DataType.Date:
                            bool isValidDate = property.TryGetDateTimeOffset(out _) || (int.TryParse(property.GetString(), out int dateInteger) && dateInteger >= 1800 && dateInteger <= 22001231) || (element.Priority != Priority.Required && dateInteger == 99999999); // to handle dates with only a year+month or only a year
                            if (!isValidDate)
                            {
                                validationMessages.Add(
                                    new ValidationMessage(
                                         Severity.Error,
                                         ValidationMessageType.Content,
                                         "Data element '" + element.Name + "' has an invalid date value",
                                         $""));
                            }
                            break;
                        case Mmg.DataType.Numeric:
                            bool isValidNumber = property.ValueKind == JsonValueKind.Number && property.TryGetDouble(out double _);
                            if (!isValidNumber)
                            {
                                validationMessages.Add(
                                    new ValidationMessage(
                                         Severity.Error,
                                         ValidationMessageType.Content,
                                         "Data element '" + element.Name + "' is an invalid floating point",
                                         $""));
                            }
                            break;
                        case Mmg.DataType.Integer:
                            bool isValidInteger = property.ValueKind == JsonValueKind.Number && property.TryGetInt64(out long _);
                            if (!isValidInteger)
                            {
                                validationMessages.Add(
                                    new ValidationMessage(
                                         Severity.Error,
                                         ValidationMessageType.Content,
                                         "Data element '" + element.Name + "' is an invalid integer",
                                         $""));
                            }
                            break;
                        case Mmg.DataType.Coded when (element.Repetitions.HasValue && element.Repetitions.Value > 1) || (element.IsRepeat):
                            foreach (JsonElement jsonArrayItem in property.EnumerateArray())
                            {
                                if (jsonArrayItem.ValueKind == JsonValueKind.Object)
                                {
                                    Dictionary<string, object> keyValuePairs = new Dictionary<string, object>();

                                    foreach (JsonProperty jsonProperty in jsonArrayItem.EnumerateObject())
                                    {
                                        keyValuePairs.Add(jsonProperty.Name, jsonProperty.Value);
                                    }

                                    validationMessages.AddRange(ValidateCodedElement(keyValuePairs, formattedName, element));
                                }
                            }
                            break;
                        case Mmg.DataType.Coded when element.Repetitions.HasValue == false || element.Repetitions.Value <= 1:
                            validationMessages.AddRange(ValidateCodedElement(properties, formattedName, element));
                            break;
                    }
                }
            }
            #endregion

            #region Validate no extraneous fields are present
            // make sure that there are no data included in the HL7 message's OBX segments that are not defined in the message mapping guide
            //var extraneousDataMessages = ValidateExtraneousOBXSegments(message, messageMappingGuide);
            //validationMessages.AddRange(extraneousDataMessages);
            #endregion

            sw.Stop();

            result.ValidationMessages = validationMessages;
            result.Elapsed = sw.Elapsed.TotalMilliseconds;
            result.Created = DateTimeOffset.Now;
            result.TransactionId = transactionId;

            return result;
        }

        private List<ValidationMessage> BuildInvalidVocabularyMessages(VocabularyValidationResult vocabularyResult, DataElement element)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();

            var mapping = element.Mappings.Hl7v251;
            string valueSetCode = element.ValueSetCode;

            if (!vocabularyResult.IsCodeValid)
            {
                ValidationMessage illegalConceptCodeMessage = new ValidationMessage(
                    severity: Severity.Warning,
                    messageType: ValidationMessageType.Vocabulary,
                    content: $"Data element '{element.Name}' with identifier '{mapping.Identifier}' is a coded element associated with the value set '{valueSetCode}'. However, the concept code '{vocabularyResult.ConceptCode}' in the message was not found as a valid concept for this value set",
                    path: string.Empty);

                illegalConceptCodeMessage.DataElementId = element.Id.ToString();
                illegalConceptCodeMessage.ValueSetCode = valueSetCode;

                messages.Add(illegalConceptCodeMessage);
            }
            if (vocabularyResult.IsCodeValid && !vocabularyResult.IsNameValid)
            {
                ValidationMessage illegalConceptNameMessage = new ValidationMessage(
                    severity: Severity.Warning,
                    messageType: ValidationMessageType.Vocabulary,
                    content: $"Data element '{element.Name}' with identifier '{mapping.Identifier}' is a coded element associated with the value set '{valueSetCode}'. However, the concept name '{vocabularyResult.ConceptName}' in the message was not found as a valid concept name for the code '{vocabularyResult.ConceptCode}' in this value set",
                    path: string.Empty);

                illegalConceptNameMessage.DataElementId = element.Id.ToString();
                illegalConceptNameMessage.ValueSetCode = valueSetCode;

                messages.Add(illegalConceptNameMessage);
            }

            return messages;
        }

        private List<ValidationMessage> ValidateCodedElement(Dictionary<string, object> properties, string formattedName, DataElement element)
        {
            List<ValidationMessage> validationMessages = new List<ValidationMessage>();

            bool allPropertiesExist = true;
            if (!properties.ContainsKey(formattedName + "__code"))
            {
                validationMessages.Add(
                    new ValidationMessage(
                         Severity.Error,
                         ValidationMessageType.Structural,
                         "Data element '" + element.Name + "' is a coded element but no code property was found in the message",
                         $""));
                allPropertiesExist = false;
            }
            if (!properties.ContainsKey(formattedName + "__code_system"))
            {
                validationMessages.Add(
                    new ValidationMessage(
                         Severity.Error,
                         ValidationMessageType.Structural,
                         "Data element '" + element.Name + "' is a coded element but no code system property was found in the message",
                         $""));
                allPropertiesExist = false;
            }

            if (!allPropertiesExist)
            {
                return validationMessages;
            }

            try
            {
                JsonElement property = (JsonElement)properties[formattedName];
                JsonElement propertyCode = (JsonElement)properties[formattedName + "__code"];
                JsonElement propertyCodeSystem = (JsonElement)properties[formattedName + "__code_system"];

                string code = propertyCode.GetString();
                string codeDescription = property.GetString();
                string codeSystem = propertyCodeSystem.GetString();

                VocabularyValidationResult vocabularyResult = _vocabService.IsValid(code, codeDescription, codeSystem, element.ValueSetCode);

                if (!vocabularyResult.IsCodeValid)
                {
                    List<ValidationMessage> vocabularyMessages = BuildInvalidVocabularyMessages(
                            vocabularyResult,
                            element);

                    validationMessages.AddRange(vocabularyMessages);
                }

            }
            catch (Exception ex)
            {
                validationMessages.Add(new ValidationMessage(ex));
            }

            return validationMessages;
        }

        private bool TryGetMessageMappingGuide(string profile, string conditionCode, out MessageMappingGuide messageMappingGuide)
        {
            try
            {
                messageMappingGuide = _mmgService.Get(profile, conditionCode);
                if (messageMappingGuide == null)
                {
                    throw new InvalidOperationException("Message mapping guide for " + profile + " could not be found by the MMG API");
                }
                return true;
            }
            catch
            {
                messageMappingGuide = null;
                return false;
            }
        }
    }
}
