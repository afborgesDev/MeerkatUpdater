using Microsoft.Extensions.Logging;
using System;
using System.Globalization;

namespace MeerkatUpdater.Core.Config.Model
{
    /// <summary>
    /// All basic configurations for MeerkatUpdate runs and try to update
    /// </summary>
    public sealed class ExecutionConfigurations
    {
        /// <summary>
        /// The root path for the solution file .sln
        /// </summary>
        public string? SolutionPath { get; set; }

        /// <summary>
        /// The level of the log inside the application. <br/>
        /// to see all configurations please see <see cref="Microsoft.Extensions.Logging.LogLevel"/>
        /// </summary>
        public LogLevel LogLevel { get; set; }

        /// <summary>
        /// Aggragated configurations to nuget like: <br/>
        /// - <see cref="NugetConfigurations.MaxTimeSecondsTimeOut"/> as the maximun timeout for try to get a package from nuget repository <br/>
        /// - <see cref="NugetConfigurations.Sources"/>  as the adicional urls as sources from nuget
        /// </summary>/
        public NugetConfigurations? NugetConfigurations { get; set; }

        /// <summary>
        /// Aggregated configurations for the update process <br/>
        /// <see cref="UpdateConfigurations.RolbackIfFail"/> Sets if the process will continue if the updates fail <br/>
        /// <see cref="UpdateConfigurations.AllowedVersionsToUpdate"/> Sets the types of semantic versions that will be updated
        /// </summary>
        public UpdateConfigurations? UpdateConfigurations { get; set; }

        /// <summary>
        /// Returns the configuration from the nuget but if it was null take the time out from the default <see cref="ConfigManager.DefaultMaximumWait"/>
        /// </summary>
        /// <returns></returns>
        public int GetWaitMiliSeconds()
        {
            var fromConfig = NugetConfigurations?.GetMaximumWaitTimeMiliseconds();
            if (fromConfig is null || fromConfig <= 0)
                return Convert.ToInt32(ConfigManager.DefaultMaximumWait.TotalMilliseconds, CultureInfo.InvariantCulture);
            return fromConfig.Value;
        }
    }
}