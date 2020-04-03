using FluentAssertions;
using MeerkatUpdater.Core.Config;
using MeerkatUpdater.Core.Config.Model;
using MeerkatUpdater.Core.Test.GeneralUse;
using System;
using TechTalk.SpecFlow;

namespace MeerkatUpdater.Core.Test.FeaturesForConfig
{
    [Binding]
    public class YamlDefaultDeserializationSteps
    {
        private const string DefaultPayloadYamlIdentify = "DefaultPayloadYamlIdentify";
        private const string GeneratedClassForYamlDeserealizationIdentify = "GeneratedClassForYamlDeserealizationIdentify";
        private const string DefaulDeserealizationMethodAsActionIdentify = "DefaultDeserealizationMethodAsAction";
        private readonly ScenarioContext scenarioContext;

        public YamlDefaultDeserializationSteps(ScenarioContext scenarioContext) => this.scenarioContext = scenarioContext;

        [Given("That the default generated yaml payload")]
        public void GivenThatTheDefaultGeneratedYamlPayload()
        {
            var defaultPayload = DefaultConfigYmlGenerator.GenerateDefaultConfigurations();
            this.scenarioContext.Set(defaultPayload, DefaultPayloadYamlIdentify);
        }

        [Given("That the payload '(.*)'")]
        public void GivenThatThePayload(string stringPayload) => this.scenarioContext.Set(stringPayload, DefaultPayloadYamlIdentify);

        [Given("That the json payload '(.*)'")]
        public void GivenThatTheJsonPayload(string payloadIdentify)
        {
            var payload = EmbededResourcesUtils.GetEmbededResource(payloadIdentify, "FeaturesForConfig");
            this.scenarioContext.Set(payload, DefaultPayloadYamlIdentify);
        }

        [When("the default deserealization is triggered")]
        public void WhenTheDefaultDeserealizationIsTriggered()
        {
            var defaultPayload = GetDefaultYamlPayloadFromScenario();
            var deserializedClass = DefaultYmlDeserializer.Deserialize<ExecutionConfigurations>(defaultPayload);
            this.scenarioContext.Set(deserializedClass, GeneratedClassForYamlDeserealizationIdentify);
        }

        [When("the expected execution for deserealization is prepared")]
        public void WhenTheExpectedExecutionForDeserealizationIsPrepared()
        {
            var defaultPayload = StringUtils.TableStringPareser(GetDefaultYamlPayloadFromScenario());
            ExecutionConfigurations DefaultDeserealization() => DefaultYmlDeserializer.Deserialize<ExecutionConfigurations>(defaultPayload);
            this.scenarioContext.Set((Func<ExecutionConfigurations>)DefaultDeserealization, DefaulDeserealizationMethodAsActionIdentify);
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
            deserializedClass.NugetConfigurations.Sources.Count.Should().Be(1);
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

        [Then("the execution results into a Exception")]
        public void ThenTheExecutionResultsIntoAException()
        {
            var deserealizationMethod = this.scenarioContext.Get<Func<ExecutionConfigurations>>(DefaulDeserealizationMethodAsActionIdentify);
            deserealizationMethod.Should().Throw<Exception>();
        }

        [Then("the execution results into null")]
        public void ThenTheExecutionResultsIntoNull()
        {
            var deserealizationMethod = this.scenarioContext.Get<Func<ExecutionConfigurations>>(DefaulDeserealizationMethodAsActionIdentify);
            var deserealizationResult = deserealizationMethod.Invoke();
            deserealizationResult.Should().BeNull();
        }

        private string GetDefaultYamlPayloadFromScenario() => this.scenarioContext.Get<string>(DefaultPayloadYamlIdentify);

        private ExecutionConfigurations GetDeserializedClassFromContext() => this.scenarioContext.Get<ExecutionConfigurations>(GeneratedClassForYamlDeserealizationIdentify);
    }
}