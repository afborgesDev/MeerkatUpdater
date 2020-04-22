using FluentAssertions;
using MeerkatUpdater.Core.Runner.Command.DotNetCommandProjectPathUpdater;
using MeerkatUpdater.Core.Runner.Command.DotNetCommandUpdate;
using MeerkatUpdater.Core.Test.GeneralUsage;
using System.Linq;
using Xunit;

namespace MeerkatUpdater.Core.Test.TestsForServices.DotNetCommandUpdateTests
{
    public class DotNetCommandUpdateTest
    {
        [Fact]
        public void ShouldUpdateOutDatedPackages()
        {
            const string TestKey = nameof(ShouldUpdateOutDatedPackages);

            try
            {
                var configManager = CreateCommandObjects.CreateConfigManager(TestKey);
                var dotNetCommand = CreateCommandObjects.CreateDotNetCommand(configManager);
                var build = CreateCommandObjects.CreateBuildCommand(configManager, dotNetCommand);
                var outDated = CreateCommandObjects.CreateOutDatedCommand(configManager, build, dotNetCommand);
                var projectPathUpdater = new ProjectPathUpdater(dotNetCommand);
                var update = new Update(configManager, dotNetCommand);

                var projectInfoList = outDated.Execute();
                projectPathUpdater.Execute(ref projectInfoList);

                update.Execute(ref projectInfoList);
                var allowedVersionToUpdate = configManager.GetConfigurations().UpdateConfigurations?.AllowedVersionsToUpdate;

                foreach (var project in projectInfoList)
                {
                    foreach (var package in project.InstalledPackages)
                    {
                        if (allowedVersionToUpdate.Any(x => x == package.SemanticVersionChange))
                        {
                            package.UpdatedAt.Should().NotBeNull();
                            package.OldVersion.Should().NotBeEquivalentTo(package.Current);
                        }
                        else
                        {
                            package.UpdatedAt.Should().BeNull();
                            package.OldVersion.Should().BeNull();
                        }
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