using MeerkatUpdater.Core.Runner.Command.Common;
using MeerkatUpdater.Core.Runner.Model.DotNet;
using MeerkatUpdater.Core.Runner.Model.PackageInfo;
using System.Collections.Generic;
using System.Linq;

namespace MeerkatUpdater.Core.Runner.Command
{
    public static class ProjectPathUpdater
    {
        public static void Execute(string workDirectory, ref List<ProjectInfo> projects)
        {
            if (string.IsNullOrWhiteSpace(workDirectory))
                return;

            if (projects is null || projects.Count == 0)
                return;

            var execution = Execution.FromDirectoryAndArguments(workDirectory, DotnetCommandConst.SolutionCommand, DotnetCommandConst.ListCommand);
            var result = DotNetCommand.RunCommand(execution);

            if (!result.IsSuccess()) return;

            var projectPaths = Scraper.ProjectPathUpdater.DiscoverProjectPath(result.Output);

            if (projectPaths is null || projectPaths.Count == 0)
                return;

            foreach (var project in projects)
                project.Path = projectPaths.First(p => p.Key == project.Name).Value;
        }
    }
}