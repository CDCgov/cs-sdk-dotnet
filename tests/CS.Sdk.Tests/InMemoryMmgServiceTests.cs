using CS.Mmg;
using CS.Sdk.Services;
using Xunit;

namespace CS.Sdk.Tests
{
    public class InMemoryMmgServiceTests
    {
        [Theory]
        [InlineData("Pertussis_MMG", "10190", "Pertussis")]
        [InlineData("Varicella_MMG", "10030", "Varicella")]
        [InlineData("Trichinellosis_MMG_V1.0", "10270", "Trichinellosis")]
        [InlineData("STD_MMG_V1.0", "10311", "STD")]
        [InlineData("Babesiosis_MMG_V1.0", "12010", "Babesiosis")]
        [InlineData("TB_MMG_V3.0", "10220", "Tuberculosis and LTBI")]
        [InlineData("CongenitalSyphilis_MMG_V1.0", "10316", "Congenital Syphilis")]
        [InlineData("CongenitalSyphilis_MMG_V1.1", "10316", "Congenital Syphilis 1.1")]
        public void Basic_Mmg_Test_01(string profile, string conditionCode, string expectedMmgName)
        {
            IMmgService mmgService = new InMemoryMmgService();

            MessageMappingGuide mmg = mmgService.Get(profile, conditionCode);

            Assert.Equal(expected: expectedMmgName, actual: mmg.Name);
        }
    }
}
