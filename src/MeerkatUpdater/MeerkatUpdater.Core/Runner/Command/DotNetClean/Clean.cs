using MeerkatUpdater.Core.Config.Manager;
using MeerkatUpdater.Core.Runner.Command.Common;
using MeerkatUpdater.Core.Runner.Command.DotNet;
using Microsoft.Extensions.Logging;

namespace MeerkatUpdater.Core.Runner.Command.DotNetClean
{
    /// <summary>
    /// Run the Clean Command
    /// </summary>
    public class Clean : IClean
    {
        private readonly IDotNetCommand dotNetCommand;
        private readonly IConfigManager configManager;
        private readonly ILogger<Clean> logger;

        /// <summary>
        /// The default DI constructor
        /// </summary>
        /// <param name="dotNetCommand"></param>
        /// <param name="configManager"></param>
        /// <param name="logger"></param>
        public Clean(IDotNetCommand dotNetCommand, IConfigManager configManager, ILogger<Clean> logger) =>
            (this.dotNetCommand, this.configManager, this.logger) = (dotNetCommand, configManager, logger);

        /// <summary>
        /// Executes the dotnet clean command <br/>
        /// For more information about the command, see: <see href="https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-clean">dotnet clean documentation</see>
        /// </summary>
        public void Execute()
        {
            var solutionPath = this.configManager.GetConfigurations().SolutionPath ?? string.Empty;
            var outputPath = this.configManager.GetConfigurations().GetTargetOutPutPath();
            this.logger.LogInformation(DefaultMessages.LOG_CleaningProject);
            _ = this.dotNetCommand.RunCommand(DotnetCommandConst.CleanCommand,
                                              solutionPath,
                                              DotnetCommandConst.TargetOutPutParam,
                                              outputPath);
        }
    }
}