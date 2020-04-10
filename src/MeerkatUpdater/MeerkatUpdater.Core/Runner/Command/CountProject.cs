using MeerkatUpdater.Core.Runner.Command.Common;

namespace MeerkatUpdater.Core.Runner.Command
{
    /// <summary>
    /// Run the commands that provide the number of projects inside a solution
    /// </summary>
    public static class CountProject
    {
        /// <summary>
        /// Executes the dotnet command sln list to see more information see <br/>
        /// <see href="https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-sln">microsoft documentation</see>
        /// </summary>
        /// <returns></returns>
        public static int? Execute()
        {
            var result = DotNetCommand.RunCommand(DotnetCommandConst.SolutionCommand, DotnetCommandConst.ListCommand);
            return Scraper.CountProject.Execute(result.Output);
        }
    }
}