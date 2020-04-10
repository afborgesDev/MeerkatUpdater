using MeerkatUpdater.Core.Runner.Command.Common;

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
        public static void Execute() => _ = DotNetCommand.RunCommand(DotnetCommandConst.CleanCommand);
    }
}