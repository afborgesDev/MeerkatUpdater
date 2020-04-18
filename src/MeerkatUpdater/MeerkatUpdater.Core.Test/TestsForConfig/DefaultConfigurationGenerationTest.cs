using FluentAssertions;
using MeerkatUpdater.Core.Config.DefaultServices;
using MeerkatUpdater.Core.Config.Model;
using Microsoft.Extensions.Logging;
using Xunit;

namespace MeerkatUpdater.Core.Test.TestsForConfig
{
    public class DefaultConfigurationGenerationTest
    {
        [Fact]
        public void ShouldGenerateValidYMLStringForDefaultConfigurations()
        {
            var payload = DefaultConfigYmlGenerator.GenerateDefaultConfigurations();

            payload.Should().NotBeNullOrWhiteSpace();
            ValidateYmlPayloadIsValid(payload);
            ValidateYmlPayloadCanBeConvertedToConfigClass(payload);
        }

        private static void ValidateYmlPayloadIsValid(string payloadResult)
        {
            var isYmlValid = TryDeserialize(payloadResult, out _);
            isYmlValid.Should().BeTrue();
        }

        private static bool TryDeserialize(string payload, out ExecutionConfigurations executionConfigurations)
        {
            executionConfigurations = DefaultYmlDeserializer.Deserialize<ExecutionConfigurations>(payload);
            return executionConfigurations != null;
        }

        private static void ValidateYmlPayloadCanBeConvertedToConfigClass(string payloadResult)
        {
            TryDeserialize(payloadResult, out var executionConfigurations);
            executionConfigurations.Should().NotBeNull();
            executionConfigurations.SolutionPath.Should().Be(DefaultConfigYmlGenerator.DefaultSolutionPath);
            executionConfigurations.LogLevel.Should().Be(LogLevel.Information);
        }
    }
}