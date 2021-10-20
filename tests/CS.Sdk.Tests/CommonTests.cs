using System;
using System.Collections.Generic;
using Xunit;

namespace CS.Sdk.Tests
{
    public class CommonTests
    {
        [Theory]
        [InlineData("201406301200", "2014-06-30T12:00:00")]
        [InlineData("20140630120030", "2014-06-30T12:00:30")]
        [InlineData("20140630120030.1-0500", "2014-06-30T17:00:30")]
        [InlineData("20140630120030.12-0500", "2014-06-30T17:00:30")]
        [InlineData("20140630120030.12+0500", "2014-06-30T07:00:30")]
        [InlineData("20140630120030.123-0500", "2014-06-30T17:00:30")]
        [InlineData("20140630120030.1234-0500", "2014-06-30T17:00:30")]
        public void Hl7DateToJsonDateConversion(string hl7v2timestamp, string isoDate)
        {
            DateTimeOffset dt = Common.ConvertHl7DateTimeToUniversalTime(hl7v2timestamp);
            string actual = dt.ToUniversalTime().ToString("s");

            Assert.Equal(isoDate, actual);
        }

        [Theory]
        [InlineData("Age at Case Investigation", "age_at_case_investigation")]
        [InlineData("Patient's name", "patients_name")]
        [InlineData("Patient’s name", "patients_name")]
        [InlineData("Patient`s name", "patients_name")]
        [InlineData("Signs & symptoms", "signs_and_symptoms")]
        [InlineData("Problems (TB) - Symptoms", "problems_tb_symptoms")]
        [InlineData("Problems (TB) -- Symptoms", "problems_tb_symptoms")]
        [InlineData("Problems (TB) --- Symptoms", "problems_tb_symptoms")]
        [InlineData(" Problems (TB) - Symptoms", "problems_tb_symptoms")]
        [InlineData("Problems (TB) - Symptoms ", "problems_tb_symptoms")]
        [InlineData(" Problems (TB) - Symptoms ", "problems_tb_symptoms")]
        public void FormatPropertyName(string propertyName, string expected)
        {
            string formattedPropertyName = Common.FormatDataElementName(propertyName);
            Assert.Equal(expected, formattedPropertyName);
        }

        [Theory]
        [InlineData(@"Parker \T\ Sons", "Parker & Sons")]
        [InlineData(@"Parker /T/ Sons", "Parker /T/ Sons")] // the /T/ is not a real escape sequence... \T\ is!
        [InlineData(@"Parker \F\ Sons", "Parker | Sons")]
        [InlineData(@"Parker \R\ Sons", "Parker ~ Sons")]
        [InlineData(@"Parker \S\ Sons", "Parker ^ Sons")]
        [InlineData(@"Parker \E\ Sons", @"Parker \ Sons")]
        public void UnescapeCharacters(string escapedString, string expectedUnescapedString)
        {
            string actualUnescapedString = Common.UnescapeHL7v2Text(escapedString);
            Assert.Equal(expected: expectedUnescapedString, actual: actualUnescapedString);
        }

        [Theory]
        [InlineData("abcd", "YWJjZA==")]
        public void Base64Encode(string plaintext, string base64encoded)
        {
            string actualValue = Common.Base64Encode(plaintext);
            Assert.Equal(base64encoded, actualValue);
        }

        [Theory]
        [InlineData("YWJjZA==", "abcd")]
        public void Base64Decode(string base64encoded, string plaintext)
        {
            string actualValue = Common.Base64Decode(base64encoded);
            Assert.Equal(plaintext, actualValue);
        }

        [Theory]
        [InlineData("", false)]
        [InlineData("2", false)]
        [InlineData("20", false)]
        [InlineData("201", false)]
        [InlineData("2014", false)]
        [InlineData("20140", false)]
        [InlineData("201406", false)]
        [InlineData("2014063", false)]
        [InlineData("20140630", true)]
        [InlineData("201406301", false)]
        [InlineData("2014063012", true)]
        [InlineData("201406301200", true)]
        [InlineData("20140630120030", true)]
        [InlineData("20140630120030.1-0500", true)]
        [InlineData("20140630120030.12-0500", true)]
        [InlineData("20140630120030.12+0500", true)]
        [InlineData("20140630120030.123-0500", true)]
        [InlineData("20140630120030.1234-0500", true)]
        public void TryConvertHl7DateTimeToUniversalTime(string hl7v2datetime, bool expectedSuccess)
        {
            bool isSuccess = Common.TryConvertHl7DateTimeToUniversalTime(hl7v2datetime, out DateTimeOffset dt);
            Assert.Equal(expectedSuccess, isSuccess);
        }

        [Theory]
        [InlineData(@"{ ""name"": ""John"" }", @"{""name"":""John""}")]
        [InlineData(@"{ ""name"": { ""first"": ""John"", ""last"": ""Smith"" } }", @"{""name.first"":""John"",""name.last"":""Smith""}")]
        [InlineData(@"{ ""names"": [ ""John"", ""Smith"" ] }", @"{""names[0]"":""John"",""names[1]"":""Smith""}")]
        public void FlattenJsonTest(string jsonToFlatten, string expectedJsonOutput)
        {
            System.Text.Json.JsonSerializerOptions options = new System.Text.Json.JsonSerializerOptions() { WriteIndented = false };
            Dictionary<string, string> nameValuePairs = Common.FlattenJson(jsonToFlatten);
            string actualJsonOutput = System.Text.Json.JsonSerializer.Serialize<Dictionary<string, string>>(nameValuePairs, options);
            Assert.Equal(expectedJsonOutput, actualJsonOutput);
        }
    }
}
