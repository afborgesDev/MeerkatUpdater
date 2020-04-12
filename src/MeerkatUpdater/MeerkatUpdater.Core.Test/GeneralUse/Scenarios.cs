using System.IO;
using TechTalk.SpecFlow;

namespace MeerkatUpdater.Core.Test.GeneralUse
{
    [Binding]
    public static class Scenarios
    {
        private const int MaximumDegreeToFindDirectory = 10;
        private const string OutPutBuild = "outputTest";

        [AfterScenario("deleteOutPutTest")]
        public static void DeleteOutPutTest()
        {
            var directory = FindOutPutBuildPath();
            if (!string.IsNullOrWhiteSpace(directory))
                Directory.Delete(directory, true);
        }

        public static string FindOutPutBuildPath()
        {
            var root = Directory.GetCurrentDirectory();
            for (int i = 0; i <= MaximumDegreeToFindDirectory; i++)
            {
                var directories = Directory.GetDirectories(root, OutPutBuild);
                if (directories.Length > 0)
                    return directories[0];

                root = Directory.GetParent(root).FullName;
            }

            return string.Empty;
        }
    }
}