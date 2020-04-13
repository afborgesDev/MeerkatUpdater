using MeerkatUpdater.Core.Config;
using MeerkatUpdater.Core.Runner.Command.Common;
using MeerkatUpdater.Core.Runner.Scraper;
using System;

namespace MeerkatUpdater.Core.Runner.Command
{
    /// <summary>
    /// Wrap the dotnet build command
    /// </summary>
    public static class Build
    {
        /// <summary>
        /// Exceute the dotnet build command <br/>
        /// <see href="https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-build">microsoft documentation</see>
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="NullReferenceException"></exception>
        public static bool Execute()
        {
            var result = DotNetCommand.RunCommand(DotnetCommandConst.BuildCommand,
                                                  ConfigManager.GetExecutionConfigurations().SolutionPath ?? string.Empty,
                                                  DotnetCommandConst.TargetOutPutParam, ConfigManager.GetExecutionConfigurations().GetOutPutPath());
            return CleanOrBuildSuccess.IsSucceed(result.Output);
        }
    }
}