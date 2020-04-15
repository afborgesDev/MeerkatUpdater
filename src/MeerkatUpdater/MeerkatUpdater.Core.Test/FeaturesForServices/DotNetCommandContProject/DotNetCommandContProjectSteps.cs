using FluentAssertions;
using MeerkatUpdater.Core.Config.Manager;
using MeerkatUpdater.Core.Runner.Command.DotNetContProject;
using MeerkatUpdater.Core.Test.FeaturesForServices.DotNetCommand;
using TechTalk.SpecFlow;

namespace MeerkatUpdater.Core.Test.FeaturesForServices.DotNetCommandContProject
{
    [Binding]
    public class DotNetCommandContProjectSteps
    {
        private const string CountProjectResultKey = "countProjectResult";
        private readonly ScenarioContext scenarioContext;
        private readonly ICountProject countProject;
        private readonly IConfigManager configManager;

        public DotNetCommandContProjectSteps(ScenarioContext scenarioContext, ICountProject countProject, IConfigManager configManager) =>
            (this.scenarioContext, this.countProject, this.configManager) = (scenarioContext, countProject, configManager);

        [When("The CountProject is executed")]
        public void WhenTheCountProjectIsExecuted()
        {
            DotNetCommandUtils.SetConfigurationsIfWasSaved(this.scenarioContext, this.configManager);
            var result = this.countProject.Execute();
            this.scenarioContext.Set(result, CountProjectResultKey);
        }

        [Then("The count shoudl be more than '(.*)'")]
        public void ThenTheCountShoudlBeMoreThan(int minimumNumberOfProjects)
        {
            var result = this.scenarioContext.Get<int?>(CountProjectResultKey);
            result.Should().NotBeNull();
            result.Should().BeGreaterThan(minimumNumberOfProjects);
        }
    }
}