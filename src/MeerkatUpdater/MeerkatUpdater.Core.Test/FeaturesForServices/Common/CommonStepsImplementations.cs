using MeerkatUpdater.Core.Test.FeaturesForServices.DotNetCommand;
using MeerkatUpdater.Core.Test.GeneralUse;
using System.IO;
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
            configurations.SolutionPath = PrepareSolutionForCustomOutPutPath(outputTestPath);
            configurations.OutPutPath = outputTestPath;
            configurations.NugetConfigurations.SetNewMaxTimeSecondsTimeOut(30);

            this.scenarioContext.Set(configurations, DotNetCommandUtils.ConfigurationsKey);
            Scenarios.SaveOutPutPath(this.scenarioContext, outputTestPath);
        }

        private string PrepareSolutionForCustomOutPutPath(string outputTestPath)
        {
            SolutionForRunTests.ExtractPayloadForSolutionTest(outputTestPath);
            var solutionPath = Path.Combine(Directory.GetCurrentDirectory(), outputTestPath, SolutionForRunTests.EmbededPayloadFolderKey, SolutionForRunTests.PayloadSolutionFileName);
            if (File.Exists(solutionPath))
                Scenarios.SaveSolutionPath(this.scenarioContext, solutionPath);

            return solutionPath;
        }
    }
}