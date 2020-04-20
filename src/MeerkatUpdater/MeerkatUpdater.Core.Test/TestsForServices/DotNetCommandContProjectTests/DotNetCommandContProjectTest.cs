using FluentAssertions;
using MeerkatUpdater.Core.Test.GeneralUsage;
using Xunit;

namespace MeerkatUpdater.Core.Test.TestsForServices.DotNetCommandContProjectTests
{
    public class DotNetCommandContProjectTest
    {
        [Fact]
        public void ShouldReturnNumberOfCsProjInsideSolution()
        {
            const int ExpectedNumberOfProjects = 2;
            const string TestKey = "ShouldReturnNumberOfCsProjInsideSolution";
            try
            {
                var configManager = CreateCommandObjects.CreateConfigManager(TestKey);
                var dotNetCommand = CreateCommandObjects.CreateDotNetCommand(configManager);
                var countProject = CreateCommandObjects.CreateCountProject(dotNetCommand);

                var count = countProject.Execute();
                count.Should().Be(ExpectedNumberOfProjects);
            }
            finally
            {
                CreatedFoldersManager.TierDownTest(TestKey);
            }
        }
    }
}