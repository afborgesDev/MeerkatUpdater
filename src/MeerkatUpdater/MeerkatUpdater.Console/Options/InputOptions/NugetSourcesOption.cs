using System.Collections.Generic;
using System.CommandLine;
using System.Diagnostics.CodeAnalysis;

namespace MeerkatUpdater.Console.Options.InputOptions
{
    [ExcludeFromCodeCoverage]
    public class NugetSourcesOption
    {
        public static readonly Option<string[]> NugetSourcesConfigOption = new Option<string[]>(Aliases, Description);
        private const string Description = "set the list of nuget repositories to used during update";
        private static readonly string[] Aliases = new string[] { "--sources", "-s" };

        public NugetSourcesOption(string[]? sources) => Sources = sources;

        public IEnumerable<string>? Sources { get; }
    }
}