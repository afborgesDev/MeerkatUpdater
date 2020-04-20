using FluentAssertions;
using MeerkatUpdater.Core.Config.Model;
using MeerkatUpdater.Core.Runner.Command.DotNet;
using MeerkatUpdater.Core.Runner.Model.DotNet;
using MeerkatUpdater.Core.Test.GeneralUsage;
using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Xunit;

namespace MeerkatUpdater.Core.Test.TestsForServices.DotNetCommandTests
{
    public class DotNetCommandTest
    {
        private readonly Regex DotNetVersionPattern = new Regex("([0-9]{1,}([.])?)", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline);

        //Get version of the installed dotnet works well with valid configurations
        [Fact]
        public void ShouldReturnInstalleDotNetVersionWhenUsingValidConfigurations()
        {
            const string TestKey = "returnVersionDotNetCommand";
            var dotnetCommand = BuildDotNetCommand(TestKey);

            try
            {
                var (_, result) = ExecuteDotNetCommand(dotnetCommand, "--version");

                BasicValidationsDotNetCommand(result, true, false, true);
                var hasMatch = DotNetVersionPattern.Matches(result.Output);
                hasMatch.Count.Should().BeGreaterThan(0);
            }
            finally
            {
                CreatedFoldersManager.TierDownTest(TestKey);
            }
        }

        [Fact]
        public void ShouldCommandExecuteByTheTimeOutDefaultIfDoesntHaveAnyValidConfigurationForThat()
        {
            const string TestKey = "shouldUseDefaultTimeOutDotNetCommnand";

            try
            {
                var configurations = CreateCommandObjects.GetObjectConfigurationFromDefault();
                var configManager = CreateCommandObjects.CreateConfigManager(TestKey);
                configurations.NugetConfigurations = new NugetConfigurations();
                configManager.SetConfigurations(configurations);
                var dotNetCommand = new DotNetCommand(configManager);
                var (watcher, result) = ExecuteDotNetCommand(dotNetCommand, "--version");
                BasicValidationsDotNetCommand(result, true, false, true);

                var defaultLongMaximumWait = Convert.ToInt64(configManager.GetDefaultMaximumWait().TotalSeconds);
                var timeSpanSpended = TimeSpan.FromMilliseconds(watcher.ElapsedMilliseconds);
                var secondsSpended = timeSpanSpended.TotalSeconds;

                secondsSpended.Should().BeLessOrEqualTo(defaultLongMaximumWait);
            }
            finally
            {
                CreatedFoldersManager.TierDownTest(TestKey);
            }
        }

        [Fact]
        public void ShortTimeOutShouldKillTaskAndResltIntoNotSucceedExecution()
        {
            const string TestKey = "shortTimeOutKillTask";
            const int OneSecond = 1;

            try
            {
                var configManager = CreateCommandObjects.CreateConfigManager(TestKey);
                var configuarations = configManager.GetConfigurations();
                configuarations.NugetConfigurations.SetNewMaxTimeSecondsTimeOut(OneSecond);
                configManager.SetConfigurations(configuarations);
                var dotNetCommand = new DotNetCommand(configManager);

                var (_, result) = ExecuteDotNetCommand(dotNetCommand, "build -v d");
                BasicValidationsDotNetCommand(result, false, false, false);
            }
            finally
            {
                CreatedFoldersManager.TierDownTest(TestKey);
            }
        }

        private IDotNetCommand BuildDotNetCommand(string testKey)
        {
            var configurations = CreateCommandObjects.CreateConfigManager(testKey);
            return CreateCommandObjects.CreateDotNetCommand(configurations);
        }

        private (Stopwatch, Result) ExecuteDotNetCommand(IDotNetCommand dotNetCommand, string command)
        {
            var watcher = new Stopwatch();
            watcher.Start();
            var result = dotNetCommand.RunCommand(command);
            watcher.Stop();

            return (watcher, result);
        }

        private void BasicValidationsDotNetCommand(Result result, bool isSucceed, bool shouldHaveError, bool shouldHaveOutPut)
        {
            result.Should().NotBeNull();
            result.IsSucceed().Should().Be(isSucceed);
            if (shouldHaveError)
                result.Errors.Should().NotBeNullOrWhiteSpace();
            else
                result.Errors.Should().BeNullOrWhiteSpace();

            if (shouldHaveOutPut)
                result.Output.Should().NotBeNullOrWhiteSpace();
            else
                result.Output.Should().BeNullOrWhiteSpace();
        }
    }
}