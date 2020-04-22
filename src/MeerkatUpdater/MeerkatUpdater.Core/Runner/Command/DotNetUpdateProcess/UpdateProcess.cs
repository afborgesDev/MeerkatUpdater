using MeerkatUpdater.Core.Config.Manager;
using MeerkatUpdater.Core.Runner.Command.DotNetBuild;
using MeerkatUpdater.Core.Runner.Command.DotNetClean;
using MeerkatUpdater.Core.Runner.Command.DotNetCommandProjectPathUpdater;
using MeerkatUpdater.Core.Runner.Command.DotNetCommandUpdate;
using MeerkatUpdater.Core.Runner.Command.DotNetContProject;
using MeerkatUpdater.Core.Runner.Command.DotNetOutDated;
using MeerkatUpdater.Core.Runner.Model.PackageInfo;
using System;
using System.Collections.Generic;
using System.Globalization;

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
        private readonly IProjectPathUpdater projectPathUpdater;
        private readonly IUpdate update;

        /// <summary>
        /// Default DI constructor
        /// </summary>
        /// <param name="outDated"></param>
        /// <param name="clean"></param>
        /// <param name="build"></param>
        /// <param name="countProject"></param>
        /// <param name="configManager"></param>
        /// <param name="projectPathUpdater"></param>
        /// <param name="update"></param>
        public UpdateProcess(IOutDated outDated,
            IClean clean,
            IBuild build,
            ICountProject countProject,
            IConfigManager configManager,
            IProjectPathUpdater projectPathUpdater,
            IUpdate update)
        {
            this.clean = clean;
            this.build = build;
            this.countProject = countProject;
            this.outDated = outDated;
            this.configManager = configManager;
            this.projectPathUpdater = projectPathUpdater;
            this.update = update;
        }

        /// <summary>
        /// Do the OutDated process by checking and trying to update
        /// </summary>
        /// <returns></returns>
        public void Execute()
        {
            BeforeExecution();
            var projectInfo = this.outDated.Execute();
            if (projectInfo is null || projectInfo.Count == 0)
                return;

            UpdateProjects(projectInfo);
        }

        private void UpdateProjects(List<ProjectInfo> projectInfo)
        {
            this.projectPathUpdater.Execute(ref projectInfo);
            this.update.Execute(ref projectInfo);
        }

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