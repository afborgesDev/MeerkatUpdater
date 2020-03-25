using System;
using System.IO;
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
        private const string FilePathIdentify = "FilePathIdentify";

        private readonly ScenarioContext scenarioContext;

        public DefaultConfigurationGenerationSteps(ScenarioContext scenarioContext) => this.scenarioContext = scenarioContext;

        [Given("The static default method to generate the configurations")]
        public void GivenTheStaticDefaultMethodToGenerateTheConfigurations()
        {
            var payloadResult = DefaultConfigYmlGenerator.GenerateDefaultConfigurations();
            this.scenarioContext.Set(payloadResult, DefaultConfigYmlIndentify);
        }

        [Given("The static default method to generate the file using the path: '(.*)'")]
        public void GivenTheStaticDefaultMethodToGenerateTheFileUsingThePath(string filePath)
        {
            DefaultConfigYmlGenerator.GenerateYmlFileForDefaultConfigurations(filePath);
            this.scenarioContext.Set(filePath, FilePathIdentify);
        }

        [When("has a valid string result")]
        public void WhenHasAValidStringResult()
        {
            var payloadResult = GetPayloadResultString();
            payloadResult.Should().NotBeNullOrWhiteSpace();
        }

        [When("has a existent file")]
        public void WhenHasAExistentFile()
        {
            var filePath = GetFilePathResult();
            var fileExists = File.Exists(filePath);
            fileExists.Should().BeTrue();
        }

        [Then("The string result should be a valid yml file")]
        public void ThenTheStringResultShouldBeAValidYmlFile()
        {
            var payloadResult = GetPayloadResultString();
            ValidateYmlPayloadIsValid(payloadResult);
        }

        [Then("the file is a valid yml")]
        public void ThenTheFileIsAValidYml()
        {
            var payloadResult = GetPayloadResultFromYmlFile();
            ValidateYmlPayloadIsValid(payloadResult);
        }

        [Then("the string result could serialize back into the ExecutionConfiguration class")]
        public void ThenTheStringResultCouldSerializeBackIntoTheExecutionConfigurationClass()
        {
            var payloadResult = GetPayloadResultString();
            ValidateYmlPayloadCanBeConvertedToConfigClass(payloadResult);
        }

        [Then("the file can be serialized back into the ExecutionConfiguration class")]
        public void ThenTheFileCanBeSerializedBackIntoTheExecutionConfigurationClass()
        {
            var payloadResult = GetPayloadResultFromYmlFile();
            ValidateYmlPayloadCanBeConvertedToConfigClass(payloadResult);
        }

        private static void ValidateYmlPayloadCanBeConvertedToConfigClass(string payloadResult)
        {
            const string DefaultPath = ".";

            TryDeserialize(payloadResult, out var executionConfigurations);
            executionConfigurations.Should().NotBeNull();
            executionConfigurations.SolutionPath.Should().Be(DefaultPath);
            executionConfigurations.LogLevel.Should().Be(LogLevel.Information);
        }

        private static void ValidateYmlPayloadIsValid(string payloadResult)
        {
            var isYmlValid = TryDeserialize(payloadResult, out _);
            isYmlValid.Should().BeTrue();
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

        private string GetPayloadResultFromYmlFile()
        {
            var filePath = GetFilePathResult();
            var payloadResult = File.ReadAllText(filePath);
            return payloadResult;
        }

        private string GetFilePathResult() => this.scenarioContext.Get<string>(FilePathIdentify);

        private string GetPayloadResultString() => this.scenarioContext.Get<string>(DefaultConfigYmlIndentify);
    }
}
