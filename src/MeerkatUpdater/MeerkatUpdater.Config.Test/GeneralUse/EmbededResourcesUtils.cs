using System.IO;
using System.Reflection;
using System.Text;

namespace MeerkatUpdater.Config.Test.GeneralUse
{
    public static class EmbededResourcesUtils
    {
        private const string TargetAssemblyTest = "MeerkatUpdater.Config.Test";

        public static string GetEmbededResource(string identify)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceStream = assembly.GetManifestResourceStream($"{TargetAssemblyTest}.payloads.{identify}");
            using var reader = new StreamReader(resourceStream, Encoding.UTF8);
            return reader.ReadToEnd();
        }
    }
}