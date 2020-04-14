using MeerkatUpdater.Core.Config.Manager;
using MeerkatUpdater.Core.Runner.Command.DotNet;
using MeerkatUpdater.Core.Runner.Command.DotNetBuild;
using MeerkatUpdater.Core.Runner.Command.DotNetClean;
using MeerkatUpdater.Core.Runner.Command.DotNetContProject;
using MeerkatUpdater.Core.Runner.Command.DotNetOutDated;
using MeerkatUpdater.Core.Runner.Command.DotNetUpdateProcess;
using Microsoft.Extensions.DependencyInjection;

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
            serviceContainer.AddScoped<IConfigManager>();
            RegisterCommands(serviceContainer);
            return serviceContainer;
        }

        private static void RegisterCommands(IServiceCollection serviceContainer)
        {
            serviceContainer.AddScoped<IDotNetCommand>();
            serviceContainer.AddScoped<IBuild>();
            serviceContainer.AddScoped<IClean>();
            serviceContainer.AddScoped<ICountProject>();
            serviceContainer.AddScoped<IOutDated>();
            serviceContainer.AddScoped<IUpdateProcess>();
        }
    }
}