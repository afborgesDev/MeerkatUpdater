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
        private const string BuildCommand = "build";

        /// <summary>
        /// Exceute the dotnet build command
        /// </summary>
        /// <param name="execution"></param>
        /// <returns></returns>
        public static bool Execute(Execution execution)
        {
            if (execution is null)
                throw new ArgumentNullException(nameof(execution));

            if (execution.WorkDirectory is null)
                throw new NullReferenceException(DefaultMessages.RequiredWorkDirectoryForBuildCommand);

            execution.Arguments.Add(BuildCommand);
            execution.Arguments.Add(execution.WorkDirectory);

            var result = DotNetCommand.RunCommand(execution);
            return CleanOrBuildSuccess.IsSucceed(result.Output);
        }
    }
}