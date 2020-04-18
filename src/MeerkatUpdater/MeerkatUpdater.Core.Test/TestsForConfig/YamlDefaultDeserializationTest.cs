using FluentAssertions;
using MeerkatUpdater.Core.Config.DefaultServices;
using MeerkatUpdater.Core.Config.Model;
using System;
using Xunit;

namespace MeerkatUpdater.Core.Test.TestsForConfig
{
    public class YamlDefaultDeserializationTest
    {
        [Fact]
        public void ShouldDefaultDeserealizationGenerateValidExcutionConfigClass()
        {
            var defaultPayload = DefaultConfigYmlGenerator.GenerateDefaultConfigurations();
            var deserializedClass = DefaultYmlDeserializer.Deserialize<ExecutionConfigurations>(defaultPayload);

            deserializedClass.Should().NotBeNull();
            deserializedClass.NugetConfigurations.Should().NotBeNull();
            deserializedClass.NugetConfigurations.Sources.Count.Should().Be(1);
            deserializedClass.UpdateConfigurations.Should().NotBeNull();
            deserializedClass.SolutionPath.Should().Be(DefaultConfigYmlGenerator.DefaultSolutionPath);
            deserializedClass.LogLevel.Should().Be(DefaultConfigYmlGenerator.DefaultLogLevel);
        }

        [Theory]
        [InlineData("payload")]
        [InlineData("string.empty")]
        [InlineData("null")]
        public void ShouldDefaultDeserealizationRiseExceptionWhenInvalidStringPayload(string stringPayload)
        {
            if (stringPayload == DefaultConfigurationGenerationTest.stringEmptyIdentifyForParameters)
                stringPayload = string.Empty;

            if (stringPayload == DefaultConfigurationGenerationTest.nullIdentifyForParameters)
                stringPayload = null;

            Exception expectedException = null;

            try
            {
                DefaultYmlDeserializer.Deserialize<ExecutionConfigurations>(stringPayload);
            }
            catch (Exception e)
            {
                expectedException = e;
            }
            finally
            {
                expectedException.Should().NotBeNull();
            }
        }
    }
}