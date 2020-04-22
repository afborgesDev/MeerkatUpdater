using MeerkatUpdater.Core.Config.Model;
using System;

namespace MeerkatUpdater.Core.Runner.Model.PackageInfo
{
    /// <summary>
    /// Informations about the instaled package on a project
    /// </summary>
    public sealed class InstalledPackage
    {
        /// <summary>
        /// The package name
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// The current version of the package
        /// </summary>
        public VersionInfo? Current { get; set; }

        /// <summary>
        /// The latest version of the package
        /// </summary>
        public VersionInfo? Latest { get; set; }

        /// <summary>
        /// The semantic version used to target the update
        /// </summary>
        public SemanticVersion? SemanticVersionChange { get; set; }

        /// <summary>
        /// Indicates when the package was updated
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Convert the string values to the InstalledPackage and versionInfo
        /// </summary>
        /// <param name="name"></param>
        /// <param name="currentVersion"></param>
        /// <param name="latestVersion"></param>
        /// <returns></returns>
        public static InstalledPackage FromStringValues(string? name, string? currentVersion, string? latestVersion)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            if (string.IsNullOrEmpty(currentVersion))
                throw new ArgumentNullException(nameof(currentVersion));

            if (string.IsNullOrEmpty(latestVersion))
                throw new ArgumentNullException(nameof(latestVersion));

            return new InstalledPackage
            {
                Name = name,
                Current = VersionInfo.FromVersionText(currentVersion),
                Latest = VersionInfo.FromVersionText(latestVersion)
            };
        }
    }
}