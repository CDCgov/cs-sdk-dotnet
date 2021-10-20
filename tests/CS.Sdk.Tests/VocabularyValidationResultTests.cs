using CS.Sdk.Services;
using Xunit;

namespace CS.Sdk.Tests
{
    public sealed class VocabularyValidationResultTests
    {
        [Fact]
        public void Success_Test_01()
        {
            VocabularyValidationResult result = new VocabularyValidationResult(isCodeValid: true, isNameValid: false, isSystemValid: true);
            Assert.True(result.IsCodeValid);
            Assert.False(result.IsNameValid);
            Assert.True(result.IsSystemValid);
        }
    }
}
