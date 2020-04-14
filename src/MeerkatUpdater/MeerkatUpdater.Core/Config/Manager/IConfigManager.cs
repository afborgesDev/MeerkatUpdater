using MeerkatUpdater.Core.Config.Model;
using System;

namespace MeerkatUpdater.Core.Config.Manager
{
    /// <summary>
    /// Manage the configurations
    /// </summary>
    public interface IConfigManager
    {
        /// <summary>
        /// If the ExecutionConfiguration is not loaded try to load from file.
        /// </summary>
        /// <returns></returns>
        ExecutionConfigurations GetConfigurations();

        /// <summary>
        /// Set a new Instance of configurations
        /// </summary>
        /// <param name="executionConfigurations"></param>
        void SetConfigurations(ExecutionConfigurations executionConfigurations);

        /// <summary>
        /// Return the default maximum wait
        /// </summary>
        /// <returns></returns>
        TimeSpan GetDefaultMaximumWait();

        /// <summary>
        /// Get the configured wait time in mili seconds
        /// </summary>
        /// <returns></returns>
        int GetWaitMiliSeconds();
    }
}