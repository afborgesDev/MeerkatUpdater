using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace MeerkatUpdater.Core.Util
{
    /// <summary>
    /// import from https://github.com/natemcmaster/CommandLineUtils/blob/main/src/CommandLineUtils/Utilities/DotNetExe.cs
    /// </summary>
    public static class DotNetExe
    {
        private const string FileName = "dotnet";

        static DotNetExe()
        {
            FullPath = TryFindDotNetExePath();
        }

        /// <summary>
        /// The full filepath to the .NET Core CLI executable.
        /// <para>
        /// May be <c>null</c> if the CLI cannot be found.
        /// </para>
        /// </summary>
        /// <returns>The path or null</returns>
        /// <seealso cref="FullPathOrDefault" />
        public static string? FullPath { get; }

        /// <summary>
        /// Finds the full filepath to the .NET Core CLI executable,
        /// or returns a string containing the default name of the .NET Core muxer ('dotnet').
        /// <returns>The path or a string named 'dotnet'</returns>
        /// </summary>
        public static string FullPathOrDefault()
            => FullPath ?? FileName;

        private static string? TryFindDotNetExePath()
        {
            var fileName = FileName;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                fileName += ".exe";
            }

            var mainModule = Process.GetCurrentProcess().MainModule;
            if (mainModule != null && !string.IsNullOrEmpty(mainModule.FileName) &&
                Path.GetFileName(mainModule.FileName).Equals(fileName, StringComparison.OrdinalIgnoreCase))
            {
                return mainModule.FileName;
            }

            var dotnetRoot = Environment.GetEnvironmentVariable("DOTNET_ROOT");
            return !string.IsNullOrEmpty(dotnetRoot)
                ? Path.Combine(dotnetRoot, fileName)
                : null;
        }
    }
}