using MeerkatUpdater.Core.Runner.Command.Common;
using MeerkatUpdater.Core.Runner.Command.DotNet;
using MeerkatUpdater.Core.Runner.Model.PackageInfo;
using System.Collections.Generic;
using System.Linq;

namespace MeerkatUpdater.Core.Runner.Command.DotNetUpdateProcess
{
    /// <summary>
    /// Updater for path on each project inside a solution
    /// </summary>
    public class ProjectPathUpdater
    {
        private readonly IDotNetCommand dotNetCommand;

        public ProjectPathUpdater(IDotNetCommand dotNetCommand)
        {
            this.dotNetCommand = dotNetCommand;
        }

        /// <summary>
        /// Based on the solution update the path for each project
        /// </summary>
        /// <param name="projects"></param>
        public void Execute(ref List<ProjectInfo> projects)
        {
            if (projects is null || projects.Count == 0)
                return;

            var result = this.dotNetCommand.RunCommand(DotnetCommandConst.SolutionCommand, DotnetCommandConst.ListCommand);
            if (!result.IsSucceed()) return;

            var projectPaths = Scraper.ProjectPathUpdater.DiscoverProjectPath(result.Output);

            if (projectPaths is null || projectPaths.Count == 0)
                return;

            foreach (var project in projects)
                project.Path = projectPaths.First(p => p.Key == project.Name).Value;
        }
    }
}