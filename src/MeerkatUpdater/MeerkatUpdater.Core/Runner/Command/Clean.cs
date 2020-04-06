using MeerkatUpdater.Core.Runner.Command.Common;
using MeerkatUpdater.Core.Runner.Model.DotNet;

namespace MeerkatUpdater.Core.Runner.Command
{
    /// <summary>
    /// Run the Clean Command
    /// </summary>
    public static class Clean
    {
        /// <summary>
        /// Executes the dotnet clean command <br/>
        /// For more information about the command, see: <see href="https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-clean">dotnet clean documentation</see>
        /// </summary>
        /// <param name="workDirectory"></param>
        public static void Execute(string workDirectory)
        {
            var execution = Execution.FromDirectoryAndArguments(workDirectory, DotnetCommandConst.CleanCommand);
            _ = DotNetCommand.RunCommand(execution);
        }
    }
}