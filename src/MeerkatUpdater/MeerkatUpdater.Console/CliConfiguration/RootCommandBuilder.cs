using MeerkatUpdater.Console.Options.InputOptions;
using System.CommandLine;

namespace MeerkatUpdater.Console.CliConfiguration
{
    public static class RootCommandBuilder
    {
        public static RootCommand Build() => new RootCommand()
            {
                YmlConfigInputOption.YmlConfigOption,
                SolutionPathInputOption.SolutionPathConfigOption,
                NugetSourcesOption.NugetSourcesConfigOption,
                AllowedVersionToUpdateOption.AllowedVersionToUpdateConfigOption
            };
    }
}