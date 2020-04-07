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
    }
}