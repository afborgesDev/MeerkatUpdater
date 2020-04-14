using MeerkatUpdater.Core.Runner.Model.DotNet;

namespace MeerkatUpdater.Core.Runner.Command.DotNet
{
    /// <summary>
    /// Run a dotnet command and returns the execution results such as <br/>
    /// - Output <br/>
    /// - Errors <br/>
    /// - ExitCode
    /// </summary>
    public interface IDotNetCommand
    {
        /// <summary>
        /// Executes a dotnet command and takes the output and error
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns></returns>
        Result RunCommand(params string[] arguments);
    }
}