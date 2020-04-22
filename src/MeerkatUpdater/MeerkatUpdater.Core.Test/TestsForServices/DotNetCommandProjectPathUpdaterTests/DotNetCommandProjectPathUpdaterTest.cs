using FluentAssertions;
using MeerkatUpdater.Core.Runner.Command.DotNetCommandProjectPathUpdater;
using MeerkatUpdater.Core.Test.GeneralUsage;
using Xunit;

namespace MeerkatUpdater.Core.Test.TestsForServices.DotNetCommandProjectPathUpdaterTests
{
    public class DotNetCommandProjectPathUpdaterTest
    {
        [Fact]
        public void ShouldUpdatePackagePath()
        {
            const string TestKey = nameof(ShouldUpdatePackagePath);

            try
            {
                var configManager = CreateCommandObjects.CreateConfigManager(TestKey);
                var dotNetCommand = CreateCommandObjects.CreateDotNetCommand(configManager);
                var build = CreateCommandObjects.CreateBuildCommand(configManager, dotNetCommand);
                var outDated = CreateCommandObjects.CreateOutDatedCommand(configManager, build, dotNetCommand);
                var projectPathUpdater = new ProjectPathUpdater(dotNetCommand);

                var projectInfoList = outDated.Execute();
                projectPathUpdater.Execute(ref projectInfoList);

                foreach (var project in projectInfoList)
                    project.Path.Should().NotBeNullOrEmpty();
            }
            finally
            {
                CreatedFoldersManager.TierDownTest(TestKey);
            }
        }
    }
}