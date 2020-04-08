using MeerkatUpdater.Core.Runner.Model.DotNet;
using MeerkatUpdater.Core.Runner.Model.PackageInfo;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MeerkatUpdater.Core.Runner.Command
{
    /// <summary>
    /// Centralize the outdate work flow to <br/>
    /// - Count <br/>
    /// - Set Configurations <br/>
    /// - Update <br/>
    /// </summary>
    public static class UpdateProcess
    {
        /// <summary>
        /// Do the OutDated process by checking and trying to update
        /// </summary>
        /// <param name="workDirectory"></param>
        /// <returns></returns>
        public static async Task Execute(string workDirectory)
        {
            var maximumWaitTime = BeforeExecution(workDirectory);
            var projectInfo = OutDated.Execute(workDirectory);
            if (projectInfo is null || projectInfo.Count == 0)
                return;

            await UpdateProjects(workDirectory, maximumWaitTime, projectInfo).ConfigureAwait(false);
        }

        private static Task UpdateProjects(string workDirectory, TimeSpan maximumWaitTime, List<ProjectInfo> projectInfo)
        {
            ProjectPathUpdater.Execute(workDirectory, ref projectInfo);
        }

        private static TimeSpan BeforeExecution(string workDirectory)
        {
            Clean.Execute(workDirectory);
            Build.Execute(workDirectory);
            return CalculateMaximumWaitForExecution(workDirectory);
        }

        private static TimeSpan CalculateMaximumWaitForExecution(string workDirectory)
        {
            const int BaseSeconds = 10;
            var numberOfProjectsOnSolution = CountProject.Execute(workDirectory);

            if (numberOfProjectsOnSolution is null)
                return Execution.DefaultMaximumWait;

            return TimeSpan.FromSeconds(numberOfProjectsOnSolution.Value * BaseSeconds);
        }
    }
}