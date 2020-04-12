using MeerkatUpdater.Core.Config.Model;
using MeerkatUpdater.Core.Config.Model.CustomExceptions;
using System;
using System.IO;
using System.Linq;

namespace MeerkatUpdater.Core.Config
{
    /// <summary>
    /// Responsable to Load and provide access to the configurations
    /// </summary>
    public static class ConfigManager
    {
        /// <summary>
        /// The default maximum wait
        /// </summary>
        public static readonly TimeSpan DefaultMaximumWait = TimeSpan.FromSeconds(10);

        private const string ExecutionConfigurationsFile = "MeerkatUpdater";
        private static readonly string[] SupportedExtensions = new string[2] { ".yaml", ".yml" };

        /// <summary>
        /// The Configurations for executions
        /// </summary>
        public static ExecutionConfigurations? ExecutionConfigurations { get; set; }

        /// <summary>
        /// Load if necessary and then return the ExecutionConfiguration
        /// </summary>
        /// <returns></returns>
        public static ExecutionConfigurations GetExecutionConfigurations()
        {
            if (ExecutionConfigurations is null)
                LoadConfigurationsFromFile();

            if (ExecutionConfigurations is null)
                throw new DeserealizationException();

            return ExecutionConfigurations;
        }

        private static void LoadConfigurationsFromFile()
        {
            var payload = GetConfigurationsPayload();
            if (payload is null)
                throw new FileNotFoundException($"Could not find the {ExecutionConfigurationsFile} with any of extensions {SupportedExtensions.Aggregate((oldEx, currentEx) => oldEx + " or " + currentEx)}");

            ExecutionConfigurations = DefaultYmlDeserializer.Deserialize<ExecutionConfigurations>(payload);
        }

        private static string? GetConfigurationsPayload()
        {
            foreach (var extension in SupportedExtensions)
            {
                var fileName = $"{ExecutionConfigurationsFile}{extension}";
                if (File.Exists(fileName))
                    return File.ReadAllText(fileName);
            }

            return default;
        }
    }
}