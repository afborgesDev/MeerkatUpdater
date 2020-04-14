using MeerkatUpdater.Core.Runner.Command.Common;
using MeerkatUpdater.Core.Runner.Command.DotNet;

namespace MeerkatUpdater.Core.Runner.Command.DotNetContProject
{
    /// <summary>
    /// Run the commands that provide the number of projects inside a solution
    /// </summary>
    public class CountProject : ICountProject
    {
        private readonly IDotNetCommand dotNetCommand;

        /// <summary>
        /// Default constructor for DI
        /// </summary>
        /// <param name="dotNetCommand"></param>
        public CountProject(IDotNetCommand dotNetCommand) =>
            this.dotNetCommand = dotNetCommand;

        /// <summary>
        /// Executes the dotnet command sln list to see more information see <br/>
        /// <see href="https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-sln">microsoft documentation</see>
        /// </summary>
        /// <returns></returns>
        public int? Execute()
        {
            var result = this.dotNetCommand.RunCommand(DotnetCommandConst.SolutionCommand, DotnetCommandConst.ListCommand);
            return Scraper.CountProject.Execute(result.Output);
        }
    }
}