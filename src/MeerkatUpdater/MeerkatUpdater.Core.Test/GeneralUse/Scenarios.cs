using BoDi;
using MeerkatUpdater.Core.Config.Manager;
using MeerkatUpdater.Core.Runner.Command.DotNet;
using MeerkatUpdater.Core.Runner.Command.DotNetBuild;
using MeerkatUpdater.Core.Runner.Command.DotNetClean;
using MeerkatUpdater.Core.Runner.Command.DotNetContProject;
using MeerkatUpdater.Core.Runner.Command.DotNetOutDated;
using MeerkatUpdater.Core.Runner.Command.DotNetUpdateProcess;
using System.IO;
using TechTalk.SpecFlow;

namespace MeerkatUpdater.Core.Test.GeneralUse
{
    [Binding]
    public class Scenarios
    {
        private const string SolutionPathKey = "SolutionPath";
        private const string OutPutPathForTextKey = "OutPutPathForText";
        private const int MaximumDegreeToFindDirectory = 10;
        private readonly ScenarioContext scenarioContext;
        private readonly IObjectContainer container;

        public Scenarios(ScenarioContext scenarioContext, IObjectContainer container) =>
            (this.scenarioContext, this.container) = (scenarioContext, container);

        public static string FindOutPutBuildPathFromScenario(ScenarioContext scenarioContext) => FindOutPutPath(GetFromScenarioOutPutPath(scenarioContext));

        public static void SaveOutPutPath(ScenarioContext scenarioContext, string path) =>
            scenarioContext.Set(path, OutPutPathForTextKey);

        public static void SaveSolutionPath(ScenarioContext scenarioContext, string solutionPath) =>
            scenarioContext.Set(solutionPath, SolutionPathKey);

        public static string GetSolutionPath(ScenarioContext scenarioContext) =>
            scenarioContext.Get<string>(SolutionPathKey);

        public static string GetFromScenarioOutPutPath(ScenarioContext scenarioContext)
        {
            scenarioContext.TryGetValue(OutPutPathForTextKey, out string path);
            return path;
        }

        [AfterScenario("deleteOutPutTest")]
        public void DeleteOutPutTest()
        {
            var fromScenarioOutPutPath = GetFromScenarioOutPutPath(this.scenarioContext);
            var directory = FindOutPutPath(fromScenarioOutPutPath);
            if (!string.IsNullOrWhiteSpace(directory))
                Directory.Delete(directory, true);
        }

        [BeforeScenario]
        public void PrepareDependencyInjections()
        {
            this.container.RegisterTypeAs<ConfigManager, IConfigManager>();
            this.container.RegisterTypeAs<DotNetCommand, IDotNetCommand>();
            this.container.RegisterTypeAs<Build, IBuild>();
            this.container.RegisterTypeAs<Clean, IClean>();
            this.container.RegisterTypeAs<CountProject, ICountProject>();
            this.container.RegisterTypeAs<OutDated, IOutDated>();
            this.container.RegisterTypeAs<UpdateProcess, IUpdateProcess>();
        }

        private static string FindOutPutPath(string targetPath)
        {
            var root = Directory.GetCurrentDirectory();
            for (int i = 0; i <= MaximumDegreeToFindDirectory; i++)
            {
                var directories = Directory.GetDirectories(root, targetPath);
                if (directories.Length > 0)
                    return directories[0];

                root = Directory.GetParent(root).FullName;
            }

            return string.Empty;
        }
    }
}