using System;
using FluentAssertions;
using MeerkatUpdater.Config.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TechTalk.SpecFlow;
using YamlDotNet.Serialization;

namespace MeerkatUpdater.Config.Test
{
    [Binding]
    public class DefaultConfigurationGenerationSteps
    {
        private const string DefaultConfigYmlIndentify = "DefaultConfigYmlIndentify";
        private readonly ScenarioContext scenarioContext;

        public DefaultConfigurationGenerationSteps(ScenarioContext scenarioContext) => this.scenarioContext = scenarioContext;

        [Given("The static default method to generate the configurations")]
        public void GivenTheStaticDefaultMethodToGenerateTheConfigurations()
        {
            var payloadResult = DefaultConfigYmlGenerator.GenerateDefaultConfigurations();
            this.scenarioContext.Set(payloadResult, DefaultConfigYmlIndentify);
        }

        [When("The string result should not be empty")]
        public void WhenTheStringResultShouldNotBeEmpty()
        {
            var payloadResult = GetPayloadResultString();
            payloadResult.Should().NotBeNullOrWhiteSpace();
        }

        [Then("The string result should be a valid yml file")]
        public void ThenTheStringResultShouldBeAValidYmlFile()
        {
            var payloadResult = GetPayloadResultString();
            var isYmlValid = TryDeserialize(payloadResult, out _);
            isYmlValid.Should().BeTrue();
        }

        [Then("the string result could serialize back into the ExecutionConfiguration class")]
        public void ThenTheStringResultCouldSerializeBackIntoTheExecutionConfigurationClass()
        {
            const string DefaultPath = ".";

            var payloadResult = GetPayloadResultString();
            TryDeserialize(payloadResult, out var executionConfigurations);
            executionConfigurations.Should().NotBeNull();
            executionConfigurations.SolutionPath.Should().Be(DefaultPath);
            executionConfigurations.LogLevel.Should().Be(LogLevel.Information);
        }

        private static bool TryDeserialize(string payload, out ExecutionConfigurations executionConfigurations)
        {
            try
            {
                var serializerJson = new SerializerBuilder().JsonCompatible().Build();
                var deserializerJson = new DeserializerBuilder().Build();

                var dynamicYaml = deserializerJson.Deserialize<dynamic>(payload);
                var jsonCompatibleYaml = serializerJson.Serialize(dynamicYaml);
                executionConfigurations = JsonConvert.DeserializeObject<ExecutionConfigurations>(jsonCompatibleYaml);
                return executionConfigurations != null;
            }
            catch (Exception e)
            {
                executionConfigurations = default;
                return false;
            }
        }

        private string GetPayloadResultString() => this.scenarioContext.Get<string>(DefaultConfigYmlIndentify);
    }
}
