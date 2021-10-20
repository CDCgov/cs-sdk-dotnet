using System;
using System.Collections.Generic;

namespace CS.Sdk.Services
{
    /// <summary>
    /// Vocabulary service with hard-coded values for commonly-used value sets. This
    /// service is not recommended for production use, only for development and testing
    /// purposes.
    /// </summary>
    public sealed class InMemoryVocabularyService : IVocabularyService
    {
        private static List<Tuple<string, string, string>> YNU = new List<Tuple<string, string, string>>() {
            new Tuple<string, string, string>( "Y", "Yes", "CDC" ),
            new Tuple<string, string, string>( "N", "No", "CDC" ),
            new Tuple<string, string, string>( "UNK", "Unknown", "NULLFL" ),
        };

        private static List<Tuple<string, string, string>> MFU = new List<Tuple<string, string, string>>() {
            new Tuple<string, string, string>( "M", "Male", "CDC" ),
            new Tuple<string, string, string>( "F", "Female", "CDC" ),
            new Tuple<string, string, string>( "U", "Unknown", "NULLFL" ),
        };

        private static List<Tuple<string, string, string>> Ethnicity = new List<Tuple<string, string, string>>() {
            new Tuple<string, string, string>( "2135-2", "Hispanic or Latino", "CDC" ),
            new Tuple<string, string, string>( "2186-5", "Not Hispanic or Latino", "CDC" ),
            new Tuple<string, string, string>( "OTH", "other", "NULLFL" ),
            new Tuple<string, string, string>( "UNK", "unknown", "NULLFL" ),
        };

        private static List<Tuple<string, string, string>> Race = new List<Tuple<string, string, string>>() {
            new Tuple<string, string, string>( "1002-5", "American Indian or Alaska Native", "CDC" ),
            new Tuple<string, string, string>( "2028-9", "Asian", "CDC" ),
            new Tuple<string, string, string>( "ASKU", "asked but unknown", "NULLFL" ),
            new Tuple<string, string, string>( "2054-5", "Black or African American", "CDC" ),
            new Tuple<string, string, string>( "2076-8", "Native Hawaiian or Other Pacific Islander", "CDC" ),
            new Tuple<string, string, string>( "NI", "NoInformation", "NULLFL" ),
            new Tuple<string, string, string>( "NASK", "not asked", "NULLFL" ),
            new Tuple<string, string, string>( "2131-1", "Other Race", "CDC" ),
            new Tuple<string, string, string>( "PHC1175", "Refused to answer", "PHVS" ),
            new Tuple<string, string, string>( "UNK", "unknown", "CDC" ),
            new Tuple<string, string, string>( "2106-3", "White", "CDC" ),
        };

        private static List<Tuple<string, string, string>> DurationUnit = new List<Tuple<string, string, string>>() {
            new Tuple<string, string, string>( "d", "day [time]", "UCUM" ),
            new Tuple<string, string, string>( "h", "hour [time]", "UCUM" ),
            new Tuple<string, string, string>( "min", "minute [time]", "UCUM" ),
            new Tuple<string, string, string>( "mo", "month [time]", "UCUM" ),
            new Tuple<string, string, string>( "s", "second [time]", "UCUM" ),
            new Tuple<string, string, string>( "UNK", "unknown", "NULLFL" ),
            new Tuple<string, string, string>( "wk", "week [time]", "UCUM" ),
            new Tuple<string, string, string>( "a", "year [time]", "UCUM" ),
        };

        private static List<Tuple<string, string, string>> AgeUnit = new List<Tuple<string, string, string>>() {
            new Tuple<string, string, string>( "d", "day [time]", "UCUM" ),
            new Tuple<string, string, string>( "h", "hour [time]", "UCUM" ),
            new Tuple<string, string, string>( "min", "minute [time]", "UCUM" ),
            new Tuple<string, string, string>( "mo", "month [time]", "UCUM" ),
            new Tuple<string, string, string>( "OTH", "other", "NULLFL" ),
            new Tuple<string, string, string>( "s", "second [time]", "UCUM" ),
            new Tuple<string, string, string>( "UNK", "unknown", "NULLFL" ),
            new Tuple<string, string, string>( "wk", "week [time]", "UCUM" ),
            new Tuple<string, string, string>( "a", "year [time]", "UCUM" ),
        };

        private static List<Tuple<string, string, string>> DiseaseAcquiredJurisdiction = new List<Tuple<string, string, string>>() {
            new Tuple<string, string, string>( "PHC244", "Indigenous", "CDC" ),
            new Tuple<string, string, string>( "C1512888", "International", "CDC" ),
            new Tuple<string, string, string>( "PHC245", "In State,Out of jurisdiction", "CDC" ),
            new Tuple<string, string, string>( "PHC246", "Out of state", "CDC" ),
            new Tuple<string, string, string>( "UNK", "Unknown", "NULLFL" ),
            new Tuple<string, string, string>( "PHC1274", "Yes imported, but not able to determine source state and/or country", "CDC" ),
        };

        private static List<Tuple<string, string, string>> CaseClassStatus = new List<Tuple<string, string, string>>() {
            new Tuple<string, string, string>( "410605003", "Confirmed present", "CDC" ),
            new Tuple<string, string, string>( "PHC178", "Not a Case", "CDC" ),
            new Tuple<string, string, string>( "2931005", "Probable diagnosis", "CDC" ),
            new Tuple<string, string, string>( "415684004", "Suspected", "CDC" ),
            new Tuple<string, string, string>( "UNK", "Unknown", "NULLFL" ),
        };

        private static List<Tuple<string, string, string>> CaseTransmissionMode = new List<Tuple<string, string, string>>() {
            new Tuple<string, string, string>( "416380006", "Airborne transmission", "CDC" ),
            new Tuple<string, string, string>( "409700000", "Animal to human transmission", "CDC" ),
            new Tuple<string, string, string>( "420014008", "Blood borne transmission", "CDC" ),
            new Tuple<string, string, string>( "461551000124100", "Droplet transmission", "CDC" ),
            new Tuple<string, string, string>( "416086007", "Food-borne transmission", "CDC" ),
            new Tuple<string, string, string>( "418375005", "Indeterminate disease transmission mode", "CDC" ),
            new Tuple<string, string, string>( "PHC142", "Mechanical", "CDC" ),
            new Tuple<string, string, string>( "416085006", "Nosocomial transmission", "CDC" ),
            new Tuple<string, string, string>( "OTH", "Other", "NULLFL" ),
            new Tuple<string, string, string>( "417564009", "Sexual transmission", "CDC" ),
            new Tuple<string, string, string>( "420193003", "Transdermal transmission", "CDC" ),
            new Tuple<string, string, string>( "417409004", "Transplacental transmission", "CDC" ),
            new Tuple<string, string, string>( "418427004", "Vector-borne transmission", "CDC" ),
            new Tuple<string, string, string>( "418117003", "Water-borne transmission", "CDC" )
        };

        private static List<Tuple<string, string, string>> ResultStatus = new List<Tuple<string, string, string>>() {
            new Tuple<string, string, string>( "F", "Final results; Can only be changed with a corrected result", "HL7" ),
            new Tuple<string, string, string>( "C", "Record coming over is a correction and thus replaces a final result", "HL7" ),
            new Tuple<string, string, string>( "X", "Results cannot be obtained for this observation", "HL7" ),
        };

        private static List<Tuple<string, string, string>> BinationalReportingCriteria = new List<Tuple<string, string, string>>() {
            new Tuple<string, string, string>( "PHC1140", "Exposure to suspected product from Canada or Mexico", "CDC" ),
            new Tuple<string, string, string>( "PHC1139", "Has case contacts in or from Mexico or Canada", "CDC" ),
            new Tuple<string, string, string>( "PHC1141", "Other situations that may require binational notification or coordination of response", "CDC" ),
            new Tuple<string, string, string>( "PHC1215", "Potentially exposed by a resident of Mexico or Canada", "CDC" ),
            new Tuple<string, string, string>( "PHC1137", "Potentially exposed while in Mexico or Canada", "CDC" ),
            new Tuple<string, string, string>( "PHC1138", "Resident of Canada or Mexico", "CDC" ),
        };

        private static List<Tuple<string, string, string>> TemperatureUnit = new List<Tuple<string, string, string>>() {
            new Tuple<string, string, string>( "Cel", "degree Celsius [temperature]", "UCUM" ),
            new Tuple<string, string, string>( "[degF]", "degree Fahrenheit [temperature]", "UCUM" ),
        };

        private static List<Tuple<string, string, string>> NationalReportingJurisdiction = new List<Tuple<string, string, string>>() {
            new Tuple<string, string, string>( "01", "AL", "FIPS5_2" ),
            new Tuple<string, string, string>( "02", "AK", "FIPS5_2" ),
            //new Tuple<string, string, string>( "03", "Unknown", "FIPS5_2" ),
            new Tuple<string, string, string>( "04", "AZ", "FIPS5_2" ),
            new Tuple<string, string, string>( "05", "AR", "FIPS5_2" ),
            new Tuple<string, string, string>( "06", "CA", "FIPS5_2" ),
            //new Tuple<string, string, string>( "07", "Unknown", "FIPS5_2" ),
            new Tuple<string, string, string>( "08", "CO", "FIPS5_2" ),
            new Tuple<string, string, string>( "09", "CT", "FIPS5_2" ),
            new Tuple<string, string, string>( "10", "DE", "FIPS5_2" ),
            new Tuple<string, string, string>( "11", "DC", "FIPS5_2" ),
            new Tuple<string, string, string>( "12", "FL", "FIPS5_2" ),
            new Tuple<string, string, string>( "13", "GA", "FIPS5_2" ),
            //new Tuple<string, string, string>( "14", "Unknown", "FIPS5_2" ),
            new Tuple<string, string, string>( "15", "HI", "FIPS5_2" ),
            new Tuple<string, string, string>( "16", "ID", "FIPS5_2" ),
            new Tuple<string, string, string>( "17", "IL", "FIPS5_2" ),
            new Tuple<string, string, string>( "18", "IN", "FIPS5_2" ),
            new Tuple<string, string, string>( "19", "IA", "FIPS5_2" ),
            new Tuple<string, string, string>( "20", "KS", "FIPS5_2" ),
            new Tuple<string, string, string>( "21", "KY", "FIPS5_2" ),
            new Tuple<string, string, string>( "22", "LA", "FIPS5_2" ),
            new Tuple<string, string, string>( "23", "ME", "FIPS5_2" ),
            new Tuple<string, string, string>( "24", "MD", "FIPS5_2" ),
            new Tuple<string, string, string>( "25", "MA", "FIPS5_2" ),
            new Tuple<string, string, string>( "26", "MI", "FIPS5_2" ),
            new Tuple<string, string, string>( "27", "MN", "FIPS5_2" ),
            new Tuple<string, string, string>( "28", "MS", "FIPS5_2" ),
            new Tuple<string, string, string>( "29", "MO", "FIPS5_2" ),
            new Tuple<string, string, string>( "30", "MT", "FIPS5_2" ),
            new Tuple<string, string, string>( "31", "NE", "FIPS5_2" ),
            new Tuple<string, string, string>( "32", "NV", "FIPS5_2" ),
            new Tuple<string, string, string>( "33", "NH", "FIPS5_2" ),
            new Tuple<string, string, string>( "34", "NJ", "FIPS5_2" ),
            new Tuple<string, string, string>( "35", "NM", "FIPS5_2" ),
            new Tuple<string, string, string>( "36", "NY", "FIPS5_2" ),
            new Tuple<string, string, string>( "37", "NC", "FIPS5_2" ),
            new Tuple<string, string, string>( "38", "ND", "FIPS5_2" ),
            new Tuple<string, string, string>( "39", "OH", "FIPS5_2" ),
            new Tuple<string, string, string>( "40", "OK", "FIPS5_2" ),
            new Tuple<string, string, string>( "41", "OR", "FIPS5_2" ),
            new Tuple<string, string, string>( "42", "PA", "FIPS5_2" ),
            //new Tuple<string, string, string>( "43", "Unknown", "FIPS5_2" ),
            new Tuple<string, string, string>( "44", "RI", "FIPS5_2" ),
            new Tuple<string, string, string>( "45", "SC", "FIPS5_2" ),
            new Tuple<string, string, string>( "46", "SD", "FIPS5_2" ),
            new Tuple<string, string, string>( "47", "TN", "FIPS5_2" ),
            new Tuple<string, string, string>( "48", "TX", "FIPS5_2" ),
            new Tuple<string, string, string>( "49", "UT", "FIPS5_2" ),
            new Tuple<string, string, string>( "50", "VT", "FIPS5_2" ),
            new Tuple<string, string, string>( "51", "VA", "FIPS5_2" ),
            //new Tuple<string, string, string>( "52", "Unknown", "FIPS5_2" ),
            new Tuple<string, string, string>( "53", "WA", "FIPS5_2" ),
            new Tuple<string, string, string>( "54", "WV", "FIPS5_2" ),
            new Tuple<string, string, string>( "55", "WI", "FIPS5_2" ),
            new Tuple<string, string, string>( "56", "WY", "FIPS5_2" ),
            //new Tuple<string, string, string>( "57", "Unknown", "FIPS5_2" ),
            //new Tuple<string, string, string>( "58", "Unknown", "FIPS5_2" ),
            //new Tuple<string, string, string>( "59", "Unknown", "FIPS5_2" ),
            new Tuple<string, string, string>( "60", "American Samoa", "FIPS5_2" ),
            new Tuple<string, string, string>( "64", "Fed States of Micronesia", "FIPS5_2" ),
            new Tuple<string, string, string>( "66", "Guam", "FIPS5_2" ),
            new Tuple<string, string, string>( "68", "Marshall Islands", "FIPS5_2" ),
            new Tuple<string, string, string>( "69", "N.Mariana Islands", "FIPS5_2" ),
            new Tuple<string, string, string>( "70", "Republic of Palau", "FIPS5_2" ),
            new Tuple<string, string, string>( "72", "Puerto Rico", "FIPS5_2" ),
            new Tuple<string, string, string>( "78", "US Virgin Islands", "FIPS5_2" ),
            new Tuple<string, string, string>( "975772", "New York City", "USGS" ),
        };

        private static List<Tuple<string, string, string>> YNRD = new List<Tuple<string, string, string>>() {
            new Tuple<string, string, string>( "Y", "Yes", "HL7" ),
            new Tuple<string, string, string>( "N", "No", "HL7" ),
            new Tuple<string, string, string>( "UNK", "Unknown", "NULLFL" ),
            new Tuple<string, string, string>( "NASK", "Did not ask", "NULLFL" ),
            new Tuple<string, string, string>( "PHC1175", "Refused to answer", "CDC" ),
        };

        private static List<Tuple<string, string, string>> BloodProduct = new List<Tuple<string, string, string>>() {
            new Tuple<string, string, string>( "256400005", "Plasma product", "SCT" ),
            new Tuple<string, string, string>( "256395009", "Platelet product", "SCT" ),
            new Tuple<string, string, string>( "126242007", "Red blood cells, blood product", "SCT" ),
            new Tuple<string, string, string>( "OTH", "other", "NULLFL" ),
            new Tuple<string, string, string>( "UNK", "unknown", "NULLFL" ),
        };

        private static List<Tuple<string, string, string>> OutdoorActivities = new List<Tuple<string, string, string>>() {
            new Tuple<string, string, string>( "10496000", "Camping", "SCT" ),
            new Tuple<string, string, string>( "451261000124107", "Hiking (qualifier value)", "SCT" ),
            new Tuple<string, string, string>( "451251000124105", "Hunting (qualifier value)", "SCT" ),
            new Tuple<string, string, string>( "451271000124100", "Yard work (qualifier value)", "SCT" ),
            new Tuple<string, string, string>( "OTH", "other", "NULLFL" ),
        };

        private static List<Tuple<string, string, string>> LabTestInterpretation = new List<Tuple<string, string, string>>() {
            new Tuple<string, string, string>( "82334004", "Indeterminate", "SCT" ),
            new Tuple<string, string, string>( "260385009", "Negative", "SCT" ),
            new Tuple<string, string, string>( "385660001", "Not done", "SCT" ),
            new Tuple<string, string, string>( "10828004", "Positive", "SCT" ),
            new Tuple<string, string, string>( "720735008", "Presumptive positive", "SCT" ),
            new Tuple<string, string, string>( "I", "Pending", "HL7" ),
            new Tuple<string, string, string>( "UNK", "unknown", "NULLFL" ),
        };

        /// <summary>
        /// Determines if a given concept is valid, using a specified value set. That is, the service should be queried to determine if the
        /// concept exists in the value set, and if it does not, then a validation failure will be returned.
        /// </summary>
        /// <param name="conceptCode">The concept code to check</param>
        /// <param name="conceptName">The concept name to check</param>
        /// <param name="conceptCodeSystem">The code system of the concept</param>
        /// <param name="valueSetCode">The value set to check against</param>
        /// <returns>VocabularyValidationResult</returns>
        public VocabularyValidationResult IsValid(string conceptCode, string conceptName, string conceptCodeSystem, string valueSetCode)
        {
            List<Tuple<string, string, string>> valueSet = valueSetCode switch
            {
                "PHVS_YesNoUnknown_CDC" => YNU,
                "PHVS_Sex_MFU" => MFU,
                "PHVS_RaceCategory_CDC_NullFlavor" => Race,
                "PHVS_EthnicityGroup_CDC_Unk" => Ethnicity,
                "PHVS_DurationUnit_CDC" => DurationUnit,
                "PHVS_AgeUnit_UCUM" => AgeUnit,
                "PHVS_DiseaseAcquiredJurisdiction_NND" => DiseaseAcquiredJurisdiction,
                "PHVS_CaseClassStatus_NND" => CaseClassStatus,
                "PHVS_CaseTransmissionMode_NND" => CaseTransmissionMode,
                "PHVS_ResultStatus_NND" => ResultStatus,
                "PHVS_BinationalReportingCriteria_CDC" => BinationalReportingCriteria,
                "PHVS_NationalReportingJurisdiction_NND" => NationalReportingJurisdiction,
                "PHVS_YNRD_CDC" => YNRD,
                "PHVS_TemperatureUnit_UCUM" => TemperatureUnit,
                "PHVS_BloodProduct_CDC" => BloodProduct,
                "PHVS_OutdoorActivities_CDC" => OutdoorActivities,
                "PHVS_LabTestInterpretation_CDC" => LabTestInterpretation,
                _ => null
            };

            if (valueSet == null)
            {
                // if it's not in our in-memory list, just skip it for now and assume it's valid
                return new VocabularyValidationResult(
                    isCodeValid: true,
                    isNameValid: true,
                    isSystemValid: true);
            }

            bool isCodeValid = false;
            bool isDescriptionValid = false;

            foreach (var t in valueSet)
            {
                if (t.Item1 == conceptCode)
                {
                    isCodeValid = true;
                    if (t.Item2.Equals(conceptName, StringComparison.OrdinalIgnoreCase))
                    {
                        isDescriptionValid = true;
                    }
                    break;
                }
            }

            var result = new VocabularyValidationResult(
                isCodeValid: isCodeValid,
                isNameValid: isDescriptionValid,
                isSystemValid: true); // assume true for code system lookups until we can build an API

            result.ConceptCode = conceptCode;
            result.ConceptName = conceptName;
            result.ConceptCodeSystem = conceptCodeSystem;

            return result;
        }
    }
}
