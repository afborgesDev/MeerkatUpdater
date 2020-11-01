using MeerkatUpdater.Core.Runner.Command.DotNetCommandProjectPathUpdater;
using MeerkatUpdater.Core.Runner.Command.DotNetCommandUpdate;
using MeerkatUpdater.Core.Runner.Command.DotNetOutDated;
using MeerkatUpdater.Core.Runner.Model.PackageInfo;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

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
        private readonly IOutDated outDated;
        private readonly IProjectPathUpdater projectPathUpdater;
        private readonly IUpdate update;
        private readonly ILogger<UpdateProcess> logger;
        private readonly IUpdateProcessPreparation updateProcessPreparation;

        /// <summary>
        /// Default DI constructor
        /// </summary>
        /// <param name="outDated"></param>
        /// <param name="updateProcessPreparation"></param>
        /// <param name="projectPathUpdater"></param>
        /// <param name="update"></param>
        /// <param name="logger"></param>
        public UpdateProcess(IOutDated outDated,
            IUpdateProcessPreparation updateProcessPreparation,
            IProjectPathUpdater projectPathUpdater,
            IUpdate update,
            ILogger<UpdateProcess> logger)
        {
            this.updateProcessPreparation = updateProcessPreparation;
            this.outDated = outDated;
            this.projectPathUpdater = projectPathUpdater;
            this.update = update;
            this.logger = logger;
        }

        /// <summary>
        /// Do the OutDated process by checking and trying to update
        /// </summary>
        /// <returns></returns>
        public void Execute()
        {
            this.logger.LogInformation(DefaultMessages.LOG_StartingExecutionUpdate);
            this.updateProcessPreparation.Prepare();
            var projectInfo = this.outDated.Execute();
            this.logger.LogInformation($"That is the number of packages to be updated on the solution {projectInfo?.Count}");
            if (projectInfo is null || projectInfo.Count == 0)
                return;

            UpdateProjects(projectInfo);
        }

        private void UpdateProjects(List<ProjectInfo> projectInfo)
        {
            this.logger.LogInformation(DefaultMessages.LOG_StartingUpdatePackages);
            this.projectPathUpdater.Execute(ref projectInfo);
            this.update.Execute(ref projectInfo);
        }
    }
}