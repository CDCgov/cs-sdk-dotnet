using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CS.Sdk
{
    /// <summary>
    /// Common operations needed for the Case Surveillance utilities library
    /// </summary>
    public static class Common
    {
        /// <summary>
        /// Converts an HL7v2 datetime string to a .NET DateTimeOffset in UTC
        /// </summary>
        /// <remarks>
        /// HL7v2 date times can arrive in YYYY[MM[DD[HH[MM[SS[.S[S[S[S]]]]]]]]][+/-ZZZZ] format
        /// </remarks>
        /// <param name="hl7v2dateTime">HL7v2 hl7v2dateTime or timeStamp string</param>
        /// <returns>.NET DateTimeOffset in UTC</returns>
        public static DateTimeOffset ConvertHl7DateTimeToUniversalTime(string hl7v2dateTime)
        {
            if (hl7v2dateTime.Length < 8)
            {
                return new DateTime(); // not a valid hl7v2dateTime, so don't attempt to convert
            }

            DateTimeOffset dateTime;
            char separator = '-';

            if (hl7v2dateTime.IndexOf('+') > 0)
            {
                separator = '+';
            }

            string[] parts = hl7v2dateTime.Split(separator);

            string datetime = parts[0];
            string timezoneoffset = parts.Length > 1 ? parts[1] : string.Empty;

            string yearStr = datetime.Substring(0, 4);
            string monthStr = datetime.Length > 4 ? datetime.Substring(4, 2) : "00";
            string dayStr = datetime.Length > 6 ? datetime.Substring(6, 2) : "00";

            string hourStr = datetime.Length > 8 ? datetime.Substring(8, 2) : "00";
            string minStr = datetime.Length > 10 ? datetime.Substring(10, 2) : "00";
            string secStr = datetime.Length > 12 ? datetime.Substring(12, 2) : "00";

            int.TryParse(yearStr, out int year);
            int.TryParse(monthStr, out int month);
            int.TryParse(dayStr, out int day);
            int.TryParse(hourStr, out int hour);
            int.TryParse(minStr, out int minute);
            int.TryParse(secStr, out int second);

            TimeSpan offset = new TimeSpan();

            if (!string.IsNullOrEmpty(timezoneoffset))
            {
                int offsetHours = int.Parse(timezoneoffset.Substring(0, 2));
                int offsetMinutes = int.Parse(timezoneoffset.Substring(2, 2));

                if (separator == '+')
                {
                    offset = new TimeSpan(offsetHours, offsetMinutes, 0);
                }
                else if (separator == '-')
                {
                    offset = new TimeSpan(-(offsetHours), -(offsetMinutes), 0);
                }
            }

            dateTime = new DateTimeOffset(year, month, day, hour, minute, second, offset);
            return dateTime.ToUniversalTime();
        }

        /// <summary>
        /// Removes HL7v2 escape sequences by converting them into their intended format. For example, 
        /// "Parker \T\ Sons" should be changed to "Parker & Sons".
        /// </summary>
        /// <param name="hl7v2content">HL7v2 content that contains escape sequence(s)</param>
        /// <returns>String that contains the unescaped character sequence</returns>
        public static string UnescapeHL7v2Text(string hl7v2content)
        {
            string unescapedContent = hl7v2content
                .Replace(@"\F\", "|", StringComparison.Ordinal)
                .Replace(@"\T\", "&", StringComparison.Ordinal)
                .Replace(@"\R\", "~", StringComparison.Ordinal)
                .Replace(@"\S\", "^", StringComparison.Ordinal)
                .Replace(@"\E\", @"\", StringComparison.Ordinal);
            return unescapedContent;
        }

        /// <summary>
        /// Converts an HL7v2 datetime string to a .NET DateTimeOffset in UTC
        /// </summary>
        /// <remarks>
        /// HL7v2 date times can arrive in YYYY[MM[DD[HH[MM[SS[.S[S[S[S]]]]]]]]][+/-ZZZZ] format.
        /// Note that a valid HL7v2 date can be just "2016" given the format string.
        /// </remarks>
        /// <param name="hl7v2dateTime">HL7v2 hl7v2dateTime or timeStamp string</param>
        /// <param name="dateTime">The converted datetime as a DateTimeOffset</param>
        /// <returns>bool; whether the conversion operation was a success</returns>
        public static bool TryConvertHl7DateTimeToUniversalTime(string hl7v2dateTime, out DateTimeOffset dateTime)
        {
            if (hl7v2dateTime.Length <= 7 || hl7v2dateTime.StartsWith("9", StringComparison.OrdinalIgnoreCase))
            {
                dateTime = DateTime.MinValue;
                return false;
            }

            try
            {
                dateTime = ConvertHl7DateTimeToUniversalTime(hl7v2dateTime);
                return true;
            }
            catch
            {
                dateTime = DateTime.MinValue;
                return false;
            }
        }

        /// <summary>
        /// Formats a data element name into something suitable for use as a Json property name
        /// </summary>
        /// <param name="dataElementName">The name of a data element</param>
        /// <returns>The name of the data element formatted for use as a Json property</returns>
        public static string FormatDataElementName(string dataElementName)
        {
            StringBuilder sb = new StringBuilder();
            char prev = ' ';
            for (int i = 0; i < dataElementName.Length; i++)
            {
                char c = dataElementName[i];

                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z'))
                {
                    sb.Append(c);
                    prev = c;
                }
                else if (prev != '_' && (c == '-' || c == ' '))
                {
                    sb.Append('_');
                    prev = '_';
                }
                else if (c == '&')
                {
                    sb.Append("and");
                    prev = ' ';
                }
                if (c == '_' && prev != '_')
                {
                    sb.Append(c);
                    prev = c;
                }
            }

            string result = sb.ToString().ToLower().Trim('_');
            return result;
        }

        /// <summary>
        /// Decodes a base64-encoded string
        /// </summary>
        /// <param name="base64EncodedData">Base64 encoded string</param>
        /// <returns>Decoded string</returns>
        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }

        /// <summary>
        /// Encodes a string to base64
        /// </summary>
        /// <param name="plainText">Plaintext string to encode</param>
        /// <returns>Base64-encoded string</returns>
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        /// <summary>
        /// Flattens a hierarchial Json object
        /// </summary>
        /// <param name="json">Json object to flatten</param>
        /// <returns>Dictionary of key/value pairs representing the flat Json property/value pairs</returns>
        public static Dictionary<string, string> FlattenJson(string json)
        {
            JObject jsonObject = JObject.Parse(json);
            IEnumerable<JToken> jTokens = jsonObject.Descendants().Where(p => !p.HasValues);
            Dictionary<string, string> results = jTokens.Aggregate(new Dictionary<string, string>(), (properties, jToken) =>
            {
                properties.Add(jToken.Path, jToken.ToString());
                return properties;
            });

            return results;
        }
    }
}