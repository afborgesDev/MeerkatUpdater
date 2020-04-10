using FluentAssertions;
using MeerkatUpdater.Core.Config;
using MeerkatUpdater.Core.Runner.Model.DotNet;
using System.Text.RegularExpressions;
using TechTalk.SpecFlow;

namespace MeerkatUpdater.Core.Test.FeaturesForServices.DotNetCommand
{
    [Binding]
    public class DotNetCommandSteps
    {
        private const string CommandToExecuteKey = "commandToExecute";
        private const string ExecutedCommandResultObjectKey = "executedCommandResultObject";
        private readonly Regex DotNetVersionPattern = new Regex("([0-9]{1,}([.])?)", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline);
        private readonly ScenarioContext scenarioContext;

        public DotNetCommandSteps(ScenarioContext scenarioContext) => this.scenarioContext = scenarioContext;

        [Given("The valid configurations")]
        public static void GivenTheValidConfigurations() => DefaultConfigYmlGenerator.GenerateYmlFileForDefaultConfigurations("MeerkatUpdater.yml");

        [Given("The arguments to execute was '(.*)'")]
        public void GivenTheArgumentsToExecuteWas(string command) => this.scenarioContext.Set(command, CommandToExecuteKey);

        [When("The DotNetCommand is executed")]
        public void WhenTheDotNetCommandIsExecuted()
        {
            var commandToBeExecuted = this.scenarioContext.Get<string>(CommandToExecuteKey);
            var result = Runner.Command.DotNetCommand.RunCommand(commandToBeExecuted);
            this.scenarioContext.Set(result, ExecutedCommandResultObjectKey);
        }

        [Then("The result was a succeed execution")]
        public void ThenTheResultWasASucceedExecution()
        {
            var result = GetResultObject();
            result.Should().NotBeNull();
            result.IsSuccess().Should().BeTrue();
        }

        [Then("The errorOutput doesn't have any loged error")]
        public void ThenTheErrorOutputDoesnTHaveAnyLogedError()
        {
            var result = GetResultObject();
            result.Errors.Should().BeNullOrEmpty();
        }

        [Then("The output Has the dotnet version")]
        public void ThenTheOutputHasTheDotnetVersion()
        {
            var result = GetResultObject();
            result.Output.Should().NotBeNullOrEmpty();
            var hasMatch = DotNetVersionPattern.Matches(result.Output);
            hasMatch.Count.Should().BeGreaterThan(0);
        }

        private Result GetResultObject() => this.scenarioContext.Get<Result>(ExecutedCommandResultObjectKey);
    }
}