using System.IO;
using System.Reflection;
using System.Text;

namespace MeerkatUpdater.Core.Test.GeneralUsage
{
    public static class EmbededResourcesUtils
    {
        private const string TargetAssemblyTest = "MeerkatUpdater.Core.Test";

        public static void SaveEmbededResourceToFile(string rootPath, string targetFileName,
            string embededResourceIdentify, string embededResourAditionalPath = "")
        {
            var assembly = Assembly.GetExecutingAssembly();
            var path = BuildPath(embededResourceIdentify, embededResourAditionalPath);
            if (!Directory.Exists(rootPath))
                Directory.CreateDirectory(rootPath);

            var fileName = Path.Combine(rootPath, targetFileName);
            using var resourceStream = assembly.GetManifestResourceStream(path);
            using var file = new FileStream(fileName, FileMode.Create, FileAccess.Write);
            resourceStream.CopyTo(file);
        }

        public static string GetEmbededResource(string identify, string aditionalPath = "")
        {
            var assembly = Assembly.GetExecutingAssembly();
            var path = BuildPath(identify, aditionalPath);
            using var resourceStream = assembly.GetManifestResourceStream(path);
            using var reader = new StreamReader(resourceStream, Encoding.UTF8);
            return reader.ReadToEnd();
        }

        private static string BuildPath(string identify, string aditionalPath)
        {
            if (string.IsNullOrWhiteSpace(aditionalPath))
                return $"{TargetAssemblyTest}.payloads.{identify}";
            return $"{TargetAssemblyTest}.{aditionalPath}.payloads.{identify}";
        }
    }
}