using System.IO;
using System.Reflection;
using System.Text;

namespace MeerkatUpdater.Core.Test.GeneralUse
{
    public static class EmbededResourcesUtils
    {
        private const string TargetAssemblyTest = "MeerkatUpdater.Core.Test";

        public static string GetEmbededResource(string identify, string aditionalPath = "")
        {
            var assembly = Assembly.GetExecutingAssembly();
            var path = BuildPath(identify, aditionalPath);
            var resourceStream = assembly.GetManifestResourceStream(path);
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