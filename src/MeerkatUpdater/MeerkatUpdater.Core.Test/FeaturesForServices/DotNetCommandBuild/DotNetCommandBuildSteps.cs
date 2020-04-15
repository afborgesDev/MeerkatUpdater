using FluentAssertions;
using MeerkatUpdater.Core.Config.Manager;
using MeerkatUpdater.Core.Runner.Command.DotNetBuild;
using MeerkatUpdater.Core.Test.FeaturesForServices.DotNetCommand;
using MeerkatUpdater.Core.Test.GeneralUse;
using System.IO;
using TechTalk.SpecFlow;

namespace MeerkatUpdater.Core.Test.FeaturesForServices.DotNetCommandBuild
{
    [Binding]
    public class DotNetCommandBuildSteps
    {
        private readonly ScenarioContext scenarioContext;
        private readonly IBuild build;
        private readonly IConfigManager configManager;

        public DotNetCommandBuildSteps(ScenarioContext scenarioContext, IBuild build, IConfigManager configManager) =>
            (this.scenarioContext, this.build, this.configManager) = (scenarioContext, build, configManager);

        [Given("The configurations for a invalid solution path for outputPath '(.*)'")]
        public void GivenTheConfigurationsForAInvalidSolutionPathForOutputPath(string outputTestPath)
        {
            var configurations = DotNetCommandUtils.GetObjectConfigurationFromDefault();
            configurations.SolutionPath = Path.Combine(Path.GetDirectoryName(SolutionFinder.GetFirstSolutionFile()), "Invali%$@#$09()dSln.sln");
            configurations.OutPutPath = outputTestPath;
            configurations.NugetConfigurations.SetNewMaxTimeSecondsTimeOut(20);
            this.scenarioContext.Set(configurations, DotNetCommandUtils.ConfigurationsKey);
            Scenarios.SaveOutPutPath(this.scenarioContext, outputTestPath);
        }

        [When("The Build is executed")]
        public void WhenTheBuildIsExecuted()
        {
            DotNetCommandUtils.SetConfigurationsIfWasSaved(this.scenarioContext, this.configManager);
            var result = this.build.Execute();
            this.scenarioContext.Set(result, DotNetCommandUtils.ExecutedCommandResultObjectKey);
        }

        [Then("The execution result should be '(.*)'")]
        public void ThenTheExecutionResultShouldBe(bool expectedResult)
        {
            var result = GetResultObject();
            result.Should().Be(expectedResult);
        }

        [Then("The folder '(.*)' should be created with files")]
        public void ThenTheFolderShouldBeCreatedWithFiles(string outputFolderName)
        {
            var directoryOutPutPath = Scenarios.FindOutPutBuildPathFromScenario(this.scenarioContext);
            directoryOutPutPath.Should().Contain(outputFolderName);

            var files = Directory.EnumerateFiles(directoryOutPutPath);
            files.Should().HaveCountGreaterThan(0);
        }

        [Then("The output folder should not be created with files")]
        public void ThenTheOutputFolderShouldNotBeCreatedWithFiles()
        {
            var directoryOutPutPath = Scenarios.FindOutPutBuildPathFromScenario(this.scenarioContext);
            directoryOutPutPath.Should().BeNullOrWhiteSpace();
        }

        private bool GetResultObject() => this.scenarioContext.Get<bool>(DotNetCommandUtils.ExecutedCommandResultObjectKey);
    }
}