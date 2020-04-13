using FluentAssertions;
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

        public DotNetCommandBuildSteps(ScenarioContext scenarioContext)
        {
            this.scenarioContext = scenarioContext;
        }

        [Given("The valid configurations with the solution path for outputPath '(.*)'")]
        public void GivenTheValidConfigurationsWithTheSolutionPathForOutputPath(string outputTestPath)
        {
            var configurations = DotNetCommandUtils.GetObjectConfigurationFromDefault();
            configurations.SolutionPath = SolutionFinder.GetFirstSolutionFile();
            configurations.OutPutPath = outputTestPath;
            configurations.NugetConfigurations.SetNewMaxTimeSecondsTimeOut(20);
            this.scenarioContext.Set(configurations, DotNetCommandUtils.ConfigurationsKey);
            Scenarios.SaveOutPutPath(this.scenarioContext, outputTestPath);
        }

        [Given("The configurations for a invalid solution path for outputPath '(.*)'")]
        public void GivenTheConfigurationsForAInvalidSolutionPathForOutputPath(string outputTestPath)
        {
            var configurations = DotNetCommandUtils.GetObjectConfigurationFromDefault();
            configurations.SolutionPath = Path.Combine(Path.GetDirectoryName(SolutionFinder.GetFirstSolutionFile()), "InvalidSln.sln");
            configurations.OutPutPath = outputTestPath;
            configurations.NugetConfigurations.SetNewMaxTimeSecondsTimeOut(20);
            this.scenarioContext.Set(configurations, DotNetCommandUtils.ConfigurationsKey);
            Scenarios.SaveOutPutPath(this.scenarioContext, outputTestPath);
        }

        [When("The Build is executed")]
        public void WhenTheBuildIsExecuted()
        {
            DotNetCommandUtils.SetConfigurationsIfWasSaved(this.scenarioContext);
            var result = Runner.Command.Build.Execute();
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

        private bool GetResultObject() => this.scenarioContext.Get<bool>(DotNetCommandUtils.ExecutedCommandResultObjectKey);
    }
}