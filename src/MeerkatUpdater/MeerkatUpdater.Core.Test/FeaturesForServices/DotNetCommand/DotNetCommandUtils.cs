using MeerkatUpdater.Core.Config.DefaultServices;
using MeerkatUpdater.Core.Config.Manager;
using MeerkatUpdater.Core.Config.Model;
using System.IO;
using TechTalk.SpecFlow;

namespace MeerkatUpdater.Core.Test.FeaturesForServices.DotNetCommand
{
    public static class DotNetCommandUtils
    {
        public const string ConfigurationsKey = "configurations";
        public const string ExecutedCommandResultObjectKey = "executedCommandResultObject";

        public static ExecutionConfigurations GetObjectConfigurationFromDefault()
        {
            var payload = DefaultConfigYmlGenerator.GenerateDefaultConfigurations();
            var configurations = DefaultYmlDeserializer.Deserialize<ExecutionConfigurations>(payload);
            configurations.OutPutPath = ConfigManager.DefaultTestOutput;
            return configurations;
        }

        public static void WriteNewConfigurations(ExecutionConfigurations configurations)
        {
            var payload = DefaultConfigYmlGenerator.BuildYmlFile(configurations);
            File.WriteAllText(DefaultConfigYmlGenerator.DefaultConfigurationFileName, payload);
        }

        public static void SetConfigurationsIfWasSaved(ScenarioContext scenarioContext)
        {
            if (scenarioContext.TryGetValue<ExecutionConfigurations>(ConfigurationsKey, out var configurations))
                ConfigManager.ExecutionConfigurations = configurations;
        }
    }
}