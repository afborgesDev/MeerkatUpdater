using System;
using System.IO;
using MeerkatUpdater.Config.Models;
using Microsoft.Extensions.Logging;
using YamlDotNet.Serialization;

namespace MeerkatUpdater.Config
{
    /// <summary>
    /// Generate the default yml file that have the default convetions for the configurations
    /// </summary>
    public static class DefaultConfigYmlGenerator
    {
        /// <summary>
        /// Generate the yml string with the default configurations
        /// </summary>
        /// <returns></returns>
        public static string GenerateDefaultConfigurations()
        {
            var defaultConfigs = BuildDefaultConfigs();
            return BuildYmlFile(defaultConfigs);
        }

        /// <summary>
        /// Generate the yml configuration files that have the default convertions
        /// </summary>
        /// <param name="filePath"></param>
        public static void GenerateYmlFileForDefaultConfigurations(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentNullException(nameof(filePath));

            File.WriteAllText(filePath, GenerateDefaultConfigurations());
        }

        private static string BuildYmlFile(ExecutionConfigurations defaultConfigs)
        {
            var serializer = new SerializerBuilder().Build();
            return serializer.Serialize(defaultConfigs);
        }

        private static ExecutionConfigurations BuildDefaultConfigs() =>
            new ExecutionConfigurations {
                LogLevel = LogLevel.Information,
                SolutionPath = ".",
                NugetConfigurations = GetNugetDefaultConfigurations(),
                UpdateConfigurations = GetUpdateConfigurations()
            };

        private static NugetConfigurations GetNugetDefaultConfigurations() =>
            new NugetConfigurations {
                MaxTimeSecondsTimeOut = (int)TimeSpan.FromSeconds(10).TotalSeconds,
            };

        private static UpdateConfigurations GetUpdateConfigurations()
        {
            var updateConfig = new UpdateConfigurations() { RolbackIfFail = false };
            updateConfig.AllowedVersionsToUpdate.Add(SemanticVersion.NoUpdate);
            return updateConfig;
        }
    }
}
