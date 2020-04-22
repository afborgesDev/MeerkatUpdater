using System;
using System.Linq;

namespace MeerkatUpdater.Core.Runner.Model.PackageInfo
{
    /// <summary>
    /// Information about the instaled package
    /// </summary>
    public sealed class VersionInfo
    {
        private const string AlphaIndicator = "-alpha";
        private const string BetaIndicator = "-beta";
        private const string ReleaseCanditateIndicator = "-rc";

        private readonly string[] NotEstableVersionIndicators = new string[3] { AlphaIndicator, BetaIndicator, ReleaseCanditateIndicator };

        /// <summary>
        /// The number of the version
        /// </summary>
        public string? Version { get; set; }

        /// <summary>
        /// Creates a new versionInfo by using the text version
        /// </summary>
        /// <param name="versionText"></param>
        public static VersionInfo FromVersionText(string? versionText) => new VersionInfo { Version = versionText };

        /// <summary>
        /// Indicates if the version is istable or not
        /// </summary>
        public bool IsEstable() => NotEstableVersionIndicators.Any(x => Version?.Contains(x, StringComparison.InvariantCultureIgnoreCase) == true);
    }
}