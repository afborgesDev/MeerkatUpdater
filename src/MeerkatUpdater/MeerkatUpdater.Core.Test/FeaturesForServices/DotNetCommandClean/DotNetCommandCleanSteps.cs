using FluentAssertions;
using MeerkatUpdater.Core.Config.Manager;
using MeerkatUpdater.Core.Runner.Command.DotNetBuild;
using MeerkatUpdater.Core.Runner.Command.DotNetClean;
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
        private readonly IBuild build;
        private readonly IClean clean;
        private readonly IConfigManager configManager;

        public DotNetCommandCleanSteps(ScenarioContext scenarioContext, IBuild build, IClean clean, IConfigManager configManager) =>
            (this.scenarioContext, this.build, this.clean, this.configManager) = (scenarioContext, build, clean, configManager);

        [When("The Clean is executed")]
        public void WhenTheCleanIsExecuted()
        {
            DotNetCommandUtils.SetConfigurationsIfWasSaved(this.scenarioContext, this.configManager);
            this.build.Execute();
            this.clean.Execute();
        }

        [Then("The target solution dll file was cleaned up")]
        public void ThenTheTargetSolutionDllFileWasCleanedUp()
        {
            const string ProjectName = "MeerkatUpdater.Core.dll";

            var buildedProject = Path.Combine(Path.GetDirectoryName(SolutionFinder.GetFirstSolutionFile()),
                                              this.configManager.GetConfigurations().OutPutPath,
                                              ProjectName);

            var fileExists = File.Exists(buildedProject);
            fileExists.Should().BeFalse();
        }
    }
}