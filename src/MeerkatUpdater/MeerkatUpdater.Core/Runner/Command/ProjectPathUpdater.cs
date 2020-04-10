using MeerkatUpdater.Core.Runner.Command.Common;
using MeerkatUpdater.Core.Runner.Model.PackageInfo;
using System.Collections.Generic;
using System.Linq;

namespace MeerkatUpdater.Core.Runner.Command
{
    /// <summary>
    /// Updater for path on each project inside a solution
    /// </summary>
    public static class ProjectPathUpdater
    {
        /// <summary>
        /// Based on the solution update the path for each project
        /// </summary>
        /// <param name="projects"></param>
        public static void Execute(ref List<ProjectInfo> projects)
        {
            if (projects is null || projects.Count == 0)
                return;

            var result = DotNetCommand.RunCommand(DotnetCommandConst.SolutionCommand, DotnetCommandConst.ListCommand);
            if (!result.IsSuccess()) return;

            var projectPaths = Scraper.ProjectPathUpdater.DiscoverProjectPath(result.Output);

            if (projectPaths is null || projectPaths.Count == 0)
                return;

            foreach (var project in projects)
                project.Path = projectPaths.First(p => p.Key == project.Name).Value;
        }
    }
}