using System.IO;
using System.IO.Compression;

namespace MeerkatUpdater.Core.Test.GeneralUse
{
    public static class SolutionForRunTests
    {
        public const string EmbededPayloadKey = "PayloadSolutionForTest.zip";
        public const string EmbededPayloadFolderKey = "PayloadSolutionForTest";
        public const string PayloadSolutionFileName = EmbededPayloadFolderKey + ".sln";
        private const int MaximumDegreeToFindSolution = 10;
        private const string SolutionFilter = "*.sln";

        public static string GetFirstSolutionFile()
        {
            var root = Directory.GetCurrentDirectory();
            for (int i = 0; i <= MaximumDegreeToFindSolution; i++)
            {
                var slnFile = Directory.GetFiles(root, SolutionFilter);
                if (slnFile.Length > 0)
                    return slnFile[0];
                root = Directory.GetParent(root).FullName;
            }
            return default;
        }

        public static void ExtractPayloadForSolutionTest(string rootPath)
        {
            EmbededResourcesUtils.SaveEmbededResourceToFile(rootPath, EmbededPayloadKey, EmbededPayloadKey, EmbededPayloadFolderKey);
            rootPath = Path.Combine(Directory.GetCurrentDirectory(), rootPath);

            var fileName = Path.Combine(rootPath, EmbededPayloadKey);
            var destPath = Path.Combine(rootPath, EmbededPayloadFolderKey);

            ZipFile.ExtractToDirectory(fileName, destPath);
        }
    }
}