using MeerkatUpdater.Core.Config.Manager;
using MeerkatUpdater.Core.Runner.Command.DotNet;
using MeerkatUpdater.Core.Runner.Command.DotNetBuild;
using MeerkatUpdater.Core.Runner.Command.DotNetClean;
using MeerkatUpdater.Core.Runner.Command.DotNetCommandProjectPathUpdater;
using MeerkatUpdater.Core.Runner.Command.DotNetCommandUpdate;
using MeerkatUpdater.Core.Runner.Command.DotNetContProject;
using MeerkatUpdater.Core.Runner.Command.DotNetOutDated;
using MeerkatUpdater.Core.Runner.Command.DotNetUpdateProcess;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace MeerkatUpdater.Core.DependencyInjection
{
    /// <summary>
    /// Configure and inject all necessary items to Meerkat works
    /// </summary>
    public static class ContainerManager
    {
        /// <summary>
        /// Responsable to inject <br/>
        /// - <see cref="IConfigManager"/> <br/>
        /// - <see cref="IDotNetCommand"/> <br/>
        /// -
        /// </summary>
        /// <param name="serviceContainer"></param>
        /// <returns></returns>
        public static IServiceCollection AddMeerkatUpdater(this IServiceCollection serviceContainer)
        {
            serviceContainer.AddScoped<IConfigManager, ConfigManager>();
            RegisterCommands(serviceContainer);
            ConfigureLog(serviceContainer);
            return serviceContainer;
        }

        private static void RegisterCommands(IServiceCollection serviceContainer)
        {
            serviceContainer.AddScoped<IDotNetCommand, DotNetCommand>();
            serviceContainer.AddScoped<IBuild, Build>();
            serviceContainer.AddScoped<IClean, Clean>();
            serviceContainer.AddScoped<ICountProject, CountProject>();
            serviceContainer.AddScoped<IOutDated, OutDated>();
            serviceContainer.AddScoped<IProjectPathUpdater, ProjectPathUpdater>();
            serviceContainer.AddScoped<IUpdate, Update>();
            serviceContainer.AddScoped<IUpdateProcessPreparation, UpdateProcessPreparation>();
            serviceContainer.AddScoped<IUpdateProcess, UpdateProcess>();
        }

        [ExcludeFromCodeCoverage]
        private static void ConfigureLog(IServiceCollection serviceContainer) =>
           serviceContainer.AddLogging(configure => configure.AddConsole())
                           .Configure<LoggerFilterOptions>(options => options.MinLevel = LogLevel.Information);
    }
}