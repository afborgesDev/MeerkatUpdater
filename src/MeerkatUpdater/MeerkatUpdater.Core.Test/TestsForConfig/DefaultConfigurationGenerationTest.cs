using FluentAssertions;
using MeerkatUpdater.Core.Config.DefaultServices;
using MeerkatUpdater.Core.Config.Manager;
using MeerkatUpdater.Core.Config.Model;
using MeerkatUpdater.Core.Test.GeneralUsage;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using Xunit;

namespace MeerkatUpdater.Core.Test.TestsForConfig
{
    public class DefaultConfigurationGenerationTest
    {
        public const string stringEmptyIdentifyForParameters = "string.empty";
        public const string nullIdentifyForParameters = "null";

        [Fact]
        public void ShouldGenerateValidYMLStringForDefaultConfigurations()
        {
            var payload = DefaultConfigYmlGenerator.GenerateDefaultConfigurations();

            payload.Should().NotBeNullOrWhiteSpace();
            ValidateYmlPayloadIsValid(payload);
            ValidateYmlPayloadCanBeConvertedToConfigClass(payload);
        }

        [Fact]
        public void ShouldGenerateValidYMLFileForDefaultConfigurations()
        {
            const string fileName = "meerkatupdated.yml";
            DefaultConfigYmlGenerator.GenerateYmlFileForDefaultConfigurations(fileName);

            File.Exists(fileName).Should().BeTrue();

            var generatedPayload = File.ReadAllText(fileName);
            ValidateYmlPayloadIsValid(generatedPayload);
            ValidateYmlPayloadCanBeConvertedToConfigClass(generatedPayload);
        }

        [Theory]
        [InlineData("invalidPath")]
        [InlineData(@"--#$!#@#\")]
        [InlineData(@"PathWithOutYml\")]
        [InlineData("null")]
        [InlineData("string.empty")]
        [InlineData("Path.sln")]
        public void ShouldNotGenerateYMLFileWhenUsingInvalidPath(string path)
        {
            if (path == stringEmptyIdentifyForParameters)
                path = string.Empty;

            if (path == nullIdentifyForParameters)
                path = null;

            Exception triggeredException = null;

            try
            {
                DefaultConfigYmlGenerator.GenerateYmlFileForDefaultConfigurations(path);
            }
            catch (Exception e)
            {
                triggeredException = e;
            }
            finally
            {
                triggeredException.Should().NotBeNull();
            }
        }

        [Fact]
        public void ShoudlLoadConfigurationFromFileIfNull()
        {
            const string TestKey = nameof(ShoudlLoadConfigurationFromFileIfNull);
            try
            {
                var folderName = CreatedFoldersManager.GenerateNewFolder(TestKey);
                var outPut = CreateCommandObjects.PrepareSolutionForCustomOutPutPath(folderName);
                var defaultConfig = DefaultYmlDeserializer.Deserialize<ExecutionConfigurations>(DefaultConfigYmlGenerator.GenerateDefaultConfigurations());
                var configPath = Path.Combine(folderName, DefaultConfigYmlGenerator.DefaultConfigurationFileName);
                DefaultConfigYmlGenerator.GenerateYmlFileForDefaultConfigurations(configPath);

                var configManager = new ConfigManager(folderName + Path.DirectorySeparatorChar);
                var configurations = configManager.GetConfigurations();
                configurations.Should().NotBeNull();
                configurations.Should().BeEquivalentTo(defaultConfig);
            }
            finally
            {
                CreatedFoldersManager.TierDownTest(TestKey);
            }
        }

        [Fact]
        public void ShouldThrowExceptionWhenTryToLoadFromNotFoundFile()
        {
            const string TestKey = nameof(ShouldThrowExceptionWhenTryToLoadFromNotFoundFile);
            try
            {
                var folderName = CreatedFoldersManager.GenerateNewFolder(TestKey);
                var outPut = CreateCommandObjects.PrepareSolutionForCustomOutPutPath(folderName);
                var configManager = new ConfigManager(folderName);
                Action getConfiguration = () => configManager.GetConfigurations();
                getConfiguration.Should().Throw<FileNotFoundException>();
            }
            finally
            {
                CreatedFoldersManager.TierDownTest(TestKey);
            }
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