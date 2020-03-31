using MeerkatUpdater.Core.Helpers;
using MeerkatUpdater.Core.Model.DotNetCommand;
using System;
using System.Threading.Tasks;

namespace MeerkatUpdater.Core.Runner
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
        /// <param name="execution"></param>
        /// <returns></returns>
        public static Result RunCommand(Execution execution)
        {
            if (execution is null)
                throw new ArgumentNullException(nameof(execution));

            using var process = ExecutionProcess.CreateNewProcess(execution);

            process.Start();
            var outputExecution = OutPutDotNetCommandExecution.FromStreamRead(process.StandardOutput);
            var errorExecution = OutPutDotNetCommandExecution.FromStreamRead(process.StandardError);

            var processExited = process.WaitForExit(execution.GetTotalMilisecondsForMaximumWait());
            if (!processExited)
            {
                process.Kill();
                return CreateAResultFromOutPutAndExitCode(outputExecution, errorExecution, Result.DefaultErrorExitCode);
            }

            Task.WaitAll(outputExecution.OutPutTask, errorExecution.OutPutTask);
            return CreateAResultFromOutPutAndExitCode(outputExecution, errorExecution, Result.DefaultSuccessExitCode);
        }

        private static Result CreateAResultFromOutPutAndExitCode(OutPutDotNetCommandExecution outputExecution, OutPutDotNetCommandExecution errorExecution, int exitCode) =>
            Result.FromStandardsTextAndExitCode(outputExecution.GetOutPutString(), errorExecution.GetOutPutString(), exitCode);
    }
}