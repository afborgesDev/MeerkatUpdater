namespace MeerkatUpdater.Core.Runner.Model.PackageInfo
{
    /// <summary>
    /// Determine the version of the package
    /// </summary>
    public enum SemanticVersion
    {
        /// <summary>
        /// Major version
        /// </summary>
        Major = 0,

        /// <summary>
        /// Minor version
        /// </summary>
        Minor = 1,

        /// <summary>
        /// Path version
        /// </summary>
        Path = 2,

        /// <summary>
        /// Should not update
        /// </summary>

        NoUpdate = 3
    }
}