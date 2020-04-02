using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MeerkatUpdater.Core.Model.DotNetCommand
{
    /// <summary>
    /// Represents the required params to execute a dotnet command
    /// </summary>
    public sealed class Execution
    {
        private const double NotAllowedMaximumWait = 0;
        private static readonly TimeSpan DefaultMaximumWait = TimeSpan.FromSeconds(15);

        /// <summary>
        /// The full path to dotnet command start to work
        /// </summary>
        public string? WorkDirectory { get; set; }

        /// <summary>
        /// The maximun time to wait until abort a execution
        /// </summary>
        public TimeSpan MaximumWait { get; set; }

        /// <summary>
        /// The complete list of arguments to be forward into the dotnet command
        /// </summary>
        public List<string> Arguments { get; } = new List<string>();

        /// <summary>
        /// Creates a Execution using default for maximumWait and the current directory <br/>
        /// Just taking the arguments <br/>
        /// This method takes the WorkDirectory directly from <see cref="Directory.GetCurrentDirectory"/>
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">If the arguments has zero items</exception>
        /// <exception cref="ArgumentException">If there's any argument that is a blank or white space</exception>
        public static Execution FromCurrentDirrectoryAndArgument(params string[] arguments)
        {
            var execution = NewExecution(Directory.GetCurrentDirectory(), DefaultMaximumWait);
            ValidateAndAddArguments(ref execution, arguments);
            return execution;
        }

        /// <summary>
        /// Creates a Execution using the default maximum wait time <see cref="DefaultMaximumWait"></see> <br/>
        ///
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">If the arguments has zero items</exception>
        /// <exception cref="ArgumentException">If there's any argument that is a blank or white space</exception>
        public static Execution FromDirectoryAndArguments(string directory, params string[] arguments)
        {
            var execution = NewExecution(directory, DefaultMaximumWait);
            ValidateAndAddArguments(ref execution, arguments);
            return execution;
        }

        /// <summary>
        /// Creates a Execution with all custom parameters
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="maximumWait"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">If the arguments has zero items</exception>
        /// <exception cref="ArgumentException">If there's any argument that is a blank or white space</exception>
        public static Execution FromDirectoryWaitTimeAndArguments(string directory, TimeSpan maximumWait, params string[] arguments)
        {
            var execution = NewExecution(directory, maximumWait);
            ValidateAndAddArguments(ref execution, arguments);
            return execution;
        }

        /// <summary>
        /// Get in seconds the MaximumWait time converted to int
        /// </summary>
        /// <returns></returns>
        public int GetTotalMilisecondsForMaximumWait() => Convert.ToInt32(MaximumWait.TotalMilliseconds);

        private static void ValidateAndAddArguments(ref Execution execution, params string[] arguments)
        {
            if (string.IsNullOrWhiteSpace(execution.WorkDirectory))
                throw new NullReferenceException(DefaultMessages.ValidateArgumentToWorkDotNetCommand);

            if (execution.MaximumWait == default || execution.MaximumWait.TotalSeconds <= NotAllowedMaximumWait)
                throw new NullReferenceException(DefaultMessages.MaximumWaitNotEnougthDotNetCommand);

            ValidateArguments(arguments);
            execution.Arguments.AddRange(arguments);
        }

        private static void ValidateArguments(string[] arguments)
        {
            if (arguments?.Length == 0)
                throw new ArgumentNullException(nameof(arguments));

            if (arguments.Any(x => string.IsNullOrWhiteSpace(x)))
                throw new ArgumentException(DefaultMessages.ArgumentExeptionForValidationDotNetCommand);
        }

        private static Execution NewExecution(string directory, TimeSpan maximumWait) => new Execution
        {
            WorkDirectory = directory,
            MaximumWait = maximumWait
        };
    }
}