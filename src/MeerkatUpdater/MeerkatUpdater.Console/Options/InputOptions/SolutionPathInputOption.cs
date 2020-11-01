using System.CommandLine;
using System.Diagnostics.CodeAnalysis;

namespace MeerkatUpdater.Console.Options.InputOptions
{
    [ExcludeFromCodeCoverage]
    public class SolutionPathInputOption
    {
        public static readonly Option SolutionPathConfigOption = new Option(Aliases, Description);
        private const string Description = "To indicate the solution for update check";
        private static readonly string[] Aliases = new string[] { "--slnPath", "-sln" };

        public SolutionPathInputOption(string? solutionPath) => this.SolutionPath = solutionPath;

        public string? SolutionPath { get; set; }
    }
}