using System;

namespace MeerkatUpdater.Core.Runner.Command.Common
{
    /// <summary>
    /// Internal validation for arguments to be used when trigger a command
    /// </summary>
    public static class ArgumentsValidation
    {
        /// <summary>
        /// Validate if ther's any argument to be used on commands
        /// </summary>
        /// <param name="arguments"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void Validate(params string[] arguments)
        {
            if (arguments is null || arguments.Length == 0)
                throw new ArgumentNullException(nameof(arguments));
        }
    }
}