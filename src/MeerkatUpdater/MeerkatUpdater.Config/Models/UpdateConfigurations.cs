using System.Collections.Generic;

namespace MeerkatUpdater.Config.Models
{
    /// <summary>
    /// Aggregated configurations for the update process
    /// </summary>
    public sealed class UpdateConfigurations
    {
        /// <summary>
        /// Sets if the process will continue if the updates fail
        /// </summary>
        public bool RolbackIfFail { get; set; }

        /// <summary>
        /// Sets the types of semantic versions that will be updated
        /// </summary>
        public HashSet<SemanticVersion> AllowedVersionsToUpdate { get; } = new HashSet<SemanticVersion>();
    }
}
