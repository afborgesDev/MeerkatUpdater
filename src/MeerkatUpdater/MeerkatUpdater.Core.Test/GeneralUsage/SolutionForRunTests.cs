using System.IO;
using System.IO.Compression;

namespace MeerkatUpdater.Core.Test.GeneralUsage
{
    public static class SolutionForRunTests
    {
        public const string EmbededPayloadKey = "PayloadSolutionForTest.zip";
        public const string EmbededPayloadFolderKey = "PayloadSolutionForTest";
        public const string PayloadSolutionFileName = EmbededPayloadFolderKey + ".sln";

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