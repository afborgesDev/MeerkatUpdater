using FluentAssertions;
using MeerkatUpdater.Core.Config;
using MeerkatUpdater.Core.Config.Model;
using MeerkatUpdater.Core.Runner.Model.DotNet;
using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using TechTalk.SpecFlow;

namespace MeerkatUpdater.Core.Test.FeaturesForServices.DotNetCommand
{
    [Binding]
    public class DotNetCommandSteps
    {
        private const string CommandToExecuteKey = "commandToExecute";
        private const string ExecutedCommandResultObjectKey = "executedCommandResultObject";
        private const string SpendedTimeToExecuteCommandKey = "SpendedTimeToExecuteCommand";
        private readonly Regex DotNetVersionPattern = new Regex("([0-9]{1,}([.])?)", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline);
        private readonly ScenarioContext scenarioContext;

        public DotNetCommandSteps(ScenarioContext scenarioContext) => this.scenarioContext = scenarioContext;

        [Given("The valid configurations")]
        public static void GivenTheValidConfigurations() => DefaultConfigYmlGenerator.GenerateYmlFileForDefaultConfigurations("MeerkatUpdater.yml");

        [Given("The configuration with no valid WaitTimeOut")]
        public void GivenTheConfigurationWithNoValidWaitTimeOut()
        {
            var payload = DefaultConfigYmlGenerator.GenerateDefaultConfigurations();
            var configurations = DefaultYmlDeserializer.Deserialize<ExecutionConfigurations>(payload);
            configurations.NugetConfigurations = new NugetConfigurations();
            payload = DefaultConfigYmlGenerator.BuildYmlFile(configurations);
            File.WriteAllText(DefaultConfigYmlGenerator.DefaultConfigurationFileName, payload);
        }

        [Given("The arguments to execute was '(.*)'")]
        public void GivenTheArgumentsToExecuteWas(string command) => this.scenarioContext.Set(command, CommandToExecuteKey);

        [When("The DotNetCommand is executed")]
        public void WhenTheDotNetCommandIsExecuted()
        {
            var commandToBeExecuted = this.scenarioContext.Get<string>(CommandToExecuteKey);

            var watcher = new Stopwatch();
            watcher.Start();
            var result = Runner.Command.DotNetCommand.RunCommand(commandToBeExecuted);
            watcher.Stop();

            this.scenarioContext.Set(result, ExecutedCommandResultObjectKey);
            this.scenarioContext.Set(watcher.ElapsedMilliseconds, SpendedTimeToExecuteCommandKey);
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

        [Then("The time spend was equal or lest than the default")]
        public void ThenTheTimeSpendWasEqualOrLestThanTheDefault()
        {
            var spendedTime = this.scenarioContext.Get<long>(SpendedTimeToExecuteCommandKey);
            var defaultLongMaximumWait = Convert.ToInt64(ConfigManager.DefaultMaximumWait.TotalSeconds);
            var timeSpanSpended = TimeSpan.FromMilliseconds(spendedTime);
            var secondsSpended = timeSpanSpended.TotalSeconds;

            secondsSpended.Should().BeLessOrEqualTo(defaultLongMaximumWait);
        }

        private Result GetResultObject() => this.scenarioContext.Get<Result>(ExecutedCommandResultObjectKey);
    }
}