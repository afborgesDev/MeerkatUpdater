using MeerkatUpdater.Core.Config;
using System.IO;
using TechTalk.SpecFlow;

namespace MeerkatUpdater.Core.Test.GeneralUse
{
    [Binding]
    public class Scenarios
    {
        private const string OutPutPathForTextKey = "OutPutPathForText";
        private const int MaximumDegreeToFindDirectory = 10;
        private readonly ScenarioContext scenarioContext;

        public Scenarios(ScenarioContext scenarioContext) => this.scenarioContext = scenarioContext;

        public static string FindOutPutBuildPath() => FindOutPutPath(ConfigManager.DefaultTestOutput);

        public static string FindOutPutBuildPathFromScenario(ScenarioContext scenarioContext) => FindOutPutPath(GetFromScenarioOutPutPath(scenarioContext));

        public static void SaveOutPutPath(ScenarioContext scenarioContext, string path) =>
            scenarioContext.Set(path, OutPutPathForTextKey);

        public static string GetFromScenarioOutPutPath(ScenarioContext scenarioContext)
        {
            scenarioContext.TryGetValue(OutPutPathForTextKey, out string path);
            return path;
        }

        [AfterScenario("deleteOutPutTest")]
        public void DeleteOutPutTest()
        {
            var directory = FindOutPutBuildPath();
            if (!string.IsNullOrWhiteSpace(directory))
                Directory.Delete(directory, true);

            var fromScenarioOutPutPath = GetFromScenarioOutPutPath(this.scenarioContext);
            directory = FindOutPutPath(fromScenarioOutPutPath);
            if (!string.IsNullOrWhiteSpace(directory))
                Directory.Delete(directory, true);
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