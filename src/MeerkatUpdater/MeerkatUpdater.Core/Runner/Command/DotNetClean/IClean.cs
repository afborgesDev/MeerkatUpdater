namespace MeerkatUpdater.Core.Runner.Command.DotNetClean
{
    /// <summary>
    /// Run the Clean Command
    /// </summary>
    public interface IClean
    {
        /// <summary>
        /// Executes the dotnet clean command <br/>
        /// For more information about the command, see: <see href="https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-clean">dotnet clean documentation</see>
        /// </summary>
        void Execute();
    }
}