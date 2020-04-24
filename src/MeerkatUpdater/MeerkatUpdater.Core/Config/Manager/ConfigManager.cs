using MeerkatUpdater.Core.Config.DefaultServices;
using MeerkatUpdater.Core.Config.Model;
using System;
using System.Globalization;
using System.IO;
using System.Linq;

namespace MeerkatUpdater.Core.Config.Manager
{
    /// <summary>
    /// Responsable to Load and provide access to the configurations
    /// </summary>
    public class ConfigManager : IConfigManager
    {
        private const string ExecutionConfigurationsFile = "MeerkatUpdater";
        private readonly TimeSpan DefaultMaximumWait = TimeSpan.FromSeconds(10);
        private readonly string[] SupportedExtensions = new string[2] { ".yaml", ".yml" };

        private ExecutionConfigurations? ExecutionConfigurations { get; set; }

        /// <summary>
        /// If the ExecutionConfiguration is not loaded try to load from file.
        /// </summary>
        /// <returns></returns>
        public ExecutionConfigurations GetConfigurations()
        {
            if (ExecutionConfigurations is null)
                LoadConfigurationsFromFile();

            return ExecutionConfigurations ?? throw new FileNotFoundException(DefaultMessages.ErrorOnLoadConfigurationsFromFile);
        }

        /// <summary>
        /// Return the default maximum wait
        /// </summary>
        /// <returns></returns>
        public TimeSpan GetDefaultMaximumWait() => DefaultMaximumWait;

        /// <summary>
        /// Get the configured wait time in mili seconds
        /// </summary>
        /// <returns></returns>
        public int GetWaitMiliSeconds()
        {
            var fromConfig = GetConfigurations().NugetConfigurations?.GetMaximumWaitTimeMiliseconds();
            if (fromConfig is null || fromConfig <= 0)
                return Convert.ToInt32(GetDefaultMaximumWait().TotalMilliseconds, CultureInfo.InvariantCulture);
            return fromConfig.Value;
        }

        /// <summary>
        /// Set a new Instance of configurations
        /// </summary>
        /// <param name="executionConfigurations"></param>
        public void SetConfigurations(ExecutionConfigurations executionConfigurations) =>
            ExecutionConfigurations = executionConfigurations ?? throw new ArgumentNullException(nameof(executionConfigurations));

        private void LoadConfigurationsFromFile()
        {
            var payload = GetConfigurationsPayload();
            if (payload is null)
                throw new FileNotFoundException($"Could not find the {ExecutionConfigurationsFile} with any of extensions {SupportedExtensions.Aggregate((oldEx, currentEx) => oldEx + " or " + currentEx)}");

            ExecutionConfigurations = DefaultYmlDeserializer.Deserialize<ExecutionConfigurations>(payload);
        }

        private string? GetConfigurationsPayload()
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