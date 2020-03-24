﻿using System.Collections.Generic;

namespace MeerkatUpdater.Config
{
    /// <summary>
    /// Aggragated configurations to nuget
    /// </summary>
    public class NugetConfigurations
    {
        /// <summary>
        /// The maximun timeout for try to get a package from nuget repository
        /// </summary>
        public int MaxTimeSecondsTimeOut { get; set; }

        /// <summary>
        /// The adicional urls as sources from nuget
        /// </summary>
        public IEnumerable<string> Sources { get; set; }
    }
}
