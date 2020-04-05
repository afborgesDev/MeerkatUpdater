using FluentAssertions;
using MeerkatUpdater.Core.Runner.Command;
using MeerkatUpdater.Core.Test.GeneralUse;
using TechTalk.SpecFlow;

namespace MeerkatUpdater.Core.Test.FeaturesForServices
{
    [Binding]
    public class BuildCommandSteps
    {
        private const string SolutionFileKey = "solutionFile";
        private const string ResultKey = "result";
        private readonly ScenarioContext scenarioContext;

        public BuildCommandSteps(ScenarioContext scenarioContext) => this.scenarioContext = scenarioContext;

        [Given("The solution file to use")]
        public void GivenTheSolutionFileToUse()
        {
            var solutionFile = SolutionFinder.GetFirstSolutionFile();
            this.scenarioContext.Set(solutionFile, SolutionFileKey);
        }

        [When("The comamnd is executed")]
        public void WhenTheComamndIsExecuted()
        {
            var solutionFile = this.scenarioContext.Get<string>(SolutionFileKey);
            var result = Build.Execute(solutionFile);
            this.scenarioContext.Set(result, ResultKey);
        }

        [Then("The function returns a true as a succeed result for the build")]
        public void ThenTheFunctionReturnsATrueAsASucceedResultForTheBuild()
        {
            var result = this.scenarioContext.Get<bool>(ResultKey);
            result.Should().BeTrue();
        }
    }
}