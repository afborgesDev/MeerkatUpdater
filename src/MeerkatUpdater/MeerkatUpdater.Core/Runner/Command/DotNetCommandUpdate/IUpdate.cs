using MeerkatUpdater.Core.Runner.Model.PackageInfo;
using System.Collections.Generic;

namespace MeerkatUpdater.Core.Runner.Command.DotNetCommandUpdate
{
    /// <summary>
    /// Update a solution based on semantic allowed version to update
    /// </summary>
    public interface IUpdate
    {
        /// <summary>
        /// Execute the update by using add package to sulution command
        /// </summary>
        /// <param name="toUpdateProjectInfo"></param>
        void Execute(List<ProjectInfo> toUpdateProjectInfo);
    }
}