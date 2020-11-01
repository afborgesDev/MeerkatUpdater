using System.CommandLine;
using System.Diagnostics.CodeAnalysis;

namespace MeerkatUpdater.Console.Options.InputOptions
{
    [ExcludeFromCodeCoverage]
    public class YmlConfigInputOption
    {
        public static readonly Option YmlConfigOption = new Option(Aliases, Description);
        private const string Description = "To indicate the YAML configuration file";
        private static readonly string[] Aliases = new string[] { "--ymlconfig", "-yc" };

        public YmlConfigInputOption(string? ymlFile) => YmlFile = ymlFile;

        public string? YmlFile { get; set; }
    }
}