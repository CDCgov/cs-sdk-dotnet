using CS.Sdk.Services;
using Xunit;

namespace CS.Sdk.Tests
{
    public class InMemoryVocabularyServiceTests
    {
        [Theory]
        [InlineData("Y", "Yes", "YNU", "PHVS_YesNoUnknown_CDC", true)]
        [InlineData("X", "Yes", "YNU", "PHVS_YesNoUnknown_CDC", false)]
        public void Basic_Vocab_Test_01(string concept, string description, string codeSystem, string valueSetCode, bool expectedValidationResult)
        {
            IVocabularyService vocabService = new InMemoryVocabularyService();

            VocabularyValidationResult result = vocabService.IsValid(concept, description, codeSystem, valueSetCode);
            Assert.Equal(expected: expectedValidationResult, actual: result.IsCodeValid);
        }
    }
}
