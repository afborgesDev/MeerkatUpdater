using FluentAssertions;
using MeerkatUpdater.Core.Model.DotNetCommand;
using MeerkatUpdater.Core.Runner;
using System;
using TechTalk.SpecFlow;

namespace MeerkatUpdater.Core.Test.FeaturesForServices
{
    [Binding]
    public class DotNetCommandSteps
    {
        private const string DotNetCommandIdentify = "DotNetCommandIdentify";
        private const string DotNetExecutionResultIdentify = "DotNetExecutionResultIdentify";
        private const string ExecutingCommandActionKey = "ExecutingCommandActionKey";
        private const string ExecutionParamsKey = "executionParams";
        private readonly ScenarioContext scenarioContext;

        public DotNetCommandSteps(ScenarioContext scenarioContext) => this.scenarioContext = scenarioContext;

        [Given("The command '(.*)'")]
        public void GivenTheCommand(string command)
        {
            var execution = Execution.FromCurrentDirrectoryAndArgument(command);
            this.scenarioContext.Set(execution, DotNetCommandIdentify);
        }

        [Given("The null reference for Execution parameter")]
        public void GivenTheNullReferenceForExecutionParameter() => this.scenarioContext.Add(DotNetCommandIdentify, null);

        [Given("The Execution with a blank workdir parameter")]
        public void GivenTheExecutionWithABlankWorkdirParameter()
        {
            var execution = new Execution { WorkDirectory = string.Empty, Arguments = { "someWrong" }, MaximumWait = TimeSpan.FromSeconds(0) };
            this.scenarioContext.Set(execution, DotNetCommandIdentify);
        }

        [Given("The Execution params")]
        public void GivenTheExecutionParams(Table table)
        {
            var executionParams = DotNetCommandTableArgumentsTransformer.TransformTableParamsToTargetExecutionMethod(table);
            this.scenarioContext.Set(executionParams, ExecutionParamsKey);
        }

        [When("the DotNetCommandService is triggered")]
        public void WhenTheDotNetCommandServiceIsTriggered()
        {
            var execution = this.scenarioContext.Get<Execution>(DotNetCommandIdentify);
            var result = DotNetCommand.RunCommand(execution);
            this.scenarioContext.Set(result, DotNetExecutionResultIdentify);
        }

        [When("The DotNetCommandService uses this reference")]
        public void WhenTheDotNetCommandServiceUsesThisReference()
        {
            this.scenarioContext.TryGetValue(DotNetCommandIdentify, out Execution execution);
            void ExecutingCommand() => DotNetCommand.RunCommand(execution);
            this.scenarioContext.Set((Action)ExecutingCommand, ExecutingCommandActionKey);
        }

        [When("The compatible uses the compatible method")]
        public void WhenTheCompatibleUsesTheCompatibleMethod()
        {
            var executionParams = this.scenarioContext.Get<ExecutionParams>(ExecutionParamsKey);
            void ExecutingCommand() => Execution.FromDirectoryWaitTimeAndArguments(executionParams.WorkDirectory, executionParams.MaximumWaitTime.Value, executionParams.Arguments.ToArray());
            this.scenarioContext.Set((Action)ExecutingCommand, ExecutingCommandActionKey);
        }

        [Then("the results have the success execution and the errors has no items")]
        public void ThenTheResultsHaveTheSuccessExecutionAndTheErrorsHasNoItems()
        {
            var result = this.scenarioContext.Get<Result>(DotNetExecutionResultIdentify);
            AssertDotNetExecution(result, true, true, false);
        }

        [Then("the results have no success and has errors")]
        public void ThenTheResultsHaveNoSuccessAndHasErrors()
        {
            var result = this.scenarioContext.Get<Result>(DotNetExecutionResultIdentify);
            AssertDotNetExecution(result, true, false, true);
        }

        [Then("The execution trigger an exception")]
        public void ThenTheExecutionTriggerAnException()
        {
            var executingCommand = this.scenarioContext.Get<Action>(ExecutingCommandActionKey);
            executingCommand.Should().Throw<Exception>();
        }

        private static void AssertDotNetExecution(Result result, bool hasSuccess, bool hasOutPut, bool hasErrors)
        {
            result.Should().NotBeNull();
            result.IsSuccess().Should().Be(hasSuccess);
            if (hasOutPut)
                result.Output.Should().NotBeNullOrWhiteSpace();
            else
                result.Output.Should().BeNullOrWhiteSpace();

            if (hasErrors)
                result.Errors.Should().NotBeNullOrWhiteSpace();
            else
                result.Errors.Should().BeNullOrWhiteSpace();
        }
    }
}