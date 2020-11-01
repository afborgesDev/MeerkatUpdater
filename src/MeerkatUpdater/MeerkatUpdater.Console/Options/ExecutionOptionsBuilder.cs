using MeerkatUpdater.Console.Options.InputOptions;
using Microsoft.Extensions.Logging;
using System.IO;

namespace MeerkatUpdater.Console.Options
{
    public class ExecutionOptionsBuilder
    {
        private readonly ILogger<ExecutionOptionsBuilder> logger;
        private string ymlConfigFile;
        private string solutionPath;

        private ExecutionOptionsBuilder(ILogger<ExecutionOptionsBuilder> logger) => this.logger = logger;

        public static ExecutionOptionsBuilder StartBuild(ILogger<ExecutionOptionsBuilder> logger) => new ExecutionOptionsBuilder(logger);

        public ExecutionOptions? Build()
        {
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
    }
}