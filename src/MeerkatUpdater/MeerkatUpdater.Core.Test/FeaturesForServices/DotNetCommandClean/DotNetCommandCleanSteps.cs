using FluentAssertions;
using MeerkatUpdater.Core.Config;
using MeerkatUpdater.Core.Test.FeaturesForServices.DotNetCommand;
using MeerkatUpdater.Core.Test.GeneralUse;
using System.IO;
using TechTalk.SpecFlow;

namespace MeerkatUpdater.Core.Test.FeaturesForServices.DotNetCommandClean
{
    [Binding]
    public class DotNetCommandCleanSteps
    {
        private readonly ScenarioContext scenarioContext;

        public DotNetCommandCleanSteps(ScenarioContext scenarioContext) => this.scenarioContext = scenarioContext;

        [When("The Clean is executed")]
        public void WhenTheCleanIsExecuted()
        {
            DotNetCommandUtils.SetConfigurationsIfWasSaved(this.scenarioContext);
            Runner.Command.Build.Execute();
            Runner.Command.Clean.Execute();
        }

        [Then("The target solution dll file was cleaned up")]
        public void ThenTheTargetSolutionDllFileWasCleanedUp()
        {
            const string ProjectName = "MeerkatUpdater.Core.dll";

            var buildedProject = Path.Combine(Path.GetDirectoryName(SolutionFinder.GetFirstSolutionFile()),
                                              ConfigManager.GetExecutionConfigurations().OutPutPath,
                                              ProjectName);
            var fileExists = File.Exists(buildedProject);
            fileExists.Should().BeFalse();
        }
    }
}