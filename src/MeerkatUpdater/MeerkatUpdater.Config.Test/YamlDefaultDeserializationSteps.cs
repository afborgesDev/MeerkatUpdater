using FluentAssertions;
using MeerkatUpdater.Config.Model;
using TechTalk.SpecFlow;

namespace MeerkatUpdater.Config.Test
{
    [Binding]
    public class YamlDefaultDeserializationSteps
    {
        private const string DefaultPayloadYamlIdentify = "DefaultPayloadYamlIdentify";
        private const string GeneratedClassForYamlDeserealizationIdentify = "GeneratedClassForYamlDeserealizationIdentify";

        private readonly ScenarioContext scenarioContext;

        public YamlDefaultDeserializationSteps(ScenarioContext scenarioContext) => this.scenarioContext = scenarioContext;

        [Given("That the default generated yaml payload")]
        public void GivenThatTheDefaultGeneratedYamlPayload()
        {
            var defaultPayload = DefaultConfigYmlGenerator.GenerateDefaultConfigurations();
            this.scenarioContext.Set(defaultPayload, DefaultPayloadYamlIdentify);
        }

        [When("the default deserealization is triggered")]
        public void WhenTheDefaultDeserealizationIsTriggered()
        {
            var defaultPayload = this.scenarioContext.Get<string>(DefaultPayloadYamlIdentify);
            var deserializedClass = DefaultYmlDeserializer.Deserialize<ExecutionConfigurations>(defaultPayload);
            this.scenarioContext.Set(deserializedClass, GeneratedClassForYamlDeserealizationIdentify);
        }

        [Then("the object result a not null ExecutionConfigClass")]
        public void ThenTheObjectResultANotNullExecutionConfigClass()
        {
            var deserializedClass = GetDeserializedClassFromContext();
            deserializedClass.Should().NotBeNull();
        }

        [Then("the NugetConfigurations is not null")]
        public void ThenTheNugetConfigurationsIsNotNull()
        {
            var deserializedClass = GetDeserializedClassFromContext();
            deserializedClass.NugetConfigurations.Should().NotBeNull();
        }

        [Then("the UpdateConfigurations is not null")]
        public void ThenTheUpdateConfigurationsIsNotNull()
        {
            var deserializedClass = GetDeserializedClassFromContext();
            deserializedClass.UpdateConfigurations.Should().NotBeNull();
        }

        [Then("the SolutionPath has the default solutionPath")]
        public void ThenTheSolutionPathHasTheDefaultSolutionPath()
        {
            var deserializedClass = GetDeserializedClassFromContext();
            deserializedClass.SolutionPath.Should().Be(DefaultConfigYmlGenerator.DefaultSolutionPath);
        }

        [Then("the LogLevel has the default LogLevel")]
        public void ThenTheLogLevelHasTheDefaultLogLevel()
        {
            var deserializedClass = GetDeserializedClassFromContext();
            deserializedClass.LogLevel.Should().Be(DefaultConfigYmlGenerator.DefaultLogLevel);
        }

        private ExecutionConfigurations GetDeserializedClassFromContext() => this.scenarioContext.Get<ExecutionConfigurations>(GeneratedClassForYamlDeserealizationIdentify);
    }
}