namespace MeerkatUpdater.Core.Config.Model
{
    /// <summary>
    /// Suported semantic versions to update
    /// </summary>
    public enum SemanticVersion
    {
        /// <summary>
        /// Represents a Major version of a package
        /// </summary>
        Major = 0,

        /// <summary>
        /// Represents a Minor version of a pacakge
        /// </summary>
        Minor = 1,

        /// <summary>
        /// Represents a Path version of a pacakge
        /// </summary>
        Path = 2,

        /// <summary>
        /// Represents that the update should not update (It is used to jus show that have packages to update)
        /// </summary>
        NoUpdate = 3
    }
}