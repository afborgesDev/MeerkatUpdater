using Microsoft.Extensions.Logging;

namespace MeerkatUpdater.Core.Runner.Command.Common
{
    /// <summary>
    /// Define the Id for all commands to be used at the logg
    /// </summary>
    public static class CommandEventId
    {
        /// <summary>
        /// The EventId for Starting the dotnet build command <see cref="DotNetBuild.IBuild"/>
        /// </summary>
        public static readonly EventId DotNetBuildStarted = new EventId(1, nameof(DotNetBuildStarted));

        /// <summary>
        /// The EventId for ended the dotnet build command <see cref="DotNetBuild.IBuild"/>
        /// </summary>
        public static readonly EventId DotNetBuildEnded = new EventId(2, nameof(DotNetBuildEnded));

        /// <summary>
        /// The EventId for Starting the dotnet clean command <see cref="DotNetClean.IClean"/>
        /// </summary>
        public static readonly EventId DotNetCleanStarted = new EventId(3, nameof(DotNetCleanStarted));

        /// <summary>
        /// The EventId for endind the dotnet clean command <see cref="DotNetClean.IClean"/>
        /// </summary>
        public static readonly EventId DotNetCleanEnded = new EventId(4, nameof(DotNetCleanEnded));

        /// <summary>
        /// The EventId for Starting The Project Path Updater <see cref="DotNetCommandProjectPathUpdater.IProjectPathUpdater"/>
        /// </summary>
        public static readonly EventId ProjectPathUpdaterStarted = new EventId(5, nameof(ProjectPathUpdaterStarted));

        /// <summary>
        /// The EventId for Starting The List projects inside the project Path Updater <see cref="DotNetCommandProjectPathUpdater.IProjectPathUpdater"/>
        /// </summary>
        public static readonly EventId ProjectPathUpdaterListCommandStarted = new EventId(6, nameof(ProjectPathUpdaterListCommandStarted));

        /// <summary>
        /// The EventId for ended The List projects inside the project Path Updater <see cref="DotNetCommandProjectPathUpdater.IProjectPathUpdater"/>
        /// </summary>
        public static readonly EventId ProjectPathUpdaterListCommandEnded = new EventId(7, nameof(ProjectPathUpdaterListCommandEnded));

        /// <summary>
        /// The EventId for Starting The discover porject path inside the project Path Updater <see cref="DotNetCommandProjectPathUpdater.IProjectPathUpdater"/>
        /// </summary>
        public static readonly EventId ProjectPathUpdaterDiscoverProjectPathStarted = new EventId(8, nameof(ProjectPathUpdaterDiscoverProjectPathStarted));

        /// <summary>
        /// The EventId for ednded The discover porject path inside the project Path Updater <see cref="DotNetCommandProjectPathUpdater.IProjectPathUpdater"/>
        /// </summary>
        public static readonly EventId ProjectPathUpdaterDiscoverProjectPathEnded = new EventId(9, nameof(ProjectPathUpdaterDiscoverProjectPathEnded));

        /// <summary>
        /// The EventId for ended The Project Path Updater <see cref="DotNetCommandProjectPathUpdater.IProjectPathUpdater"/>
        /// </summary>
        public static readonly EventId ProjectPathUpdaterEnded = new EventId(10, nameof(ProjectPathUpdaterEnded));

        /// <summary>
        /// The EventId for starting the Update pacakges command <see cref="DotNetCommandUpdate.IUpdate"/>
        /// </summary>
        public static readonly EventId DotNetUpdateStarted = new EventId(11, nameof(DotNetUpdateStarted));

        /// <summary>
        /// The EventId for ended the Update pacakges command <see cref="DotNetCommandUpdate.IUpdate"/>
        /// </summary>
        public static readonly EventId DotNetUpdateEnded = new EventId(12, nameof(DotNetUpdateEnded));
    }
}