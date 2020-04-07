namespace MeerkatUpdater.Core.Runner.Model.PackageInfo
{
    /// <summary>
    /// Information about the instaled package
    /// </summary>
    public sealed class VersionInfo
    {
        /// <summary>
        /// The number of the version
        /// </summary>
        public string? Version { get; set; }

        /// <summary>
        /// Creates a new versionInfo by using the text version
        /// </summary>
        /// <param name="versionText"></param>
        /// <returns></returns>
        public static VersionInfo FromVersionText(string? versionText) => new VersionInfo
        {
            Version = versionText
        };

        /// <summary>
        /// Indicates if the version is istable or not
        /// </summary>
        public bool IsEstable() => true;
    }
}