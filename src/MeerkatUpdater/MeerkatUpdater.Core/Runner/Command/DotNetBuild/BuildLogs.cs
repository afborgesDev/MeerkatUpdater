using MeerkatUpdater.Core.Runner.Command.Common;
using Microsoft.Extensions.Logging;
using System;

namespace MeerkatUpdater.Core.Runner.Command.DotNetBuild
{
    internal static class BuildLogs
    {
        private static readonly Action<ILogger, Exception?> _buildStarted = LoggerMessage.Define(
            LogDefaults.DefaultStartEndEventLogLevel,
            CommandEventId.DotNetBuildStarted,
            "Starting to build");

        private static readonly Action<ILogger, string, Exception?> _buildEnded = LoggerMessage.Define<string>(
            LogDefaults.DefaultStartEndEventLogLevel,
            CommandEventId.DotNetBuildEnded,
            "Build ended in {formattedDuration} duration.");

        internal static void BuildStarted(ILogger logger) => _buildStarted(logger, default);

        internal static void BuildEnded(ILogger logger, ValueStopwatch stopwatch) => _buildEnded(logger, stopwatch.GetStringFullTimeFormated(), default);
    }
}