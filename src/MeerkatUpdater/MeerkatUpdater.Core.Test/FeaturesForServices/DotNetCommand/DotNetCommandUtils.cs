using MeerkatUpdater.Core.Config;
using MeerkatUpdater.Core.Config.Model;
using System.IO;

namespace MeerkatUpdater.Core.Test.FeaturesForServices.DotNetCommand
{
    public static class DotNetCommandUtils
    {
        public static ExecutionConfigurations GetObjectConfigurationFromDefault()
        {
            var payload = DefaultConfigYmlGenerator.GenerateDefaultConfigurations();
            return DefaultYmlDeserializer.Deserialize<ExecutionConfigurations>(payload);
        }

        public static void WriteNewConfigurations(ExecutionConfigurations configurations)
        {
            var payload = DefaultConfigYmlGenerator.BuildYmlFile(configurations);
            File.WriteAllText(DefaultConfigYmlGenerator.DefaultConfigurationFileName, payload);
        }
    }
}