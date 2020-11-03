using MeerkatUpdater.Core.Runner.Command.Common;
using Microsoft.Extensions.Logging;
using System;

namespace MeerkatUpdater.Core.Runner.Command.DotNetCommandUpdate
{
    internal static class UpdateLog
    {
        internal static Action<ILogger, string, int, int, Exception?> _updateStarted = LoggerMessage.Define<string, int, int>(
                LogDefaults.DefaultStartEndEventLogLevel,
                CommandEventId.DotNetUpdateStarted,
                "Starting to update for the allowed versions: {allowedVersions} the total of {numberOfPacakgesToUpdate} packages inside {toUpdateProjectInfo.Count} projects"
            );

        internal static Action<ILogger, Exception?> _noAllowedVersionToUpdateAvaliable = LoggerMessage.Define(
                LogDefaults.DefaultStartEndEventLogLevel,
                CommandEventId.DotNetUpdateEnded,
                "None allowed versions to update was configurated"
            );

        internal static Action<ILogger, Exception?> _noneOutdatedPackageFound = LoggerMessage.Define(
                LogDefaults.DefaultStartEndEventLogLevel,
                CommandEventId.DotNetUpdateEnded,
                "None outdated package was detected"
            );

        internal static Action<ILogger, string, string, Exception?> _failDuringUpdatePackage = LoggerMessage.Define<string, string>(
                LogLevel.Error,
                CommandEventId.DotNetUpdateEnded,
                "A error ocurr when was trying to update the package: {packageName} inside the project {projectName}"
            );

        internal static Action<ILogger, string, Exception?> _updatedEnded = LoggerMessage.Define<string>(
                LogDefaults.DefaultStartEndEventLogLevel,
                CommandEventId.ProjectPathUpdaterEnded,
                "The updated process ended: "
            );
    }
}