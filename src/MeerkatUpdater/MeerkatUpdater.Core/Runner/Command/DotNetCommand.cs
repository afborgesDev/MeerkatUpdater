using MeerkatUpdater.Core.Config;
using MeerkatUpdater.Core.Runner.Command.Common;
using MeerkatUpdater.Core.Runner.Helpers;
using MeerkatUpdater.Core.Runner.Model.DotNet;
using System;
using System.Threading.Tasks;

namespace MeerkatUpdater.Core.Runner.Command
{
    /// <summary>
    /// Run a dotnet command and returns the execution results such as <br/>
    /// - Output <br/>
    /// - Errors <br/>
    /// - ExitCode
    /// </summary>
    public static class DotNetCommand
    {
        /// <summary>
        /// Executes a dotnet command and takes the output and error
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public static Result RunCommand(params string[] arguments)
        {
            ArgumentsValidation.Validate(arguments);

            using var process = ExecutionProcess.CreateNewProcess(arguments);

            process.Start();
            var outputExecution = OutPutDotNetCommandExecution.FromStreamReader(process.StandardOutput);
            var errorExecution = OutPutDotNetCommandExecution.FromStreamReader(process.StandardError);
            var waitMiliseconds = ConfigManager.GetExecutionConfigurations().GetWaitMiliSeconds();

            var processExited = process.WaitForExit(waitMiliseconds);
            if (!processExited)
            {
                process.Kill();
                return CreateAResultFromOutPutAndExitCode(outputExecution, errorExecution, Result.DefaultErrorExitCode);
            }

            if (outputExecution.OutPutTask is null || errorExecution.OutPutTask is null)
                throw new NullReferenceException(DefaultMessages.ValidationOnStandardOutputsDotNetCommand);

            Task.WaitAll(outputExecution.OutPutTask, errorExecution.OutPutTask);
            return CreateAResultFromOutPutAndExitCode(outputExecution, errorExecution, Result.DefaultSuccessExitCode);
        }

        private static Result CreateAResultFromOutPutAndExitCode(OutPutDotNetCommandExecution outputExecution, OutPutDotNetCommandExecution errorExecution, int exitCode) =>
            Result.FromStandardsTextAndExitCode(outputExecution.GetOutPutString(), errorExecution.GetOutPutString(), exitCode);
    }
}