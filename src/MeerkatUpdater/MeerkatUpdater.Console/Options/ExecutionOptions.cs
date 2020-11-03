using MeerkatUpdater.Core.Config.Model;
using System.Collections.Generic;

namespace MeerkatUpdater.Console.Options
{
    public class ExecutionOptions
    {
        public string? YmlConfig { get; set; }
        public string? SolutionPath { get; set; }
        public IEnumerable<string>? NugetSources { get; private set; }
        public HashSet<SemanticVersion>? AllowedVersionToUpdate { get; private set; }

        public static ExecutionOptions? FromYmlConfig(string? ymlConfig)
        {
            if (string.IsNullOrEmpty(ymlConfig))
                return default;

            return new ExecutionOptions { YmlConfig = ymlConfig };
        }

        public static ExecutionOptions? FromConfigurations(string? solutionPath, IEnumerable<string>? nugetSources, HashSet<SemanticVersion>? allowedVersionsToUpdate)
        {
            if (string.IsNullOrEmpty(solutionPath))
                return default;

            return new ExecutionOptions
            {
                SolutionPath = solutionPath,
                NugetSources = nugetSources,
                AllowedVersionToUpdate = allowedVersionsToUpdate
            };
        }
    }
}