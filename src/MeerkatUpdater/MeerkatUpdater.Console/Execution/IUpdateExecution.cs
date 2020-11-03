using MeerkatUpdater.Console.Options;

namespace MeerkatUpdater.Console.Execution
{
    public interface IUpdateExecution
    {
        void Execute(ExecutionOptions? executionOptions);
    }
}