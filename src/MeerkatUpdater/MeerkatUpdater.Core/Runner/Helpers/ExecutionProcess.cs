using McMaster.Extensions.CommandLineUtils;
using MeerkatUpdater.Core.Runner.Model.DotNet;
using System;
using System.Diagnostics;
using System.IO;

namespace MeerkatUpdater.Core.Runner.Helpers
{
    /// <summary>
    /// Helper for create and configure process to work with dotnet command cli
    /// </summary>
    public static class ExecutionProcess
    {
        private const string DefaultSeparatorForJoinArguments = " ";

        /// <summary>
        /// Create and configure a process to execute the dotnet command by cli
        /// </summary>
        /// <param name="execution"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static Process CreateNewProcess(Execution execution)
        {
            if (execution is null)
                throw new ArgumentNullException(nameof(execution));

            return new Process { StartInfo = CreateProcessStartInfo(execution.WorkDirectory, execution.Arguments.ToArray()) };
        }

        private static ProcessStartInfo CreateProcessStartInfo(string? workingDirectory, string[] args)
        {
            if (string.IsNullOrWhiteSpace(workingDirectory))
                throw new ArgumentException(DefaultMessages.ValidateArgumentToWorkDotNetCommand, nameof(workingDirectory));

            return new ProcessStartInfo(DotNetExe.FullPathOrDefault(), string.Join(DefaultSeparatorForJoinArguments, args))
            {
                WorkingDirectory = Path.GetDirectoryName(workingDirectory),
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };
        }
    }
}