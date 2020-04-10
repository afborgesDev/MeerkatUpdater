using MeerkatUpdater.Core.Config;
using System;
using System.Globalization;
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
        /// <returns></returns>
        public static async Task Execute()
        {
            BeforeExecution();
            var projectInfo = OutDated.Execute();
            if (projectInfo is null || projectInfo.Count == 0)
                return;

            // await UpdateProjects(projectInfo).ConfigureAwait(false);
        }

        //private static Task UpdateProjects(List<ProjectInfo> projectInfo)
        //{
        //    ProjectPathUpdater.Execute(ref projectInfo);
        //}

        private static void BeforeExecution()
        {
            Clean.Execute();
            Build.Execute();
            CalculateMaximumWaitForExecution();
        }

        private static void CalculateMaximumWaitForExecution()
        {
            const int BaseSeconds = 10;
            var numberOfProjectsOnSolution = CountProject.Execute();

            if (numberOfProjectsOnSolution is null)
                return;

            var newWaitTime = Convert.ToInt32(numberOfProjectsOnSolution * BaseSeconds, CultureInfo.InvariantCulture);

            ConfigManager.GetExecutionConfigurations().NugetConfigurations?.SetNewMaxTimeSecondsTimeOut(newWaitTime);
        }
    }
}