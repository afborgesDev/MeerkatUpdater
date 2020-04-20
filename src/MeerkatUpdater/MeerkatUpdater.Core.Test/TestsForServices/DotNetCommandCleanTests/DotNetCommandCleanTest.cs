using FluentAssertions;
using MeerkatUpdater.Core.Test.GeneralUsage;
using System.IO;
using Xunit;

namespace MeerkatUpdater.Core.Test.TestsForServices.DotNetCommandCleanTests
{
    public class DotNetCommandCleanTest
    {
        [Fact]
        public void ShouldRemoveFilesFromOutPutFolderTheCleanExecution()
        {
            const string ProjectName = "MeerkatUpdater.Core.dll";
            const string TestKey = "ShouldRemoveFilesFromOutPutFolderTheCleanExecution";
            string outputDirectory = string.Empty;
            try
            {
                var configManager = CreateCommandObjects.CreateConfigManager(TestKey);
                var configurations = configManager.GetConfigurations();
                configurations.OutPutPath = Path.Combine(configurations.OutPutPath, "CleanOutPut");
                Directory.CreateDirectory(configurations.OutPutPath);
                outputDirectory = configurations.OutPutPath;
                configManager.SetConfigurations(configurations);
                var dotNetcommand = CreateCommandObjects.CreateDotNetCommand(configManager);
                var build = CreateCommandObjects.CreateBuildCommand(configManager, dotNetcommand);
                var clean = CreateCommandObjects.CreateCleanCommand(configManager, dotNetcommand);

                build.Execute();
                clean.Execute();

                var buildedProject = Path.Combine(configurations.OutPutPath, ProjectName);
                var fileExists = File.Exists(buildedProject);
                fileExists.Should().BeFalse();
            }
            finally
            {
                Directory.Delete(outputDirectory, true);
                CreatedFoldersManager.TierDownTest(TestKey);
            }
        }
    }
}