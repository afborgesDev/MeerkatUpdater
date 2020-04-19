using FluentAssertions;
using MeerkatUpdater.Core.Config.DefaultServices;
using MeerkatUpdater.Core.Test.GeneralUsage;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Xunit;

namespace MeerkatUpdater.Core.Test.TestsForServices.DotNetCommand
{
    public class DotNetCommandTest
    {
        private readonly Regex DotNetVersionPattern = new Regex("([0-9]{1,}([.])?)", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline);

        //Get version of the installed dotnet works well with valid configurations
        [Fact]
        public void ShouldReturnInstalleDotNetVersionWhenUsingValidConfigurations()
        {
            DefaultConfigYmlGenerator.GenerateYmlFileForDefaultConfigurations("MeerkatUpdater.yml");
            var configurations = CreateCommandObjects.CreateConfigManager("returnVersionDotNetCommand");
            var dotnetCommand = CreateCommandObjects.CreateDotNetCommand(configurations);

            var watcher = new Stopwatch();
            watcher.Start();
            var result = dotnetCommand.RunCommand("--version");
            watcher.Stop();

            result.Should().NotBeNull();
            result.IsSucceed().Should().BeTrue();
            result.Errors.Should().BeNullOrEmpty();
            result.Output.Should().NotBeNullOrEmpty();
            var hasMatch = DotNetVersionPattern.Matches(result.Output);
            hasMatch.Count.Should().BeGreaterThan(0);
        }
    }
}