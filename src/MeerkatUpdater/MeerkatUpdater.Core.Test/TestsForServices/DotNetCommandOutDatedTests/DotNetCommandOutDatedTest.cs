using FluentAssertions;
using MeerkatUpdater.Core.Test.GeneralUsage;
using Xunit;

namespace MeerkatUpdater.Core.Test.TestsForServices.DotNetCommandOutDatedTests
{
    public class DotNetCommandOutDatedTest
    {
        [Fact]
        public void ShouldReturnInformationsAboutProjecOutDatedInsideSolution()
        {
            const int ThirtySecondsTimeOut = 30;
            const string TestKey = "ShouldReturnInformationsAboutProjecOutDatedInsideSolution";
            try
            {
                var configManager = CreateCommandObjects.CreateConfigManager(TestKey);
                var configurations = configManager.GetConfigurations();
                configurations.NugetConfigurations.SetNewMaxTimeSecondsTimeOut(ThirtySecondsTimeOut);
                var dotNetcommand = CreateCommandObjects.CreateDotNetCommand(configManager);
                var build = CreateCommandObjects.CreateBuildCommand(configManager, dotNetcommand);
                var outDated = CreateCommandObjects.CreateOutDatedCommand(configManager, build, dotNetcommand);

                var result = outDated.Execute();
                result.Should().NotBeNull();
                result.Should().HaveCountGreaterThan(0);

                foreach (var project in result)
                {
                    project.Name.Should().NotBeNullOrWhiteSpace();
                    project.Path.Should().BeNullOrWhiteSpace();
                    project.TargetFramework.Should().NotBeNullOrWhiteSpace();
                    project.InstalledPackages.Should().NotBeNull();
                    project.InstalledPackages.Should().HaveCountGreaterThan(0);
                    foreach (var package in project.InstalledPackages)
                    {
                        package.SemanticVersionChange.Should().NotBeNull();
                        package.Name.Should().NotBeNullOrWhiteSpace();
                        package.Current.Should().NotBeNull();
                        package.Current.Version.Should().NotBeNull();
                        package.Latest.Should().NotBeNull();
                        package.Latest.Version.Should().NotBeNull();
                    }
                }
            }
            finally
            {
                CreatedFoldersManager.TierDownTest(TestKey);
            }
        }
    }
}