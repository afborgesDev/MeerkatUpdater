using System.IO;

namespace MeerkatUpdater.Core.Test.GeneralUse
{
    public static class SolutionFinder
    {
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
    }
}