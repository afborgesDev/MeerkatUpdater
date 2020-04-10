using McMaster.Extensions.CommandLineUtils;
using MeerkatUpdater.Core.Config;
using MeerkatUpdater.Core.Runner.Command.Common;
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
        /// <param name="arguments"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static Process CreateNewProcess(params string[] arguments)
        {
            ArgumentsValidation.Validate(arguments);
            return new Process { StartInfo = CreateProcessStartInfo(ConfigManager.GetExecutionConfigurations().SolutionPath, arguments) };
        }

        private static ProcessStartInfo CreateProcessStartInfo(string? solutionPath, string[] args)
        {
            if (string.IsNullOrWhiteSpace(solutionPath))
                throw new ArgumentException(DefaultMessages.ValidateArgumentToWorkDotNetCommand, nameof(solutionPath));

            return new ProcessStartInfo(DotNetExe.FullPathOrDefault(), string.Join(DefaultSeparatorForJoinArguments, args))
            {
                WorkingDirectory = Path.GetDirectoryName(solutionPath),
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };
        }
    }
}