using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace CS.Sdk.Converters
{
    public sealed class FixedWidthToJsonConverter : IConverter
    {
        private readonly JsonWriterOptions _jsonWriterOptions = new JsonWriterOptions
        {
            Indented = true
        };

        private const string CODE_SUFFIX = "__code";
        private const string CODE_SYSTEM_SUFFIX = "__code_system";
        private const string ALT_SUFFIX = "__alt";
        private const string SOURCE_FORMAT = "NETSS";

        private const string RACE_NAME = "race_category";
        private const string RACE_CODE = RACE_NAME + CODE_SUFFIX;
        private const string RACE_CODE_SYSTEM = RACE_NAME + CODE_SYSTEM_SUFFIX;

        private const string ETH_GROUP = "ethnic_group";
        private const string ETH_GROUP_CODE = ETH_GROUP + CODE_SUFFIX;
        private const string ETH_GROUP_CODE_SYSTEM = ETH_GROUP + CODE_SYSTEM_SUFFIX;

        private const string AGE_UNIT = "age_unit_at_case_investigation";
        private const string AGE_UNIT_CODE = AGE_UNIT + CODE_SUFFIX;
        private const string AGE_UNIT_CODE_SYSTEM = AGE_UNIT + CODE_SYSTEM_SUFFIX;

        private const string SUBJECTS_SEX = "subjects_sex__code";

        private const string NETSS_RECORD_TYPE_DESCRIPTION = "netss_record_type_description";

        /// <summary>
        /// Converts a fixed-width text file into a Json structure
        /// </summary>
        /// <param name="fixedWidthMessage">The fixed-width message to transform</param>
        /// <param name="transactionId">Optional transaction ID. Leaving this empty will result in a warning.</param>
        /// <returns>A conversion result containing the resulting Json and some metadata about the conversion</returns>
        public ConversionResult Convert(string message, string transactionId = "")
        {
            return ConvertWithSpan(message, transactionId);
        }

        private ConversionResult ConvertWithSpan(string fixedWidthMessage, string transactionId = "")
        {
            string conditionCode = string.Empty;
            string condition = string.Empty;
            string json = "";

            #region Some constants
            ReadOnlySpan<char> race_aian = new ReadOnlySpan<char>("American Indian or Alaska Native".ToCharArray());
            ReadOnlySpan<char> race_aian_code = new ReadOnlySpan<char>("1002-5".ToCharArray());

            ReadOnlySpan<char> race_asian = new ReadOnlySpan<char>("Asian".ToCharArray());
            ReadOnlySpan<char> race_asian_code = new ReadOnlySpan<char>("2028-9".ToCharArray());

            ReadOnlySpan<char> race_black = new ReadOnlySpan<char>("Black or African American".ToCharArray());
            ReadOnlySpan<char> race_black_code = new ReadOnlySpan<char>("2054-5".ToCharArray());

            ReadOnlySpan<char> race_white = new ReadOnlySpan<char>("White".ToCharArray());
            ReadOnlySpan<char> race_white_code = new ReadOnlySpan<char>("2106-3".ToCharArray());

            ReadOnlySpan<char> race_other = new ReadOnlySpan<char>("Other Race".ToCharArray());
            ReadOnlySpan<char> race_other_code = new ReadOnlySpan<char>("2131-1".ToCharArray());

            ReadOnlySpan<char> race_unknown = new ReadOnlySpan<char>("Unknown".ToCharArray());
            ReadOnlySpan<char> race_unknown_code = new ReadOnlySpan<char>("UNK".ToCharArray());

            ReadOnlySpan<char> race_code_system = new ReadOnlySpan<char>("CDCREC".ToCharArray());

            #endregion

            var sw = new System.Diagnostics.Stopwatch();
            sw.Start();

            List<ProcessResultMessage> messages = new List<ProcessResultMessage>();

            ReadOnlySpan<char> message = fixedWidthMessage.AsSpan();

            int offset = 0;
            int recordNumber = 1;

            using (var stream = new MemoryStream())
            {
                using (var writer = new Utf8JsonWriter(stream, _jsonWriterOptions))
                {
                    writer.WriteStartArray();

                    for (int i = 0; i <= message.Length; i++)
                    {
                        ReadOnlySpan<char> currentCharacter =
                            i == message.Length
                                ? new ReadOnlySpan<char>(new char[] { '\n' })
                                : message.Slice(i, 1);

                        if (currentCharacter[0] == '\n' || currentCharacter[0] == '\r')
                        {
                            int recordLength = (i - offset);

                            if (recordLength > 0)
                            {
                                writer.WriteStartObject();

                                writer.WriteString("source_format", SOURCE_FORMAT);
                                writer.WriteString("transaction_id", transactionId);

                                ReadOnlySpan<char> recordTypeSpan = message.Slice(offset, 1);

                                writer.WriteString("netss_record_type", recordTypeSpan);

                                switch (recordTypeSpan[0])
                                {
                                    case 'M':
                                        writer.WriteString(NETSS_RECORD_TYPE_DESCRIPTION, "Case");
                                        break;
                                    case 'D':
                                        writer.WriteString(NETSS_RECORD_TYPE_DESCRIPTION, "Deletion");
                                        break;
                                    case 'V':
                                        writer.WriteString(NETSS_RECORD_TYPE_DESCRIPTION, "Verification");
                                        break;
                                    case 'S':
                                        writer.WriteString(NETSS_RECORD_TYPE_DESCRIPTION, "Summary");
                                        break;
                                    default:
                                        writer.WriteString(NETSS_RECORD_TYPE_DESCRIPTION, "Unknown");
                                        break;
                                }

                                writer.WriteNumber("netss_record_number_in_payload", recordNumber);

                                ReadOnlySpan<char> stateSpan = message.Slice(offset + 2, 2);
                                ReadOnlySpan<char> caseIdSpan = message.Slice(offset + 6, 6);
                                ReadOnlySpan<char> weekSpan = message.Slice(offset + 15, 2);
                                ReadOnlySpan<char> eventDateSpan = message.Slice(offset + 45, 6);
                                ReadOnlySpan<char> siteCodeSpan = message.Slice(offset + 12, 3);
                                ReadOnlySpan<char> yearSpan = message.Slice(offset + 4, 2);

                                #region Build unique case ID
                                writer.WriteString("netss_unique_case_id", caseIdSpan.ToString() + "_" + stateSpan.ToString() + "_" + siteCodeSpan.ToString() + "_" + yearSpan.ToString());
                                #endregion // Build unique case ID

                                writer.WriteString("netss_update", message.Slice(offset + 1, 1));
                                writer.WriteString("netss_state", stateSpan);
                                writer.WriteString("netss_year", yearSpan);
                                writer.WriteString("netss_case_report_id", caseIdSpan);
                                writer.WriteString("netss_site_code", siteCodeSpan);

                                #region MMWR Week

                                writer.WriteString("netss_week", weekSpan);

                                if (int.TryParse(weekSpan, out int mmwrWeek))
                                {
                                    writer.WriteNumber("mmwr_week", mmwrWeek);
                                }
                                #endregion

                                if (recordLength >= 22)
                                {
                                    writer.WriteString("netss_event_code", message.Slice(offset + 17, 5));
                                }

                                #region Count
                                if (recordLength >= 27)
                                {
                                    ReadOnlySpan<char> countSpan = message.Slice(offset + 22, 5);
                                    writer.WriteString("netss_count", countSpan);
                                    if (int.TryParse(countSpan, out int count))
                                    {
                                        writer.WriteNumber("count", count);
                                    }
                                }
                                #endregion

                                if (recordLength >= 30)
                                {
                                    writer.WriteString("netss_county_code", message.Slice(offset + 27, 3));
                                }

                                #region Birthdate
                                if (recordLength >= 38)
                                {
                                    ReadOnlySpan<char> birthDateSpan = message.Slice(offset + 30, 8);
                                    writer.WriteString("netss_birth_date", birthDateSpan);

                                    if (DateTime.TryParseExact(birthDateSpan,
                                        "yyyyMMdd",
                                        System.Globalization.CultureInfo.InvariantCulture,
                                        System.Globalization.DateTimeStyles.None,
                                            out DateTime birthDate))
                                    {
                                        writer.WriteString("birth_date", birthDate);
                                    }
                                }
                                #endregion

                                #region Age
                                if (recordLength >= 41)
                                {
                                    ReadOnlySpan<char> ageSpan = message.Slice(offset + 38, 3);
                                    writer.WriteString("netss_age", ageSpan);

                                    if (int.TryParse(ageSpan, out int age))
                                    {
                                        writer.WriteNumber("age_at_case_investigation", age);
                                    }
                                }
                                #endregion

                                #region Age type
                                if (recordLength >= 42)
                                {
                                    ReadOnlySpan<char> ageTypeSpan = message.Slice(offset + 41, 1);

                                    writer.WriteString("netss_age_type", ageTypeSpan);

                                    string ageUnits = string.Empty;
                                    string ageUnitsCode = string.Empty;
                                    string ageUnitsCodeSystem = string.Empty;

                                    if (ageTypeSpan[0] == '0')
                                    {
                                        ageUnits = "year [time]";
                                        ageUnitsCode = "a";
                                        ageUnitsCodeSystem = "UCUM";
                                    }
                                    else if (ageTypeSpan[0] == '1')
                                    {
                                        ageUnits = "month [time]";
                                        ageUnitsCode = "mo";
                                        ageUnitsCodeSystem = "UCUM";
                                    }
                                    else if (ageTypeSpan[0] == '2')
                                    {
                                        ageUnits = "week [time]";
                                        ageUnitsCode = "wk";
                                        ageUnitsCodeSystem = "UCUM";
                                    }
                                    else if (ageTypeSpan[0] == '3')
                                    {
                                        ageUnits = "day [time]";
                                        ageUnitsCode = "d";
                                        ageUnitsCodeSystem = "UCUM";
                                    }
                                    else if (ageTypeSpan[0] == '9')
                                    {
                                        ageUnits = "unknown";
                                        ageUnitsCode = "UNK";
                                        ageUnitsCodeSystem = "NULLFL";
                                    }

                                    writer.WriteString(AGE_UNIT, ageUnits);
                                    writer.WriteString(AGE_UNIT_CODE, ageUnitsCode);
                                    writer.WriteString(AGE_UNIT_CODE_SYSTEM, ageUnitsCodeSystem);
                                }
                                #endregion

                                #region Sex
                                if (recordLength >= 43)
                                {
                                    ReadOnlySpan<char> sexSpan = message.Slice(offset + 42, 1);

                                    writer.WriteString("netss_sex", sexSpan);

                                    if (sexSpan[0] == '1')
                                    {
                                        writer.WriteString(SUBJECTS_SEX, "M");
                                    }
                                    else if (sexSpan[0] == '2')
                                    {
                                        writer.WriteString(SUBJECTS_SEX, "F");
                                    }
                                    else if (sexSpan[0] == '9')
                                    {
                                        writer.WriteString(SUBJECTS_SEX, "U");
                                    }
                                }
                                #endregion

                                #region Race
                                if (recordLength >= 44)
                                {
                                    ReadOnlySpan<char> raceSpan = message.Slice(offset + 43, 1);

                                    writer.WriteString("netss_race", raceSpan);

                                    #region Race Conversion

                                    writer.WriteStartArray("race_category");
                                    writer.WriteStartObject();

                                    if (raceSpan[0] == '1')
                                    {
                                        writer.WriteString(RACE_NAME, race_aian);
                                        writer.WriteString(RACE_CODE, race_aian_code);
                                        writer.WriteString(RACE_CODE_SYSTEM, race_code_system);
                                    }
                                    else if (raceSpan[0] == '2')
                                    {
                                        // This could be either '2028-9' or '2076-8'. The first was chosen for prototyping purposes. Revisiting this is recommended.
                                        writer.WriteString(RACE_NAME, race_asian);
                                        writer.WriteString(RACE_CODE, race_asian_code);
                                        writer.WriteString(RACE_CODE_SYSTEM, race_code_system);
                                    }
                                    else if (raceSpan[0] == '3')
                                    {
                                        writer.WriteString(RACE_NAME, race_black);
                                        writer.WriteString(RACE_CODE, race_black_code);
                                        writer.WriteString(RACE_CODE_SYSTEM, race_code_system);
                                    }
                                    else if (raceSpan[0] == '5')
                                    {
                                        writer.WriteString(RACE_NAME, race_white);
                                        writer.WriteString(RACE_CODE, race_white_code);
                                        writer.WriteString(RACE_CODE_SYSTEM, race_code_system);
                                    }
                                    else if (raceSpan[0] == '8')
                                    {
                                        writer.WriteString(RACE_NAME, race_other);
                                        writer.WriteString(RACE_CODE, race_other_code);
                                        writer.WriteString(RACE_CODE_SYSTEM, race_code_system);
                                    }
                                    else if (raceSpan[0] == '9')
                                    {
                                        // There is no code for an unknown race - recommend follow-up on how to handle this
                                        writer.WriteString(RACE_NAME, race_unknown);
                                        writer.WriteString(RACE_CODE, race_unknown_code);
                                        writer.WriteString(RACE_CODE_SYSTEM, "NULLFL");
                                    }

                                    writer.WriteEndObject();
                                    writer.WriteEndArray();

                                    #endregion
                                }
                                #endregion

                                #region Ethnicity
                                if (recordLength >= 45)
                                {
                                    ReadOnlySpan<char> ethnicitySpan = message.Slice(offset + 44, 1);
                                    writer.WriteString("netss_ethnicity", ethnicitySpan);

                                    writer.WriteStartArray(ETH_GROUP);
                                    writer.WriteStartObject();

                                    switch (ethnicitySpan[0])
                                    {
                                        case '1':
                                            // hisp
                                            writer.WriteString(ETH_GROUP, "Hispanic or Latino");
                                            writer.WriteString(ETH_GROUP_CODE, "2135-2");
                                            writer.WriteString(ETH_GROUP_CODE_SYSTEM, "CDCREC");
                                            break;
                                        case '2':
                                            // not hisp
                                            writer.WriteString(ETH_GROUP, "Not Hispanic or Latino");
                                            writer.WriteString(ETH_GROUP_CODE, "2186-5");
                                            writer.WriteString(ETH_GROUP_CODE_SYSTEM, "CDCREC");
                                            break;
                                        case '9':
                                            // unknown
                                            writer.WriteString(ETH_GROUP, "unknown");
                                            writer.WriteString(ETH_GROUP_CODE, "UNK");
                                            writer.WriteString(ETH_GROUP_CODE_SYSTEM, "NULLFL");
                                            break;
                                        default:
                                            // invalid
                                            break;
                                    }

                                    writer.WriteEndObject();
                                    writer.WriteEndArray();
                                }
                                #endregion // Ethnicity

                                #region Event date
                                if (recordLength >= 51)
                                {
                                    writer.WriteString("netss_event_date", eventDateSpan);

                                    if (DateTime.TryParseExact(eventDateSpan,
                                        "yyMMdd",
                                        System.Globalization.CultureInfo.InvariantCulture,
                                        System.Globalization.DateTimeStyles.None,
                                    out DateTime eventDate))
                                    {
                                        writer.WriteString("event_date", eventDate);
                                    }
                                }
                                #endregion

                                if (recordLength >= 52)
                                {
                                    ReadOnlySpan<char> eventDateTypeSpan = message.Slice(offset + 51, 1);
                                    writer.WriteString("netss_event_date_type", eventDateTypeSpan);
                                }

                                if (recordLength >= 53)
                                    writer.WriteString("netss_case_status", message.Slice(offset + 52, 1));

                                if (recordLength >= 54)
                                    writer.WriteString("netss_imported", message.Slice(offset + 53, 1));

                                if (recordLength >= 55)
                                    writer.WriteString("netss_outbreak", message.Slice(offset + 54, 1));

                                if (recordLength >= 60)
                                    writer.WriteString("netss_future", message.Slice(offset + 55, 5));

                                writer.WriteEndObject();
                            }

                            offset = i + 1;
                            recordNumber++;
                        }
                    }

                    writer.WriteEndArray();
                }
                json = Encoding.UTF8.GetString(stream.ToArray());
            }

            sw.Stop();

            return new ConversionResult()
            {
                Content = json,
                TransactionId = transactionId,
                Condition = condition,
                ConditionCode = conditionCode,
                ConversionMessages = messages,
                //MessageDateTime = dateTimeOfMessage,
                Created = DateTime.Now,
                Elapsed = sw.Elapsed.TotalMilliseconds
            };
        }
    }
}
