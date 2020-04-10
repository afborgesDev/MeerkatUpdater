using System;

namespace MeerkatUpdater.Core.Runner.Command.Common
{
    internal static class ArgumentsValidation
    {
        internal static void Validate(params string[] arguments)
        {
            if (arguments is null || arguments.Length == 0)
                throw new ArgumentNullException(nameof(arguments));
        }
    }
}