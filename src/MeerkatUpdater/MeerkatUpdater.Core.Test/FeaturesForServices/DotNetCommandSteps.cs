using FluentAssertions;
using MeerkatUpdater.Core.Runner.Command;
using MeerkatUpdater.Core.Runner.Helpers;
using MeerkatUpdater.Core.Runner.Model.DotNet;
using System;
using System.IO;
using System.Linq;
using TechTalk.SpecFlow;

namespace MeerkatUpdater.Core.Test.FeaturesForServices
{
    [Binding]
    public class DotNetCommandSteps
    {
        private const string DotNetCommandIdentify = "DotNetCommandIdentify";
        private const string DotNetExecutionResultIdentify = "DotNetExecutionResultIdentify";
        private const string ExecutingCommandActionKey = "ExecutingCommandActionKey";
        private const string ExecutingCommandFuncKey = "ExecutingCommandFuncKey";
        private const string ExecutionParamsKey = "executionParams";
        private const string DirectoryKey = "directory";
        private const string ArgumentsKey = "arguments";
        private const string ExecutionNullValueKey = "ExecutionNullValue";
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

        [Given("The directory: '(.*)' and the arguments '(.*)'")]
        public void GivenTheDirectoryAndTheArguments(string directory, string arguments)
        {
            this.scenarioContext.Set(directory, DirectoryKey);
            this.scenarioContext.Set(arguments, ArgumentsKey);
        }

        [Given("The null value for execution")]
        public void GivenTheNullValueForExecution() => SetDefaultValueForIdentify(ExecutionNullValueKey);

        [Given("The mileSecond timeout: '(.*)'")]
        public void GivenTheMileSecondTimeout(int miliSecondsTimeOut)
        {
            var maximumWait = TimeSpan.FromMilliseconds(miliSecondsTimeOut);
            var execution = Execution.FromDirectoryWaitTimeAndArguments(".", maximumWait, "--version");
            this.scenarioContext.Set(execution, DotNetCommandIdentify);
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
            SetExecutingCommandActionIntoScenarioContext(ExecutingCommand);
        }

        [When("The compatible uses the compatible method")]
        public void WhenTheCompatibleUsesTheCompatibleMethod()
        {
            var executionParams = this.scenarioContext.Get<ExecutionParams>(ExecutionParamsKey);
            void ExecutingCommand() => Execution.FromDirectoryWaitTimeAndArguments(executionParams.WorkDirectory, executionParams.MaximumWaitTime.Value, executionParams.Arguments.ToArray());
            SetExecutingCommandActionIntoScenarioContext(ExecutingCommand);
        }

        [When("The FromDirectoryAndArguments is picked to use")]
        public void WhenTheFromDirectoryAndArgumentsIsPickedToUse()
        {
            var directory = this.scenarioContext.Get<string>(DirectoryKey);
            var arguments = this.scenarioContext.Get<string>(ArgumentsKey);

            Execution fromDirectoryExecution() => Execution.FromDirectoryAndArguments(directory, arguments);
            this.scenarioContext.Set((Func<Execution>)fromDirectoryExecution, ExecutingCommandFuncKey);
        }

        [When("The Method is select")]
        public void WhenTheMethodIsSelect()
        {
            var executionParam = SafeGetValueFromScenarioContex<Execution>(ExecutionNullValueKey);
            void ExecutingCommand() => ExecutionProcess.CreateNewProcess(executionParam);
            SetExecutingCommandActionIntoScenarioContext(ExecutingCommand);
        }

        [When("The FromStreamReader method is select")]
        public void WhenTheFromStreamReaderMethodIsSelect()
        {
            var executionParam = SafeGetValueFromScenarioContex<StreamReader>(ExecutionNullValueKey);
            void ExecutingCommand() => OutPutDotNetCommandExecution.FromStreamReader(executionParam);
            SetExecutingCommandActionIntoScenarioContext(ExecutingCommand);
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

        [Then("The result of the execution is a valid Execution object with directory: '(.*)' and arguments: '(.*)'")]
        public void ThenTheResultOfTheExecutionIsAValidExecutionObjectWithDirectoryAndArguments(string directory, string arguments)
        {
            var executionFunc = this.scenarioContext.Get<Func<Execution>>(ExecutingCommandFuncKey);
            var execution = executionFunc.Invoke();
            execution.Should().NotBeNull();
            execution.WorkDirectory.Should().Be(directory);
            execution.Arguments.Any(x => x == arguments).Should().BeTrue();
        }

        [Then("The result should not be successfull")]
        public void ThenTheResultShouldNotBeSuccessfull()
        {
            var result = this.scenarioContext.Get<Result>(DotNetExecutionResultIdentify);
            AssertDotNetExecution(result, false, false, false);
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

        private void SetDefaultValueForIdentify(string key) => this.scenarioContext.Add(key, default);

        private void SetExecutingCommandActionIntoScenarioContext(Action executingCommand) => this.scenarioContext.Set(executingCommand, ExecutingCommandActionKey);

        private T SafeGetValueFromScenarioContex<T>(string key) where T : class
        {
            this.scenarioContext.TryGetValue<T>(key, out T result);
            if (result == default)
                return null;
            return result;
        }
    }
}