using MeerkatUpdater.Console.Options;
using Microsoft.Extensions.Logging;

namespace MeerkatUpdater.Console.Execution
{
    public class UpdateExecution : IUpdateExecution
    {
        private readonly ILogger<UpdateExecution> logger;

        public UpdateExecution(ILogger<UpdateExecution> logger)
        {
            this.logger = logger;
        }

        public void Execute(ExecutionOptions? executionOptions)
        {
            if (IsExecutionOptionsInvalid(executionOptions))
                return;
        }

        private bool IsExecutionOptionsInvalid(ExecutionOptions? executionOptions)
        {
            if (executionOptions is null)
            {
                this.logger.LogError("Could not perform the process. Missing configurations");
                return true;
            }

            return false;
        }
    }
}