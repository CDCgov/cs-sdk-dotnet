using System;
using System.Collections.Generic;
using System.Text;
using CS.Sdk.Converters;
using Xunit;

namespace CS.Sdk.Tests
{
    public sealed class ConversionResultTests
    {
        [Fact]
        public void Success_Test_01()
        {
            ConversionResult result = new ConversionResult()
            {
                TransactionId = "ABCD",
                UniqueCaseId = "V001",
                ConversionMessages = new List<ProcessResultMessage>() 
                { 
                    new ProcessResultMessage()
                    {
                        Severity = Severity.Error,
                        Content = "Error 01",
                        ErrorCode = "001"
                    }
                }
            };

            Assert.False(result.IsSuccess);
            Assert.True(result.ConversionMessages.Count == 1);
            Assert.Equal("ABCD", result.TransactionId);
            Assert.Equal("V001", result.UniqueCaseId);
        }
    }
}
