using MeerkatUpdater.Core.Config.Manager;
using MeerkatUpdater.Core.Config.Model;
using MeerkatUpdater.Core.Runner.Command.Common;
using MeerkatUpdater.Core.Runner.Command.DotNet;
using MeerkatUpdater.Core.Runner.Model.PackageInfo;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MeerkatUpdater.Core.Runner.Command.DotNetCommandUpdate
{
    /// <summary>
    /// Update a solution based on semantic allowed version to update
    /// </summary>
    public class Update : IUpdate
    {
        private const string AddCommand = "add";
        private const int MaxDegreeParallelForPackagesToUpdate = 10;
        private static readonly ParallelOptions parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = MaxDegreeParallelForPackagesToUpdate };
        private readonly IConfigManager configManager;
        private readonly IDotNetCommand dotNetCommand;
        private readonly ILogger<Update> logger;

        /// <summary>
        /// Default DI constructor
        /// </summary>
        /// <param name="configManager"></param>
        /// <param name="dotNetCommand"></param>
        /// <param name="logger"></param>
        public Update(IConfigManager configManager, IDotNetCommand dotNetCommand, ILogger<Update> logger) =>
            (this.configManager, this.dotNetCommand, this.logger) = (configManager, dotNetCommand, logger);

        /// <summary>
        /// Execute the update by using add package to sulution command
        /// </summary>
        /// <param name="toUpdateProjectInfo"></param>
        public void Execute(ref List<ProjectInfo> toUpdateProjectInfo)
        {
            var toUpdateVersion = this.configManager.GetConfigurations().UpdateConfigurations?.AllowedVersionsToUpdate;

            if (toUpdateVersion is null || toUpdateVersion.Count == 0)
            {
                this.logger.LogInformation(DefaultMessages.LOG_NoneAllowedVersionToUpdate);
                return;
            }

            if (toUpdateProjectInfo is null || toUpdateProjectInfo.Count == 0)
            {
                this.logger.LogInformation(DefaultMessages.LOG_NoneOutDatedPackages);
                return;
            }

            var allowedVersions = toUpdateVersion.Select(x => Enum.GetName(typeof(SemanticVersion), x)).Aggregate((old, current) => $"{old}, {current}");
            var numberOfPacakgesToUpdate = toUpdateProjectInfo.SelectMany(x => x.InstalledPackages).Count(x => toUpdateVersion.Any(t => t == x.SemanticVersionChange));

            this.logger.LogInformation($"Starting to update for the allowed versions: {allowedVersions} the total of {numberOfPacakgesToUpdate} packages inside {toUpdateProjectInfo.Count} projects");
            var errorCount = 0;
            var updatedPackagesCount = 0;

            foreach (var projectInfo in toUpdateProjectInfo)
            {
                var toUpdatePackages = projectInfo.InstalledPackages
                                                  .Where(x => toUpdateVersion.Any(t => t == x.SemanticVersionChange));

                if (!toUpdatePackages.Any())
                    continue;

                updatedPackagesCount += toUpdatePackages.Count();

                Parallel.ForEach(toUpdatePackages, parallelOptions, packageToUpdate =>
                {
                    try
                    {
                        var hasUpdated = DoUpdatePackage(projectInfo.Path, packageToUpdate.Name);
                        if (hasUpdated)
                        {
                            packageToUpdate.UpdatedAt = DateTime.UtcNow;
                            packageToUpdate.OldVersion = packageToUpdate.Current;
                            packageToUpdate.Current = packageToUpdate.Latest;
                        }
                    }
                    catch (Exception e)
                    {
                        this.logger.LogError(e, $"A error ocurr when was trying to update the package: {packageToUpdate.Name} inside the project {projectInfo.Name}");
                        Interlocked.Increment(ref errorCount);
                    }
                });
            }
        }

        private bool DoUpdatePackage(string? projectPath, string? libName)
        {
            if (string.IsNullOrEmpty(projectPath))
                throw new ArgumentNullException(nameof(projectPath));

            if (string.IsNullOrEmpty(libName))
                throw new ArgumentNullException(nameof(libName));

            //runModel.Options.NugetSourceToStringParam(),
            var runResult = this.dotNetCommand.RunCommand(AddCommand, projectPath, DotnetCommandConst.PackageCommand, libName);
            return runResult.IsSucceed();
        }
    }
}