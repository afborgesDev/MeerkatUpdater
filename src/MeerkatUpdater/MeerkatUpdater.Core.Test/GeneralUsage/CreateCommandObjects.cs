using MeerkatUpdater.Core.Config.DefaultServices;
using MeerkatUpdater.Core.Config.Manager;
using MeerkatUpdater.Core.Config.Model;
using MeerkatUpdater.Core.Runner.Command.DotNet;
using MeerkatUpdater.Core.Runner.Command.DotNetBuild;
using MeerkatUpdater.Core.Runner.Command.DotNetClean;
using MeerkatUpdater.Core.Runner.Command.DotNetContProject;
using MeerkatUpdater.Core.Runner.Command.DotNetOutDated;
using System.IO;

namespace MeerkatUpdater.Core.Test.GeneralUsage
{
    public static class CreateCommandObjects
    {
        public static IConfigManager CreateConfigManager(string testKey)
        {
            var folderName = CreatedFoldersManager.GenerateNewFolder(testKey);
            var solution = PrepareSolutionForCustomOutPutPath(folderName);

            var configurations = GetObjectConfigurationFromDefault();
            configurations.SolutionPath = solution;
            configurations.OutPutPath = folderName;
            configurations.NugetConfigurations.SetNewMaxTimeSecondsTimeOut(30);

            var configManager = new ConfigManager();
            configManager.SetConfigurations(configurations);

            return configManager;
        }

        public static IDotNetCommand CreateDotNetCommand(IConfigManager configManager) => new DotNetCommand(configManager);

        public static IBuild CreateBuildCommand(IConfigManager configManager, IDotNetCommand dotNetCommand) => new Build(dotNetCommand, configManager);

        public static IClean CreateCleanCommand(IConfigManager configManager, IDotNetCommand dotNetCommand) => new Clean(dotNetCommand, configManager);

        public static ICountProject CreateCountProject(IDotNetCommand dotNetCommand) => new CountProject(dotNetCommand);

        public static IOutDated CreateOutDatedCommand(IConfigManager configManager, IBuild build, IDotNetCommand dotNetCommand) => new OutDated(dotNetCommand, build, configManager);

        public static ExecutionConfigurations GetObjectConfigurationFromDefault()
        {
            var payload = DefaultConfigYmlGenerator.GenerateDefaultConfigurations();
            return DefaultYmlDeserializer.Deserialize<ExecutionConfigurations>(payload);
        }

        private static string PrepareSolutionForCustomOutPutPath(string folderName)
        {
            SolutionForRunTests.ExtractPayloadForSolutionTest(folderName);
            return Path.Combine(folderName, SolutionForRunTests.EmbededPayloadFolderKey, SolutionForRunTests.PayloadSolutionFileName);
        }
    }
}