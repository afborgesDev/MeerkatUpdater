using MeerkatUpdater.Core.Runner.Model.PackageInfo;
using System.Collections.Generic;

namespace MeerkatUpdater.Core.Runner.Command.DotNetOutDated
{
    /// <summary>
    /// Warp the dotnet package --outdated
    /// </summary>
    public interface IOutDated
    {
        /// <summary>
        /// Executes the dotnet package --outdated command to have the items that need update <br/>
        /// <see href="https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-list-package">Microsoft documentation</see>
        /// </summary>
        /// <returns></returns>
        List<ProjectInfo>? Execute();
    }
}