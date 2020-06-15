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
    }
}