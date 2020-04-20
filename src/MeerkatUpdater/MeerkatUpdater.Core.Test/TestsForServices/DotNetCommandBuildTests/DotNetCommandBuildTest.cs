using FluentAssertions;
using MeerkatUpdater.Core.Test.GeneralUsage;
using System.IO;
using Xunit;

namespace MeerkatUpdater.Core.Test.TestsForServices.DotNetCommandBuildTests
{
    public class DotNetCommandBuildTest
    {
        [Fact]
        public void ShouldGenerateFilesASucceedBuild()
        {
            const string TestKey = "shoudGenerateFilesSucceedBuild";
            try
            {
                var configManager = CreateCommandObjects.CreateConfigManager(TestKey);
                var dotnetCommand = CreateCommandObjects.CreateDotNetCommand(configManager);
                var build = CreateCommandObjects.CreateBuildCommand(configManager, dotnetCommand);

                var result = build.Execute();
                result.Should().BeTrue();

                var files = Directory.EnumerateFiles(configManager.GetConfigurations().OutPutPath);
                files.Should().HaveCountGreaterThan(0);
            }
            finally
            {
                CreatedFoldersManager.TierDownTest(TestKey);
            }
        }
    }
}