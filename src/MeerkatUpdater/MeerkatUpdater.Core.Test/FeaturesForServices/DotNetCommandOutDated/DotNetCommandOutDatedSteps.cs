using FluentAssertions;
using MeerkatUpdater.Core.Config.Manager;
using MeerkatUpdater.Core.Runner.Command.DotNetOutDated;
using MeerkatUpdater.Core.Runner.Model.PackageInfo;
using MeerkatUpdater.Core.Test.FeaturesForServices.DotNetCommand;
using System.Collections.Generic;
using TechTalk.SpecFlow;

namespace MeerkatUpdater.Core.Test.FeaturesForServices.DotNetCommandOutDated
{
    [Binding]
    public class DotNetCommandOutDatedSteps
    {
        private const string OutDatedResultKey = "outDatedResult";
        private readonly ScenarioContext scenarioContext;
        private readonly IConfigManager configManager;
        private readonly IOutDated outDated;

        public DotNetCommandOutDatedSteps(ScenarioContext scenarioContext, IConfigManager configManager, IOutDated outDated) =>
            (this.scenarioContext, this.configManager, this.outDated) = (scenarioContext, configManager, outDated);

        [When("The OutDated is executed")]
        public void WhenTheOutDatedIsExecuted()
        {
            DotNetCommandUtils.SetConfigurationsIfWasSaved(this.scenarioContext, this.configManager);
            var result = this.outDated.Execute();
            this.scenarioContext.Set(result, OutDatedResultKey);
        }

        [Then("The list of project info should not be null")]
        public void ThenTheListOfProjectInfoShouldNotBeNull()
        {
            var result = this.scenarioContext.Get<List<ProjectInfo>>(OutDatedResultKey);
            result.Should().NotBeNull();
            result.Should().HaveCountGreaterThan(0);
        }

        [Then("The project info should have informatinos about the packages")]
        public void ThenTheProjectInfoShouldHaveInformatinosAboutThePackages()
        {
            foreach (var project in this.scenarioContext.Get<List<ProjectInfo>>(OutDatedResultKey))
            {
                project.Name.Should().NotBeNullOrWhiteSpace();
                project.Path.Should().BeNullOrWhiteSpace();
                project.TargetFramework.Should().NotBeNullOrWhiteSpace();
                project.InstalledPackages.Should().NotBeNull();
                project.InstalledPackages.Should().HaveCountGreaterThan(0);
                foreach (var package in project.InstalledPackages)
                {
                    package.SemanticVersionChange.Should().NotBeNull();
                    package.Name.Should().NotBeNullOrWhiteSpace();
                    package.Current.Should().NotBeNull();
                    package.Current.Version.Should().NotBeNull();
                    package.Latest.Should().NotBeNull();
                    package.Latest.Version.Should().NotBeNull();
                }
            }
        }
    }
}