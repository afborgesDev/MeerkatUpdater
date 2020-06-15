using MeerkatUpdater.Core.Runner.Command.Common;
using Microsoft.Extensions.Logging;
using System;

namespace MeerkatUpdater.Core.Runner.Command.DotNetClean
{
    internal static class CleanLogs
    {
        private static readonly Action<ILogger, Exception?> _cleanStarting = LoggerMessage.Define(
                LogDefaults.DefaultStartEndEventLogLevel,
                CommandEventId.DotNetCleanStarted,
                "Starting to clear the solution"
            );

        private static readonly Action<ILogger, string, Exception?> _cleanEnded = LoggerMessage.Define<string>(
                LogDefaults.DefaultStartEndEventLogLevel,
                CommandEventId.DotNetCleanEnded,
                "Clean solution ended in {formattedDuration} duration."
            );

        internal static void CleanStarted(ILogger logger) => _cleanStarting(logger, default);

        internal static void CleanEnded(ILogger logger, ValueStopwatch stopwatch) => _cleanEnded(logger, stopwatch.GetStringFullTimeFormated(), default);
    }
}