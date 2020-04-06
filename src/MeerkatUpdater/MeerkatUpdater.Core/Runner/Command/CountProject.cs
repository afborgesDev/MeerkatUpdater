using MeerkatUpdater.Core.Runner.Command.Common;
using MeerkatUpdater.Core.Runner.Model.DotNet;
using System;

namespace MeerkatUpdater.Core.Runner.Command
{
    /// <summary>
    /// Run the commands that provide the number of projects inside a solution
    /// </summary>
    public static class CountProject
    {
        /// <summary>
        /// Executes the dotnet command sln list to see more information see <br/>
        ///
        /// </summary>
        /// <param name="workdirectory"></param>
        /// <returns></returns>
        public static int? Execute(string workdirectory)
        {
            if (string.IsNullOrWhiteSpace(workdirectory))
                throw new ArgumentNullException(nameof(workdirectory));

            var execution = Execution.FromDirectoryAndArguments(workdirectory, DotnetCommandConst.SolutionCommand, DotnetCommandConst.ListCommand);
            var result = DotNetCommand.RunCommand(execution);
            return Scraper.CountProject.Execute(result.Output);
        }
    }
}