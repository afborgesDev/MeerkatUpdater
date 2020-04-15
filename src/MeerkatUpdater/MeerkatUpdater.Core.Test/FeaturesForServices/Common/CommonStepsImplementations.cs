using MeerkatUpdater.Core.Test.FeaturesForServices.DotNetCommand;
using MeerkatUpdater.Core.Test.GeneralUse;
using TechTalk.SpecFlow;

namespace MeerkatUpdater.Core.Test.FeaturesForServices.Common
{
    [Binding]
    public class CommonStepsImplementations
    {
        private readonly ScenarioContext scenarioContext;

        public CommonStepsImplementations(ScenarioContext scenarioContext) =>
            this.scenarioContext = scenarioContext;

        [Given("The valid configurations with the solution path for outputPath '(.*)'")]
        public void GivenTheValidConfigurationsWithTheSolutionPathForOutputPath(string outputTestPath)
        {
            var configurations = DotNetCommandUtils.GetObjectConfigurationFromDefault();
            configurations.SolutionPath = SolutionFinder.GetFirstSolutionFile();
            configurations.OutPutPath = outputTestPath;
            configurations.NugetConfigurations.SetNewMaxTimeSecondsTimeOut(30);
            this.scenarioContext.Set(configurations, DotNetCommandUtils.ConfigurationsKey);
            Scenarios.SaveOutPutPath(this.scenarioContext, outputTestPath);
        }
    }
}