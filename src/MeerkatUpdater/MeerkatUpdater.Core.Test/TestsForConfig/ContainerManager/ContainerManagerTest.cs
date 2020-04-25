using FluentAssertions;
using MeerkatUpdater.Core.Config.Manager;
using MeerkatUpdater.Core.DependencyInjection;
using MeerkatUpdater.Core.Runner.Command.DotNet;
using MeerkatUpdater.Core.Runner.Command.DotNetBuild;
using MeerkatUpdater.Core.Runner.Command.DotNetClean;
using MeerkatUpdater.Core.Runner.Command.DotNetCommandProjectPathUpdater;
using MeerkatUpdater.Core.Runner.Command.DotNetCommandUpdate;
using MeerkatUpdater.Core.Runner.Command.DotNetContProject;
using MeerkatUpdater.Core.Runner.Command.DotNetOutDated;
using MeerkatUpdater.Core.Runner.Command.DotNetUpdateProcess;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace MeerkatUpdater.Core.Test.TestsForConfig.ContainerManager
{
    public class ContainerManagerTest
    {
        [Fact]
        public void ShoudInjectNecessaryServices()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddMeerkatUpdater();

            var provider = serviceCollection.BuildServiceProvider();
            provider.GetService<IConfigManager>().Should().NotBeNull();
            provider.GetService<IDotNetCommand>().Should().NotBeNull();
            provider.GetService<IBuild>().Should().NotBeNull();
            provider.GetService<IClean>().Should().NotBeNull();
            provider.GetService<ICountProject>().Should().NotBeNull();
            provider.GetService<IOutDated>().Should().NotBeNull();
            provider.GetService<IProjectPathUpdater>().Should().NotBeNull();
            provider.GetService<IUpdate>().Should().NotBeNull();
            provider.GetService<IUpdateProcess>().Should().NotBeNull();
        }
    }
}