using FluentAssertions;
using MeerkatUpdater.Core.Config.DefaultServices;
using MeerkatUpdater.Core.Config.Manager;
using MeerkatUpdater.Core.Config.Model;
using MeerkatUpdater.Core.Test.GeneralUsage;
using Newtonsoft.Json;
using System;
using System.Globalization;
using System.IO;
using Xunit;

namespace MeerkatUpdater.Core.Test.TestsForConfig
{
    public class ConfigManagerTest
    {
        [Fact]
        public void ShouldUseDefaultMaximumWaitTimeOutIfNotinformed()
        {
            const string TestKey = nameof(ShouldUseDefaultMaximumWaitTimeOutIfNotinformed);
            try
            {
                var folderName = CreatedFoldersManager.GenerateNewFolder(TestKey);
                var outPut = CreateCommandObjects.PrepareSolutionForCustomOutPutPath(folderName);
                var defaultConfig = DefaultYmlDeserializer.Deserialize<ExecutionConfigurations>(DefaultConfigYmlGenerator.GenerateDefaultConfigurations());
                defaultConfig.NugetConfigurations = null;
                var configPath = Path.Combine(folderName, DefaultConfigYmlGenerator.DefaultConfigurationFileName);
                File.WriteAllText(configPath, JsonConvert.SerializeObject(defaultConfig));
                var configManager = new ConfigManager(folderName);
                var timeOut = configManager.GetWaitMiliSeconds();

                var defaultTimeOut = Convert.ToInt32(configManager.GetDefaultMaximumWait().TotalMilliseconds, CultureInfo.InvariantCulture);
                timeOut.Should().Be(defaultTimeOut);
            }
            finally
            {
                CreatedFoldersManager.TierDownTest(TestKey);
            }
        }

        [Fact]
        public void ShouldThrowExceptionWhenTryToSetNullConfigurations()
        {
            const string TestKey = nameof(ShouldThrowExceptionWhenTryToSetNullConfigurations);
            try
            {
                var folderName = CreatedFoldersManager.GenerateNewFolder(TestKey);
                var outPut = CreateCommandObjects.PrepareSolutionForCustomOutPutPath(folderName);
                var defaultConfig = DefaultYmlDeserializer.Deserialize<ExecutionConfigurations>(DefaultConfigYmlGenerator.GenerateDefaultConfigurations());
                var configPath = Path.Combine(folderName, DefaultConfigYmlGenerator.DefaultConfigurationFileName);
                File.WriteAllText(configPath, JsonConvert.SerializeObject(defaultConfig));

                var configManager = new ConfigManager(folderName);
                Action setNullConfig = () => configManager.SetConfigurations(default);
                setNullConfig.Should().Throw<ArgumentNullException>();
            }
            finally
            {
                CreatedFoldersManager.TierDownTest(TestKey);
            }
        }

        [Fact]
        public void ShouldExecutionConfigurationsReturnEmptyPathInsteadOfNull()
        {
            const string TestKey = nameof(ShouldExecutionConfigurationsReturnEmptyPathInsteadOfNull);
            try
            {
                var folderName = CreatedFoldersManager.GenerateNewFolder(TestKey);
                var outPut = CreateCommandObjects.PrepareSolutionForCustomOutPutPath(folderName);
                var defaultConfig = DefaultYmlDeserializer.Deserialize<ExecutionConfigurations>(DefaultConfigYmlGenerator.GenerateDefaultConfigurations());
                defaultConfig.SolutionPath = default;
                var configPath = Path.Combine(folderName, DefaultConfigYmlGenerator.DefaultConfigurationFileName);
                File.WriteAllText(configPath, JsonConvert.SerializeObject(defaultConfig));

                var configManager = new ConfigManager(folderName);
                var configurations = configManager.GetConfigurations();
                configurations.GetTargetOutPutPath().Should().Be(string.Empty);
            }
            finally
            {
                CreatedFoldersManager.TierDownTest(TestKey);
            }
        }

        [Fact]
        public void ShouldNotSetInvalidMaximumTimeOut()
        {
            const string TestKey = nameof(ShouldNotSetInvalidMaximumTimeOut);
            try
            {
                var folderName = CreatedFoldersManager.GenerateNewFolder(TestKey);
                var outPut = CreateCommandObjects.PrepareSolutionForCustomOutPutPath(folderName);
                var defaultConfig = DefaultYmlDeserializer.Deserialize<ExecutionConfigurations>(DefaultConfigYmlGenerator.GenerateDefaultConfigurations());
                defaultConfig.SolutionPath = default;
                var configPath = Path.Combine(folderName, DefaultConfigYmlGenerator.DefaultConfigurationFileName);
                File.WriteAllText(configPath, JsonConvert.SerializeObject(defaultConfig));

                var configManager = new ConfigManager(folderName);
                var configurations = configManager.GetConfigurations();
                var previousTimeOut = configurations.NugetConfigurations.MaxTimeSecondsTimeOut;
                var newTimeOut = previousTimeOut + 2;

                configurations.NugetConfigurations.SetNewMaxTimeSecondsTimeOut(null);
                configurations.NugetConfigurations.MaxTimeSecondsTimeOut.Should().Be(previousTimeOut);
                configurations.NugetConfigurations.SetNewMaxTimeSecondsTimeOut(0);
                configurations.NugetConfigurations.MaxTimeSecondsTimeOut.Should().Be(previousTimeOut);
                configurations.NugetConfigurations.SetNewMaxTimeSecondsTimeOut(newTimeOut);
                configurations.NugetConfigurations.MaxTimeSecondsTimeOut.Should().NotBe(previousTimeOut);
                configurations.NugetConfigurations.MaxTimeSecondsTimeOut.Should().Be(newTimeOut);
            }
            finally
            {
                CreatedFoldersManager.TierDownTest(TestKey);
            }
        }
    }
}