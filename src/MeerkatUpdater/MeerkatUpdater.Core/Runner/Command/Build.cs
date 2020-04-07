using MeerkatUpdater.Core.Runner.Command.Common;
using MeerkatUpdater.Core.Runner.Model.DotNet;
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
        /// <param name="solutionPath"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="NullReferenceException"></exception>
        public static bool Execute(string solutionPath)
        {
            if (string.IsNullOrWhiteSpace(solutionPath))
                throw new ArgumentNullException(nameof(solutionPath));

            var execution = BuildExecution(solutionPath);
            var result = DotNetCommand.RunCommand(execution);
            return CleanOrBuildSuccess.IsSucceed(result.Output);
        }

        private static Execution BuildExecution(string solutionPath) =>
            Execution.FromDirectoryAndArguments(solutionPath, DotnetCommandConst.BuildCommand, DotnetCommandConst.TargetOutPutParam, "outputTest");
    }
}