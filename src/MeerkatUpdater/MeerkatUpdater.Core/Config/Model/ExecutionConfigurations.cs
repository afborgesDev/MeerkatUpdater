using Microsoft.Extensions.Logging;

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
        /// Determinate if the commands that generate something on IO will use the custom output path
        /// </summary>
        public string? OutPutPath { get; set; }

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
        /// Return empty if no output path was setted
        /// </summary>
        /// <returns></returns>
        public string GetTargetOutPutPath() => OutPutPath ?? string.Empty;

        /// <summary>
        /// Safe get solution path
        /// </summary>
        /// <returns></returns>
        public string GetSolutionPath() => SolutionPath ?? string.Empty;
    }
}