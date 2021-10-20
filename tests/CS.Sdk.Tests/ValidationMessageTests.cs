using System;
using System.Collections.Generic;
using System.Text;
using CS.Sdk.Validators;
using Xunit;

namespace CS.Sdk.Tests
{
    public sealed class ValidationMessageTests
    {
        [Fact]
        public void Success_Test_01()
        {
            ValidationMessage validationMessage = new ValidationMessage();

            Assert.Equal(Severity.Information, validationMessage.Severity);
            Assert.Equal(ValidationMessageType.Other, validationMessage.MessageType);
            Assert.True(string.IsNullOrEmpty(validationMessage.Content));
            Assert.True(string.IsNullOrEmpty(validationMessage.ErrorCode));
            Assert.True(string.IsNullOrEmpty(validationMessage.Path));
            Assert.True(string.IsNullOrEmpty(validationMessage.PathAlternate));
        }

        [Fact]
        public void Success_Test_02()
        {
            ValidationMessage validationMessage = new ValidationMessage(Severity.Error, ValidationMessageType.Structural, "Content", "Path");

            Assert.Equal(Severity.Error, validationMessage.Severity);
            Assert.Equal(ValidationMessageType.Structural, validationMessage.MessageType);
            Assert.Equal("Content", validationMessage.Content);
            Assert.True(string.IsNullOrEmpty(validationMessage.ErrorCode));
            Assert.Equal("Path", validationMessage.Path);
            Assert.True(string.IsNullOrEmpty(validationMessage.PathAlternate));
        }

        [Fact]
        public void Success_Test_03()
        {
            ValidationMessage validationMessage = new ValidationMessage(Severity.Error, ValidationMessageType.Structural, "Content", "Path", "PathAlternate");

            Assert.Equal(Severity.Error, validationMessage.Severity);
            Assert.Equal(ValidationMessageType.Structural, validationMessage.MessageType);
            Assert.Equal("Content", validationMessage.Content);
            Assert.True(string.IsNullOrEmpty(validationMessage.ErrorCode));
            Assert.Equal("Path", validationMessage.Path);
            Assert.Equal("PathAlternate", validationMessage.PathAlternate);
        }
    }
}
