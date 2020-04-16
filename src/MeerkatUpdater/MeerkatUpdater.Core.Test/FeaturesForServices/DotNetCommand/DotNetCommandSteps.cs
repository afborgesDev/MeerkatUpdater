using FluentAssertions;
using MeerkatUpdater.Core.Config.DefaultServices;
using MeerkatUpdater.Core.Config.Manager;
using MeerkatUpdater.Core.Config.Model;
using MeerkatUpdater.Core.Runner.Command.DotNet;
using MeerkatUpdater.Core.Runner.Model.DotNet;
using MeerkatUpdater.Core.Test.GeneralUse;
using System;
using System.Diagnostics;
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
        private const string ConfigurationsKey = "configurations";

        private readonly Regex DotNetVersionPattern = new Regex("([0-9]{1,}([.])?)", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline);
        private readonly ScenarioContext scenarioContext;
        private readonly IDotNetCommand dotNetCommand;
        private readonly IConfigManager configManager;

        public DotNetCommandSteps(ScenarioContext scenarioContext, IDotNetCommand dotNetCommand, IConfigManager configManager) =>
            (this.scenarioContext, this.dotNetCommand, this.configManager) = (scenarioContext, dotNetCommand, configManager);

        [Given("The valid configurations")]
        public static void GivenTheValidConfigurations() => DefaultConfigYmlGenerator.GenerateYmlFileForDefaultConfigurations("MeerkatUpdater.yml");

        [Given("The configuration with no valid WaitTimeOut")]
        public void GivenTheConfigurationWithNoValidWaitTimeOut()
        {
            var configurations = DotNetCommandUtils.GetObjectConfigurationFromDefault();
            configurations.NugetConfigurations = new NugetConfigurations();
            this.scenarioContext.Set(configurations, ConfigurationsKey);
            DotNetCommandUtils.WriteNewConfigurations(configurations);
        }

        [Given("The configuration with the short WaitTimeOut")]
        public void GivenTheConfigurationWithTheShortWaitTimeOut()
        {
            const int OneSecond = 1;

            var configurations = DotNetCommandUtils.GetObjectConfigurationFromDefault();
            configurations.NugetConfigurations.SetNewMaxTimeSecondsTimeOut(OneSecond);
            configurations.SolutionPath = SolutionForRunTests.GetFirstSolutionFile();
            this.scenarioContext.Set(configurations, ConfigurationsKey);
            DotNetCommandUtils.WriteNewConfigurations(configurations);
        }

        [Given("The arguments to execute was '(.*)'")]
        public void GivenTheArgumentsToExecuteWas(string command) => this.scenarioContext.Set(command, CommandToExecuteKey);

        [When("The DotNetCommand is executed")]
        public void WhenTheDotNetCommandIsExecuted()
        {
            var commandToBeExecuted = this.scenarioContext.Get<string>(CommandToExecuteKey);
            SetConfigurationsIfWasSaved();

            var watcher = new Stopwatch();
            watcher.Start();
            var result = this.dotNetCommand.RunCommand(commandToBeExecuted);
            watcher.Stop();

            this.scenarioContext.Set(result, ExecutedCommandResultObjectKey);
            this.scenarioContext.Set(watcher.ElapsedMilliseconds, SpendedTimeToExecuteCommandKey);
        }

        [Then("The succeed result of the execution is '(.*)'")]
        public void ThenTheSucceedResultOfTheExecutionIs(bool expectedSucceedResult)
        {
            var result = GetResultObject();
            result.Should().NotBeNull();
            result.IsSucceed().Should().Be(expectedSucceedResult);
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
            var defaultLongMaximumWait = Convert.ToInt64(this.configManager.GetDefaultMaximumWait().TotalSeconds);
            var timeSpanSpended = TimeSpan.FromMilliseconds(spendedTime);
            var secondsSpended = timeSpanSpended.TotalSeconds;

            secondsSpended.Should().BeLessOrEqualTo(defaultLongMaximumWait);
        }

        private void SetConfigurationsIfWasSaved()
        {
            if (this.scenarioContext.TryGetValue<ExecutionConfigurations>(ConfigurationsKey, out var configurations))
                this.configManager.SetConfigurations(configurations);
        }

        private Result GetResultObject() => this.scenarioContext.Get<Result>(ExecutedCommandResultObjectKey);
    }
}