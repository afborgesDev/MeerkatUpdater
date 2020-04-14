namespace MeerkatUpdater.Core.Runner.Command.DotNetContProject
{
    /// <summary>
    /// Run the commands that provide the number of projects inside a solution
    /// </summary>
    public interface ICountProject
    {
        /// <summary>
        /// Executes the dotnet command sln list to see more information see <br/>
        /// <see href="https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-sln">microsoft documentation</see>
        /// </summary>
        /// <returns></returns>
        int? Execute();
    }
}