using MeerkatUpdater.Core.Runner.Command.Common;
using MeerkatUpdater.Core.Runner.Model.DotNet;
using MeerkatUpdater.Core.Runner.Model.PackageInfo;
using System.Collections.Generic;

namespace MeerkatUpdater.Core.Runner.Command
{
    /// <summary>
    /// Warp the dotnet package --outdated
    /// </summary>
    public static class OutDated
    {
        /// <summary>
        /// Executes the dotnet package --outdated command to have the items that need update <br/>
        /// <see href="https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-list-package">Microsoft documentation</see>
        /// </summary>
        /// <param name="workdDirectory"></param>
        /// <returns></returns>
        public static List<ProjectInfo> Execute(string workdDirectory)
        {
            var execution = Execution.FromDirectoryAndArguments(workdDirectory, DotnetCommandConst.PackageCommand, DotnetCommandConst.OutDatedParam);
            var result = DotNetCommand.RunCommand(execution);
        }
    }
}