using MeerkatUpdater.Core.Runner.Command.Common;
using Microsoft.Extensions.Logging;
using System;

namespace MeerkatUpdater.Core.Runner.Command.DotNetCommandProjectPathUpdater
{
    internal static class ProjectPathUpdaterLogs
    {
        private static readonly Action<ILogger, Exception?> _projectPathUpdaterStarted = LoggerMessage.Define(
                LogDefaults.DefaultStartEndEventLogLevel,
                CommandEventId.ProjectPathUpdaterDiscoverProjectPathStarted,
                "Starting to discover the project path"
            );

        private static readonly Action<ILogger, Exception?> _projectPathUpdaterListStarted = LoggerMessage.Define(
                LogDefaults.DefaultStartEndEventLogLevel,
                CommandEventId.ProjectPathUpdaterListCommandStarted,
                "List project inside solution started"
            );

        private static readonly Action<ILogger, Exception?> _projectPathUpdaterListEnded = LoggerMessage.Define(
                LogDefaults.DefaultStartEndEventLogLevel,
                CommandEventId.ProjectPathUpdaterListCommandEnded,
                "List project inside solution end"
            );

        private static readonly Action<ILogger, string, Exception?> _projectPathUpdaterEnded = LoggerMessage.Define<string>(
                LogDefaults.DefaultStartEndEventLogLevel,
                CommandEventId.ProjectPathUpdaterDiscoverProjectPathEnded,
                "Ended discover the project path: {formatted}"
            );

        internal static void ProjectPathUpdaterStarted(ILogger logger) => _projectPathUpdaterStarted(logger, default);

        internal static void ProjectPathUpdaterListStarted(ILogger logger) => _projectPathUpdaterListStarted(logger, default);

        internal static void ProjectPathUpdaterListEnded(ILogger logger) => _projectPathUpdaterListEnded(logger, default);

        internal static void ProjectPathUpdaterEnded(ILogger logger, ValueStopwatch valueStopwatch) => _projectPathUpdaterEnded(logger, valueStopwatch.GetStringFullTimeFormated(), default);
    }
}