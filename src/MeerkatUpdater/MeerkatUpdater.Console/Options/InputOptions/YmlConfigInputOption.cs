using System.CommandLine;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace MeerkatUpdater.Console.Options.InputOptions
{
    [ExcludeFromCodeCoverage]
    public class YmlConfigInputOption
    {
        public static readonly Option<FileInfo> YmlConfigOption = new Option<FileInfo>(Aliases, Description).ExistingOnly();
        private const string Description = "To indicate the YAML configuration file";
        private static readonly string[] Aliases = new string[] { "--ymlconfig", "-yc" };

        public YmlConfigInputOption(string? ymlFile) => YmlFile = ymlFile;

        public string? YmlFile { get; set; }
    }
}