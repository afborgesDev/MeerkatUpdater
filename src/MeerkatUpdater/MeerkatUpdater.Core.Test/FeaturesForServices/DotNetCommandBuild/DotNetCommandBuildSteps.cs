using FluentAssertions;
using MeerkatUpdater.Core.Config;
using MeerkatUpdater.Core.Config.Model;
using MeerkatUpdater.Core.Test.FeaturesForServices.DotNetCommand;
using MeerkatUpdater.Core.Test.GeneralUse;
using System.IO;
using TechTalk.SpecFlow;

namespace MeerkatUpdater.Core.Test.FeaturesForServices.DotNetCommandBuild
{
    [Binding]
    public class DotNetCommandBuildSteps
    {
        private const string ConfigurationsKey = "configurations";
        private const string ExecutedCommandResultObjectKey = "executedCommandResultObject";

        private readonly ScenarioContext scenarioContext;

        public DotNetCommandBuildSteps(ScenarioContext scenarioContext)
        {
            this.scenarioContext = scenarioContext;
        }

        [Then("The folder '(.*)' should be created with files")]
        public static void ThenTheFolderShouldBeCreatedWithFiles(string outputFolderName)
        {
            var directoryOutPutPath = Scenarios.FindOutPutBuildPath();
            directoryOutPutPath.Should().Contain(outputFolderName);

            var files = Directory.EnumerateFiles(directoryOutPutPath);
            files.Should().HaveCountGreaterThan(0);
        }

        [Given("The valid configurations with the solution path")]
        public void GivenTheValidConfigurationsWithTheSolutionPath()
        {
            var configurations = DotNetCommandUtils.GetObjectConfigurationFromDefault();
            configurations.SolutionPath = SolutionFinder.GetFirstSolutionFile();
            configurations.NugetConfigurations.SetNewMaxTimeSecondsTimeOut(15);
            this.scenarioContext.Set(configurations, ConfigurationsKey);
            DotNetCommandUtils.WriteNewConfigurations(configurations);
        }

        [When("The Build is executed")]
        public void WhenTheBuildIsExecuted()
        {
            SetConfigurationsIfWasSaved();
            var result = Runner.Command.Build.Execute();
            this.scenarioContext.Set(result, ExecutedCommandResultObjectKey);
        }

        [Then("The execution result should be true")]
        public void ThenTheExecutionResultShouldBeTrue()
        {
            var result = GetResultObject();
            result.Should().BeTrue();
        }

        private void SetConfigurationsIfWasSaved()
        {
            if (this.scenarioContext.TryGetValue<ExecutionConfigurations>(ConfigurationsKey, out var configurations))
                ConfigManager.ExecutionConfigurations = configurations;
        }

        private bool GetResultObject() => this.scenarioContext.Get<bool>(ExecutedCommandResultObjectKey);
    }
}