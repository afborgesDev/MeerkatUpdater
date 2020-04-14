using MeerkatUpdater.Core.Config.Manager;
using MeerkatUpdater.Core.Runner.Command.DotNetBuild;
using MeerkatUpdater.Core.Runner.Command.DotNetClean;
using MeerkatUpdater.Core.Runner.Command.DotNetContProject;
using MeerkatUpdater.Core.Runner.Command.DotNetOutDated;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace MeerkatUpdater.Core.Runner.Command.DotNetUpdateProcess
{
    /// <summary>
    /// Centralize the outdate work flow to <br/>
    /// - Count <br/>
    /// - Set Configurations <br/>
    /// - Update <br/>
    /// </summary>
    public class UpdateProcess : IUpdateProcess
    {
        private readonly IClean clean;
        private readonly IBuild build;
        private readonly ICountProject countProject;
        private readonly IOutDated outDated;
        private readonly IConfigManager configManager;

        /// <summary>
        /// Default DI constructor
        /// </summary>
        /// <param name="outDated"></param>
        /// <param name="clean"></param>
        /// <param name="build"></param>
        /// <param name="countProject"></param>
        /// <param name="configManager"></param>
        public UpdateProcess(IOutDated outDated,
            IClean clean,
            IBuild build,
            ICountProject countProject,
            IConfigManager configManager)
        {
            this.clean = clean;
            this.build = build;
            this.countProject = countProject;
            this.outDated = outDated;
            this.configManager = configManager;
        }

        /// <summary>
        /// Do the OutDated process by checking and trying to update
        /// </summary>
        /// <returns></returns>
        public async Task Execute()
        {
            BeforeExecution();
            var projectInfo = this.outDated.Execute();
            if (projectInfo is null || projectInfo.Count == 0)
                return;

            // await UpdateProjects(projectInfo).ConfigureAwait(false);
        }

        //private Task UpdateProjects(List<ProjectInfo> projectInfo)
        //{
        //    ProjectPathUpdater.Execute(ref projectInfo);
        //}

        private void BeforeExecution()
        {
            this.clean.Execute();
            this.build.Execute();
            CalculateMaximumWaitForExecution();
        }

        private void CalculateMaximumWaitForExecution()
        {
            const int BaseSeconds = 10;
            var numberOfProjectsOnSolution = this.countProject.Execute();

            if (numberOfProjectsOnSolution is null)
                return;

            var newWaitTime = Convert.ToInt32(numberOfProjectsOnSolution * BaseSeconds, CultureInfo.InvariantCulture);

            this.configManager.GetConfigurations().NugetConfigurations?.SetNewMaxTimeSecondsTimeOut(newWaitTime);
        }
    }
}