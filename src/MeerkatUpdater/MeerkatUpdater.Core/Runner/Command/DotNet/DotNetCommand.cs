using MeerkatUpdater.Core.Config.Manager;
using MeerkatUpdater.Core.Runner.Command.Common;
using MeerkatUpdater.Core.Runner.Helpers;
using MeerkatUpdater.Core.Runner.Model.DotNet;
using System.Threading.Tasks;

namespace MeerkatUpdater.Core.Runner.Command.DotNet
{
    /// <summary>
    /// Run a dotnet command and returns the execution results such as <br/>
    /// - Output <br/>
    /// - Errors <br/>
    /// - ExitCode
    /// </summary>
    public class DotNetCommand : IDotNetCommand
    {
        private readonly IConfigManager configManager;

        /// <summary>
        /// The defautl DI constructor
        /// </summary>
        /// <param name="configManager"></param>
        public DotNetCommand(IConfigManager configManager) => this.configManager = configManager;

        /// <summary>
        /// Executes a dotnet command and takes the output and error
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public Result RunCommand(params string[] arguments)
        {
            ArgumentsValidation.Validate(arguments);

            var solutionPath = this.configManager.GetConfigurations().SolutionPath;
            using var process = ExecutionProcess.CreateNewProcess(solutionPath, arguments);

            process.Start();
            var outputExecution = OutPutDotNetCommandExecution.FromStreamReader(process.StandardOutput);
            var errorExecution = OutPutDotNetCommandExecution.FromStreamReader(process.StandardError);
            var waitMiliseconds = this.configManager.GetWaitMiliSeconds();

            var processExited = process.WaitForExit(waitMiliseconds);
            if (!processExited)
            {
                process.Kill();
                return CreateAResultFromOutPutAndExitCode(outputExecution, errorExecution, Result.DefaultErrorExitCode);
            }

#pragma warning disable CS8604 // Possible null reference argument.
            Task.WaitAll(outputExecution.OutPutTask, errorExecution.OutPutTask);
#pragma warning restore CS8604 // Possible null reference argument.
            return CreateAResultFromOutPutAndExitCode(outputExecution, errorExecution, Result.DefaultSuccessExitCode);
        }

        private static Result CreateAResultFromOutPutAndExitCode(OutPutDotNetCommandExecution outputExecution, OutPutDotNetCommandExecution errorExecution, int exitCode) =>
            Result.FromStandardsTextAndExitCode(outputExecution.GetOutPutString(), errorExecution.GetOutPutString(), exitCode);
    }
}