using MeerkatUpdater.Core.Config.Manager;
using MeerkatUpdater.Core.Runner.Command.DotNetBuild;
using MeerkatUpdater.Core.Runner.Command.DotNetClean;
using MeerkatUpdater.Core.Runner.Command.DotNetContProject;
using Microsoft.Extensions.Logging;
using System;
using System.Globalization;

namespace MeerkatUpdater.Core.Runner.Command.DotNetUpdateProcess
{
    public class UpdateProcessPreparation : IUpdateProcessPreparation
    {
        private readonly IClean clean;
        private readonly IBuild build;
        private readonly ICountProject countProject;
        private readonly IConfigManager configManager;
        private readonly ILogger<UpdateProcessPreparation> logger;

        public UpdateProcessPreparation(IClean clean, IBuild build, ICountProject countProject, IConfigManager configManager, ILogger<UpdateProcessPreparation> logger)
        {
            this.clean = clean;
            this.build = build;
            this.countProject = countProject;
            this.configManager = configManager;
            this.logger = logger;
        }

        public void Prepare()
        {
            this.clean.Execute();
            this.build.Execute();
            ConfigureMaximumWaitForExecution();
        }

        private void ConfigureMaximumWaitForExecution()
        {
            var newWaitTime = CalculateWaitTimeForExecution();
            this.configManager.GetConfigurations().NugetConfigurations?.SetNewMaxTimeSecondsTimeOut(newWaitTime);
            this.logger.LogInformation($"Based on the number of projects inside the solutino. The TimeOut was changed to {TimeSpan.FromSeconds(newWaitTime)} seconds");
        }

        private int CalculateWaitTimeForExecution()
        {
            const int BaseSeconds = 10;
            var numberOfProjectsOnSolution = this.countProject.Execute();

            if (numberOfProjectsOnSolution is null)
                return BaseSeconds;

            return Convert.ToInt32(numberOfProjectsOnSolution * BaseSeconds, CultureInfo.InvariantCulture);
        }
    }
}