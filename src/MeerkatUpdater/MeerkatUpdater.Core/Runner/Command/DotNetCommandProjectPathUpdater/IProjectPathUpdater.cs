using MeerkatUpdater.Core.Runner.Model.PackageInfo;
using System.Collections.Generic;

namespace MeerkatUpdater.Core.Runner.Command.DotNetCommandProjectPathUpdater
{
    /// <summary>
    /// Using dotnet sln list update the path for each project insde a solution
    /// </summary>
    public interface IProjectPathUpdater
    {
        /// <summary>
        /// Using dotnet sln list update the path for each project insde a solution
        /// </summary>
        void Execute(ref List<ProjectInfo> projects);
    }
}