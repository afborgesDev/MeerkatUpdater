using MeerkatUpdater.Config.Model;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using YamlDotNet.Serialization;

namespace MeerkatUpdater.Config
{
    /// <summary>
    /// Generate the default yml file that have the default convetions for the configurations
    /// </summary>
    public static class DefaultConfigYmlGenerator
    {
        /// <summary>
        /// The default value that is used to generate the yaml example file
        /// </summary>
        public const string DefaultSolutionPath = ".";

        /// <summary>
        /// The default value that is used to generate loglevel for the yaml example file
        /// </summary>
        public static readonly LogLevel DefaultLogLevel = LogLevel.Information;

        private static readonly string[] SupportedYamlExtensions = new string[2] { ".yml", ".yaml" };

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

            if (!IsValidPath(filePath))
                throw new ArgumentException($"The {filePath} need be a existent valid path with a .yml or .yaml file");

            File.WriteAllText(filePath, GenerateDefaultConfigurations());
        }

        private static bool IsValidPath(string filePath)
        {
            var path = Path.GetDirectoryName(Path.GetFullPath(filePath));
            if (!Directory.Exists(path))
                return false;

            if (!HasSlnFileOnFilePath(filePath))
                return false;

            return true;
        }

        private static bool HasSlnFileOnFilePath(string filePath)
        {
            var fileExtension = Path.GetExtension(filePath);
            return SupportedYamlExtensions.Any(x => x == fileExtension);
        }

        private static string BuildYmlFile(ExecutionConfigurations defaultConfigs)
        {
            var serializer = new SerializerBuilder().Build();
            return serializer.Serialize(defaultConfigs);
        }

        private static ExecutionConfigurations BuildDefaultConfigs() =>
            new ExecutionConfigurations
            {
                LogLevel = DefaultLogLevel,
                SolutionPath = DefaultSolutionPath,
                NugetConfigurations = GetNugetDefaultConfigurations(),
                UpdateConfigurations = GetUpdateConfigurations()
            };

        private static NugetConfigurations GetNugetDefaultConfigurations()
        {
            var nugetConfigs = new NugetConfigurations { MaxTimeSecondsTimeOut = (int)TimeSpan.FromSeconds(10).TotalSeconds };
            nugetConfigs.Sources.Add("https://api.nuget.org/v3/index.json");
            return nugetConfigs;
        }

        private static UpdateConfigurations GetUpdateConfigurations()
        {
            var updateConfig = new UpdateConfigurations() { RolbackIfFail = false };
            updateConfig.AllowedVersionsToUpdate.Add(SemanticVersion.NoUpdate);
            return updateConfig;
        }
    }
}