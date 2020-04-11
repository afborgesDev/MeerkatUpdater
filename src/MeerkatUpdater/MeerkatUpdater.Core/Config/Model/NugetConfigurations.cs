using System;
using System.Collections.Generic;

namespace MeerkatUpdater.Core.Config.Model
{
    /// <summary>
    /// Aggragated configurations to nuget
    /// </summary>
    public sealed class NugetConfigurations
    {
        /// <summary>
        /// The maximun timeout for try to get a package from nuget repository
        /// </summary>
        public int MaxTimeSecondsTimeOut { get; set; }

        /// <summary>
        /// The adicional urls as sources from nuget
        /// </summary>
        public IList<string> Sources { get; } = new List<string>();

        /// <summary>
        /// Calculate the seconds for Maximum wait time based on the int property
        /// </summary>
        /// <returns></returns>
        public TimeSpan GetMaximumWaitTime() => TimeSpan.FromSeconds(MaxTimeSecondsTimeOut);

        /// <summary>
        /// Return in miliseconds the maximum wait time
        /// </summary>
        /// <returns></returns>
        public int GetMaximumWaitTimeMiliseconds() => Convert.ToInt32(GetMaximumWaitTime().TotalMilliseconds);

        /// <summary>
        /// Validate and set the new max timeseconds
        /// </summary>
        /// <param name="newMaxTime"></param>
        public void SetNewMaxTimeSecondsTimeOut(int? newMaxTime)
        {
            if (newMaxTime is null || newMaxTime <= 0)
                return;

            MaxTimeSecondsTimeOut = newMaxTime.Value;
        }
    }
}