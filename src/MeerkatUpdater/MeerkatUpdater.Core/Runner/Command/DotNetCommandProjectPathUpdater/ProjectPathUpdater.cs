using MeerkatUpdater.Core.Runner.Command.Common;
using MeerkatUpdater.Core.Runner.Command.DotNet;
using MeerkatUpdater.Core.Runner.Model.PackageInfo;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace MeerkatUpdater.Core.Runner.Command.DotNetCommandProjectPathUpdater
{
    /// <summary>
    /// Updater for path on each project inside a solution
    /// </summary>
    public class ProjectPathUpdater : IProjectPathUpdater
    {
        private readonly IDotNetCommand dotNetCommand;
        private readonly ILogger logger;

        /// <summary>
        /// Default DI constructor
        /// </summary>
        /// <param name="dotNetCommand"></param>
        /// <param name="logger"></param>
        public ProjectPathUpdater(IDotNetCommand dotNetCommand, ILogger logger) =>
            (this.dotNetCommand, this.logger) = (dotNetCommand, logger);

        /// <summary>
        /// Based on the solution update the path for each project
        /// </summary>
        /// <param name="projects"></param>
        public void Execute(ref List<ProjectInfo> projects)
        {
            if (projects is null || projects.Count == 0)
                return;

            ProjectPathUpdaterLogs.ProjectPathUpdaterStarted(this.logger);
            var stopWatch = ValueStopwatch.StartNew();
            try
            {
                ProjectPathUpdaterLogs.ProjectPathUpdaterListStarted(this.logger);
                var result = this.dotNetCommand.RunCommand(DotnetCommandConst.SolutionCommand, DotnetCommandConst.ListCommand);
                if (!result.IsSucceed()) return;

                var projectPaths = Scraper.ProjectPathUpdater.DiscoverProjectPath(result.Output);

                ProjectPathUpdaterLogs.ProjectPathUpdaterListEnded(this.logger);
                if (projectPaths is null || projectPaths.Count == 0)
                    return;

                foreach (var project in projects)
                    project.Path = projectPaths.First(p => p.Key == project.Name).Value;
            }
            finally
            {
                ProjectPathUpdaterLogs.ProjectPathUpdaterEnded(this.logger, stopWatch);
            }
        }
    }
}