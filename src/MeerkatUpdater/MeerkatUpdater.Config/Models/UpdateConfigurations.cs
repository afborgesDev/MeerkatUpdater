using System.Collections.Generic;

namespace MeerkatUpdater.Config.Models
{
    /// <summary>
    /// Aggregated configurations for the update process
    /// </summary>
    public class UpdateConfigurations
    {
        /// <summary>
        /// Sets if the process will continue if the updates fail
        /// </summary>
        public bool RolbackIfFail { get; set; }

        /// <summary>
        /// Sets the types of semantic versions that will be updated
        /// </summary>
        public IEnumerable<SemanticVersion> AllowedVersionsToUpdate { get; set; }
    }
}
