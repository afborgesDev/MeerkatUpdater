namespace MeerkatUpdater.Core.Runner.Model.DotNet
{
    /// <summary>
    /// The result of a dotnet command execution
    /// </summary>
    public sealed class Result
    {
        /// <summary>
        /// Representation of the default Exit Code for a successful dotnet command execution
        /// </summary>
        public const int DefaultSuccessExitCode = 1;

        /// <summary>
        /// Representation of the default fail exist code for a dotnet execution
        /// </summary>
        public const int DefaultErrorExitCode = -1;

        /// <summary>
        /// The complete standard output from the execution
        /// </summary>
        public string? Output { get; set; }

        /// <summary>
        /// The complete standard errors from the execution
        /// </summary>
        public string? Errors { get; set; }

        /// <summary>
        /// The exit code as result from the dotnet command
        /// </summary>
        public int ExitCode { get; set; }

        /// <summary>
        /// Creates a new instance of Result by passing the outputs and the exit code
        /// </summary>
        /// <param name="output"></param>
        /// <param name="errors"></param>
        /// <param name="exitCode"></param>
        /// <returns></returns>
        public static Result FromStandardsTextAndExitCode(string? output, string? errors, int exitCode) =>
            new Result() { Output = output, Errors = errors, ExitCode = exitCode };

        /// <summary>
        /// Check if the exit code is the success one
        /// </summary>
        public bool IsSucceed()
        {
            if (ExitCode == DefaultSuccessExitCode)
                return true;

            return ExitCode != DefaultErrorExitCode && string.IsNullOrWhiteSpace(Errors) && !string.IsNullOrWhiteSpace(Output);
        }
    }
}