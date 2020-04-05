using System.IO;
using TechTalk.SpecFlow;

namespace MeerkatUpdater.Core.Test.GeneralUse
{
    [Binding]
    public class Scenarios
    {
        private const int MaximumDegreeToFindDirectory = 10;
        private const string OutPutBuild = "outputTest";

        [AfterScenario("deleteOutPutTest")]
        public static void DeleteOutPutTest()
        {
            var root = Directory.GetCurrentDirectory();
            for (int i = 0; i <= MaximumDegreeToFindDirectory; i++)
            {
                var directories = Directory.GetDirectories(root, OutPutBuild);
                if (directories.Length > 0)
                {
                    Directory.Delete(directories[0], true);
                    return;
                }
                root = Directory.GetParent(root).FullName;
            }
        }
    }
}