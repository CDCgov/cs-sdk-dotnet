using System;
using System.IO;
using System.Linq;
using CS.Sdk.Validators;
using CS.Sdk.Services;
using Xunit;

namespace CS.Sdk.Tests
{
    public class JsonContentValidatorTests
    {
        [Fact]
        public void Test_000()
        {
            /*
             * Message has no errors or warnings
             */
            string json = File.ReadAllText(Path.Combine("messages", "json", "test-000.json"));

            IVocabularyService vocabService = new InMemoryVocabularyService();
            IMmgService mmgService = new InMemoryMmgService();

            IContentValidator validator = new JsonContentValidator(vocabService, mmgService);

            ValidationResult result = validator.Validate(json);

            // message should fail
            Assert.True(result.IsSuccess);

            // metadata should still be detected in spite of the failures
            Assert.Equal(expected: "CT", actual: result.NationalReportingJurisdiction);
            Assert.Equal(expected: "BABESIOSIS_TC01", actual: result.LocalRecordId);
            Assert.Equal(expected: "Babesiosis_MMG_V1.0", actual: result.Profile);

            // assert that the correct # of errors and warnings are present
            Assert.True(result.Errors == 0);
            Assert.True(result.Warnings == 0);
        }

        [Fact]
        public void Test_001()
        {
            /*
             * Checks for:
             * - Invalid date in a date/time field
             * - Invalid concept code in a non-repeating coded field
             * - Missing required fields: MMWR week and MMWR year are absent from the schema, meaning they have no properties in the Json
             */
            string json = File.ReadAllText(Path.Combine("messages", "json", "test-001.json"));

            IVocabularyService vocabService = new InMemoryVocabularyService();
            IMmgService mmgService = new InMemoryMmgService();

            IContentValidator validator = new JsonContentValidator(vocabService, mmgService);

            ValidationResult result = validator.Validate(json);

            // message should fail
            Assert.False(result.IsSuccess);

            // metadata should still be detected in spite of the failures
            Assert.Equal(expected: "CT", actual: result.NationalReportingJurisdiction);
            Assert.Equal(expected: "BABESIOSIS_TC01", actual: result.LocalRecordId);
            Assert.Equal(expected: "Babesiosis_MMG_V1.0", actual: result.Profile);

            // check for errors/warnings
            Assert.True(result.ValidationMessages
                .Where(m => m.Severity == Severity.Error)
                .Where(m => m.Content.Equals("Required data element 'MMWR Week' was not found", StringComparison.OrdinalIgnoreCase))
                .FirstOrDefault()
                != null);

            Assert.True(result.ValidationMessages
                .Where(m => m.Severity == Severity.Error)
                .Where(m => m.Content.Equals("Required data element 'MMWR Year' was not found", StringComparison.OrdinalIgnoreCase))
                .FirstOrDefault()
                != null);

            Assert.True(result.ValidationMessages
                .Where(m => m.Severity == Severity.Warning)
                .Where(m => m.Content.Equals("Data element 'Hospitalized' with identifier '77974-4' is a coded element associated with the value set 'PHVS_YesNoUnknown_CDC'. However, the concept code 'X' in the message was not found as a valid concept for this value set", StringComparison.OrdinalIgnoreCase))
                .FirstOrDefault()
                != null);

            Assert.True(result.ValidationMessages
                .Where(m => m.Severity == Severity.Error)
                .Where(m => m.Content.Equals("Data element 'Illness End Date' has an invalid date value", StringComparison.OrdinalIgnoreCase))
                .FirstOrDefault()
                != null);

            // assert that the correct # of errors and warnings are present
            Assert.True(result.Errors == 3);
            Assert.True(result.Warnings == 1);
        }

        [Fact]
        public void Test_002()
        {
            /*
             * Checks for:
             * - Invalid code in Race field (W)
             * - Invalid code in ethnic_group field (H)
             * - Invalid integer value for illness_duration ("fifteen days" instead of 15)
             */
            string json = File.ReadAllText(Path.Combine("messages", "json", "test-002.json"));

            IVocabularyService vocabService = new InMemoryVocabularyService();
            IMmgService mmgService = new InMemoryMmgService();

            IContentValidator validator = new JsonContentValidator(vocabService, mmgService);

            ValidationResult result = validator.Validate(json);

            // message should fail
            Assert.False(result.IsSuccess);

            // metadata should still be detected in spite of the failures
            Assert.Equal(expected: "CT", actual: result.NationalReportingJurisdiction);
            Assert.Equal(expected: "BABESIOSIS_TC01", actual: result.LocalRecordId);
            Assert.Equal(expected: "Babesiosis_MMG_V1.0", actual: result.Profile);

            // check for errors/warnings
            Assert.True(result.ValidationMessages
                .Where(m => m.Severity == Severity.Error)
                .Where(m => m.Content.Equals("Data element 'Illness Duration' is an invalid floating point", StringComparison.OrdinalIgnoreCase))
                .FirstOrDefault()
                != null);

            Assert.True(result.ValidationMessages
                .Where(m => m.Severity == Severity.Warning)
                .Where(m => m.Content.Equals(@"Data element 'Race Category' with identifier 'N/A: PID-10' is a coded element associated with the value set 'PHVS_RaceCategory_CDC_NullFlavor'. However, the concept code 'W' in the message was not found as a valid concept for this value set", StringComparison.OrdinalIgnoreCase))
                .FirstOrDefault()
                != null);

            Assert.True(result.ValidationMessages
                .Where(m => m.Severity == Severity.Warning)
                .Where(m => m.Content.Equals("Data element 'Ethnic Group' with identifier 'N/A: PID-22' is a coded element associated with the value set 'PHVS_EthnicityGroup_CDC_Unk'. However, the concept code 'H' in the message was not found as a valid concept for this value set", StringComparison.OrdinalIgnoreCase))
                .FirstOrDefault()
                != null);

            // assert that the correct # of errors and warnings are present
            Assert.True(result.Errors == 1);
            Assert.True(result.Warnings == 2);
        }

        [Fact]
        public void Test_003()
        {
            /*
             * Checks for:
             * - Illness end date should be a date in string format, but is instead an integer. ("illness_end_date": 2015,)
             */
            string json = File.ReadAllText(Path.Combine("messages", "json", "test-003.json"));

            IVocabularyService vocabService = new InMemoryVocabularyService();
            IMmgService mmgService = new InMemoryMmgService();

            IContentValidator validator = new JsonContentValidator(vocabService, mmgService);

            ValidationResult result = validator.Validate(json);

            // message should fail
            Assert.False(result.IsSuccess);

            // metadata should still be detected in spite of the failures
            Assert.Equal(expected: "CT", actual: result.NationalReportingJurisdiction);
            Assert.Equal(expected: "BABESIOSIS_TC01", actual: result.LocalRecordId);
            Assert.Equal(expected: "Babesiosis_MMG_V1.0", actual: result.Profile);

            // check for errors/warnings
            var validationMessage = result.ValidationMessages.FirstOrDefault();
            Assert.Equal(Severity.Error, validationMessage.Severity);
            Assert.Equal("Data element 'Illness End Date' is supposed to be a date but is type Number", validationMessage.Content);

            // assert that the correct # of errors and warnings are present
            Assert.True(result.Errors == 1);
        }

        [Fact]
        public void Test_004()
        {
            /*
             * Checks for:
             * - race_category __code and __code_system properties are absent
             */
            string json = File.ReadAllText(Path.Combine("messages", "json", "test-004.json"));

            IVocabularyService vocabService = new InMemoryVocabularyService();
            IMmgService mmgService = new InMemoryMmgService();

            IContentValidator validator = new JsonContentValidator(vocabService, mmgService);

            ValidationResult result = validator.Validate(json);

            // message should fail
            Assert.False(result.IsSuccess);

            // metadata should still be detected in spite of the failures
            Assert.Equal(expected: "CT", actual: result.NationalReportingJurisdiction);
            Assert.Equal(expected: "BABESIOSIS_TC01", actual: result.LocalRecordId);
            Assert.Equal(expected: "Babesiosis_MMG_V1.0", actual: result.Profile);

            // check for errors/warnings
            var validationMessage1 = result.ValidationMessages[0];
            Assert.Equal(Severity.Error, validationMessage1.Severity);
            Assert.Equal("Data element 'Race Category' is a coded element but no code property was found in the message", validationMessage1.Content);

            var validationMessage2 = result.ValidationMessages[1];
            Assert.Equal(Severity.Error, validationMessage2.Severity);
            Assert.Equal("Data element 'Race Category' is a coded element but no code system property was found in the message", validationMessage2.Content);

            // assert that the correct # of errors and warnings are present
            Assert.True(result.Errors == 2);
        }

        [Fact]
        public void Test_005()
        {
            /*
             * Checks for:
             * - illness duration units name is incorrect - "day" instead of "day [time]" - but this should still not raise any issues (is this right?)
             */
            string json = File.ReadAllText(Path.Combine("messages", "json", "test-005.json"));

            IVocabularyService vocabService = new InMemoryVocabularyService();
            IMmgService mmgService = new InMemoryMmgService();

            IContentValidator validator = new JsonContentValidator(vocabService, mmgService);

            ValidationResult result = validator.Validate(json);

            // message should fail
            Assert.True(result.IsSuccess);

            // metadata should still be detected in spite of the failures
            Assert.Equal(expected: "CT", actual: result.NationalReportingJurisdiction);
            Assert.Equal(expected: "BABESIOSIS_TC01", actual: result.LocalRecordId);
            Assert.Equal(expected: "Babesiosis_MMG_V1.0", actual: result.Profile);

            // check for errors/warnings
            Assert.Empty(result.ValidationMessages);

            // assert that the correct # of errors and warnings are present
            Assert.True(result.Warnings == 0);
            Assert.True(result.Errors == 0);
        }

        [Fact]
        public void Test_006()
        {
            /*
             * Checks for:
             * - date/time of message is absent
             */
            string json = File.ReadAllText(Path.Combine("messages", "json", "test-006.json"));

            IVocabularyService vocabService = new InMemoryVocabularyService();
            IMmgService mmgService = new InMemoryMmgService();

            IContentValidator validator = new JsonContentValidator(vocabService, mmgService);

            ValidationResult result = validator.Validate(json);

            // message should fail
            Assert.False(result.IsSuccess);

            // metadata should still be detected in spite of the failures
            Assert.Equal(expected: "CT", actual: result.NationalReportingJurisdiction);
            Assert.Equal(expected: "BABESIOSIS_TC01", actual: result.LocalRecordId);
            Assert.Equal(expected: "Babesiosis_MMG_V1.0", actual: result.Profile);

            // check for errors/warnings
            var validationMessage = result.ValidationMessages.FirstOrDefault();
            Assert.Equal(Severity.Error, validationMessage.Severity);
            Assert.Equal("Date/time of message was not found", validationMessage.Content);

            // assert that the correct # of errors and warnings are present
            Assert.True(result.Warnings == 0);
            Assert.True(result.Errors == 1);
        }

        [Fact]
        public void Test_007()
        {
            /*
             * Checks for:
             * - date/time of message is an invalid date ("201")
             */
            string json = File.ReadAllText(Path.Combine("messages", "json", "test-007.json"));

            IVocabularyService vocabService = new InMemoryVocabularyService();
            IMmgService mmgService = new InMemoryMmgService();

            IContentValidator validator = new JsonContentValidator(vocabService, mmgService);

            ValidationResult result = validator.Validate(json);

            // message should fail
            Assert.False(result.IsSuccess);

            // metadata should still be detected in spite of the failures
            Assert.Equal(expected: "CT", actual: result.NationalReportingJurisdiction);
            Assert.Equal(expected: "BABESIOSIS_TC01", actual: result.LocalRecordId);
            Assert.Equal(expected: "Babesiosis_MMG_V1.0", actual: result.Profile);

            // check for errors/warnings
            var validationMessage = result.ValidationMessages.FirstOrDefault();
            Assert.Equal(Severity.Error, validationMessage.Severity);
            Assert.Equal("Date/time of message is invalid", validationMessage.Content);

            // assert that the correct # of errors and warnings are present
            Assert.True(result.Warnings == 0);
            Assert.True(result.Errors == 1);
        }

        [Fact]
        public void Test_008()
        {
            /*
             * Checks for:
             * - date/time of message is an invalid data type (number instead of string)
             */
            string json = File.ReadAllText(Path.Combine("messages", "json", "test-008.json"));

            IVocabularyService vocabService = new InMemoryVocabularyService();
            IMmgService mmgService = new InMemoryMmgService();

            IContentValidator validator = new JsonContentValidator(vocabService, mmgService);

            ValidationResult result = validator.Validate(json);

            // message should fail
            Assert.False(result.IsSuccess);

            // metadata should still be detected in spite of the failures
            Assert.Equal(expected: "CT", actual: result.NationalReportingJurisdiction);
            Assert.Equal(expected: "BABESIOSIS_TC01", actual: result.LocalRecordId);
            Assert.Equal(expected: "Babesiosis_MMG_V1.0", actual: result.Profile);

            // check for errors/warnings
            var validationMessage = result.ValidationMessages.FirstOrDefault();
            Assert.Equal(Severity.Error, validationMessage.Severity);
            Assert.Equal("Date/time of message is expeted to be String, but is Number", validationMessage.Content);

            // assert that the correct # of errors and warnings are present
            Assert.True(result.Warnings == 0);
            Assert.True(result.Errors == 1);
        }

        [Fact]
        public void Test_009()
        {
            /*
             * Checks for:
             * - date/time of message is a blank string
             */
            string json = File.ReadAllText(Path.Combine("messages", "json", "test-009.json"));

            IVocabularyService vocabService = new InMemoryVocabularyService();
            IMmgService mmgService = new InMemoryMmgService();

            IContentValidator validator = new JsonContentValidator(vocabService, mmgService);

            ValidationResult result = validator.Validate(json);

            // message should fail
            Assert.False(result.IsSuccess);

            // metadata should still be detected in spite of the failures
            Assert.Equal(expected: "CT", actual: result.NationalReportingJurisdiction);
            Assert.Equal(expected: "BABESIOSIS_TC01", actual: result.LocalRecordId);
            Assert.Equal(expected: "Babesiosis_MMG_V1.0", actual: result.Profile);

            // check for errors/warnings
            var validationMessage = result.ValidationMessages.FirstOrDefault();
            Assert.Equal(Severity.Error, validationMessage.Severity);
            Assert.Equal("Date/time of message is blank", validationMessage.Content);

            // assert that the correct # of errors and warnings are present
            Assert.True(result.Warnings == 0);
            Assert.True(result.Errors == 1);
        }
    }
}
