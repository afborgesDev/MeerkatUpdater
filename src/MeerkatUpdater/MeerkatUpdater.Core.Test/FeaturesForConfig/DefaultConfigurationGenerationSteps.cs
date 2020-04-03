﻿using FluentAssertions;
using MeerkatUpdater.Core.Config;
using MeerkatUpdater.Core.Config.Model;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using TechTalk.SpecFlow;

namespace MeerkatUpdater.Core.Test.FeaturesForConfig
{
    [Binding]
    public class DefaultConfigurationGenerationSteps
    {
        private const string DefaultConfigYmlIndentify = "DefaultConfigYmlIndentify";
        private const string FilePathIdentify = "FilePathIdentify";
        private const string ActionToTriggerGenerateYmlFileIndex = "ActionToTriggerGenerateYmlFileIndex";
        private const string stringEmptyIdentifyForParameters = "string.empty";
        private const string nullIdentifyForParameters = "null";
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

        [Given("Using the path: '(.*)'")]
        public void GivenUsingThePath(string invalidPath)
        {
            if (invalidPath == stringEmptyIdentifyForParameters)
                invalidPath = string.Empty;

            this.scenarioContext.Set(invalidPath, FilePathIdentify);
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

        [When("the static method to generate file is executed")]
        public void WhenTheStaticMethodToGenerateFileIsExecuted()
        {
            var filePath = GetFilePathResult();
            if (filePath == nullIdentifyForParameters)
                filePath = default;

            void triggerTheGenerateMethod() => DefaultConfigYmlGenerator.GenerateYmlFileForDefaultConfigurations(filePath);
            this.scenarioContext.Set((Action)triggerTheGenerateMethod, ActionToTriggerGenerateYmlFileIndex);
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

        [Then("An exception of '(.*)' is raised")]
        public void ThenAnExceptionOfIsRaised(string exceptionType)
        {
            var actionToTriggerGenerateYmlFile = this.scenarioContext.Get<Action>(ActionToTriggerGenerateYmlFileIndex);

            if (exceptionType == nameof(ArgumentException))
                actionToTriggerGenerateYmlFile.Should().Throw<ArgumentException>();
            else if (exceptionType == nameof(ArgumentNullException))
                actionToTriggerGenerateYmlFile.Should().Throw<ArgumentNullException>();
            else
                throw new ArgumentOutOfRangeException($"exceptionType not configured on the tests method: {nameof(ThenAnExceptionOfIsRaised)}");
        }

        private static void ValidateYmlPayloadCanBeConvertedToConfigClass(string payloadResult)
        {
            TryDeserialize(payloadResult, out var executionConfigurations);
            executionConfigurations.Should().NotBeNull();
            executionConfigurations.SolutionPath.Should().Be(DefaultConfigYmlGenerator.DefaultSolutionPath);
            executionConfigurations.LogLevel.Should().Be(LogLevel.Information);
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

        private string GetPayloadResultFromYmlFile()
        {
            var filePath = GetFilePathResult();
            return File.ReadAllText(filePath);
        }

        private string GetFilePathResult() => this.scenarioContext.Get<string>(FilePathIdentify);

        private string GetPayloadResultString() => this.scenarioContext.Get<string>(DefaultConfigYmlIndentify);
    }
}