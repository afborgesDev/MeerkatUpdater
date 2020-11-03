using MeerkatUpdater.Console.Options.InputOptions;
using MeerkatUpdater.Core.Config.Model;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MeerkatUpdater.Console.Options
{
    public class ExecutionOptionsBuilder
    {
        private readonly ILogger<ExecutionOptionsBuilder> logger;
        private string? ymlConfigFile;
        private string? solutionPath;
        private IEnumerable<string>? nugetSources;
        private HashSet<SemanticVersion>? allowedVersionToUpdateOption;

        private ExecutionOptionsBuilder(ILogger<ExecutionOptionsBuilder> logger) => this.logger = logger;

        public static ExecutionOptionsBuilder StartBuild(ILogger<ExecutionOptionsBuilder> logger) => new ExecutionOptionsBuilder(logger);

        public ExecutionOptions? Build()
        {
            if (IsInvalidOptionCombinations())
                return default;

            if (!string.IsNullOrEmpty(this.ymlConfigFile))
                return ExecutionOptions.FromYmlConfig(this.ymlConfigFile);

            if (!string.IsNullOrEmpty(this.solutionPath))
                return ExecutionOptions.FromConfigurations(this.solutionPath, this.nugetSources, this.allowedVersionToUpdateOption);

            return default;
        }

        public ExecutionOptionsBuilder WithYmlConfig(YmlConfigInputOption? ymlConfigInputOption)
        {
            if (ymlConfigInputOption is null || string.IsNullOrEmpty(ymlConfigInputOption.YmlFile) ||
                !File.Exists(ymlConfigInputOption.YmlFile))
            {
                this.logger.LogInformation("Starting without YML config option. Could not find the file");
                return this;
            }

            this.ymlConfigFile = ymlConfigInputOption.YmlFile;

            return this;
        }

        public ExecutionOptionsBuilder WithSolutionPathConfig(SolutionPathInputOption? solutionPathInputOption)
        {
            if (solutionPathInputOption is null || string.IsNullOrEmpty(solutionPathInputOption.SolutionPath) ||
                !File.Exists(solutionPathInputOption.SolutionPath))
            {
                this.logger.LogInformation("Starting without Solution Path config option. Could not find the file");
                return this;
            }

            this.solutionPath = solutionPathInputOption.SolutionPath;

            return this;
        }

        public ExecutionOptionsBuilder WithNugetSourcesConfig(NugetSourcesOption? nugetSourcesOption)
        {
            if (nugetSourcesOption is null || nugetSourcesOption.Sources is null || !nugetSourcesOption.Sources.Any())
                return this;

            this.nugetSources = nugetSourcesOption.Sources;

            return this;
        }

        internal ExecutionOptionsBuilder WithAllowedVersionToUpdateConfig(AllowedVersionToUpdateOption? allowedVersionToUpdateOption)
        {
            if (allowedVersionToUpdateOption is null || allowedVersionToUpdateOption.AllowedVersionsToUpdate is null || !allowedVersionToUpdateOption.AllowedVersionsToUpdate.Any())
                return this;

            this.allowedVersionToUpdateOption = allowedVersionToUpdateOption.AllowedVersionsToUpdate;

            return this;
        }

        private bool IsInvalidOptionCombinations()
        {
            if (!string.IsNullOrEmpty(this.ymlConfigFile) && !string.IsNullOrEmpty(this.solutionPath))
            {
                this.logger.LogError("Shall use YML Config or Solution Path. Both configurations at the same execution aren't supported");
                return true;
            }

            return false;
        }
    }
}