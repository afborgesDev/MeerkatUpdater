using MeerkatUpdater.Core.Test.GeneralUse;
using TechTalk.SpecFlow;

namespace MeerkatUpdater.Core.Test.FeaturesForServices
{
    [Binding]
    public class BuildCommandSteps
    {
        private const string SolutionFileKey = "solutionFile";
        private readonly ScenarioContext scenarioContext;

        public BuildCommandSteps(ScenarioContext scenarioContext) => this.scenarioContext = scenarioContext;

        [Given("The solution file to use")]
        public void GivenTheSolutionFileToUse()
        {
            var solutionFile = SolutionFinder.GetFirstSolutionFile();
            //var execution = Execution.from
            this.scenarioContext.Set(solutionFile, SolutionFileKey);
        }

        [When("The comamnd is executed")]
        public void WhenTheComamndIsExecuted()
        {
            //var result =
        }
    }
}