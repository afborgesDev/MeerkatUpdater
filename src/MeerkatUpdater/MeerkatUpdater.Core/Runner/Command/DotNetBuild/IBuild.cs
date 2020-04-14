using System;

namespace MeerkatUpdater.Core.Runner.Command.DotNetBuild
{
    /// <summary>
    /// Wrap the dotnet build command
    /// </summary>
    public interface IBuild
    {
        /// <summary>
        /// Exceute the dotnet build command <br/>
        /// <see href="https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-build">microsoft documentation</see>
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="NullReferenceException"></exception>
        bool Execute();
    }
}