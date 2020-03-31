using FluentAssertions;
using MeerkatUpdater.Core.Model.DotNetCommand;
using MeerkatUpdater.Core.Runner;
using TechTalk.SpecFlow;

namespace MeerkatUpdater.Core.Test.FeaturesForServices
{
    [Binding]
    public class DotNetCommandSteps
    {
        private const string DotNetCommandIdentify = "DotNetCommandIdentify";
        private const string DotNetExecutionResultIdentify = "DotNetExecutionResultIdentify";
        private readonly ScenarioContext scenarioContext;

        public DotNetCommandSteps(ScenarioContext scenarioContext) => this.scenarioContext = scenarioContext;

        [Given("The command for get the version of the dotnet '(.*)'")]
        public void GivenTheCommandForGetTheVersionOfTheDotnet(string command)
        {
            var execution = Execution.FromCurrentDirrectoryAndArgument(command);
            this.scenarioContext.Set(execution, DotNetCommandIdentify);
        }

        [When("the DotNetCommandService is triggered")]
        public void WhenTheDotNetCommandServiceIsTriggered()
        {
            var execution = this.scenarioContext.Get<Execution>(DotNetCommandIdentify);
            var result = DotNetCommand.RunCommand(execution);
            this.scenarioContext.Set(result, DotNetExecutionResultIdentify);
        }

        [Then("the results have the exitCode '(.*)' and the errors has no items")]
        public void ThenTheResultsHaveTheExitCodeAndTheErrorsHasNoItems(int exitCode)
        {
            var result = this.scenarioContext.Get<Result>(DotNetExecutionResultIdentify);
            result.Should().NotBeNull();
            result.ExitCode.Should().Be(exitCode);
            result.Output.Length.Should().BeGreaterThan(0);
            result.Errors.Length.Should().Be(0);
        }
    }
}