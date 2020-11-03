using System.CommandLine;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace MeerkatUpdater.Console.Options.InputOptions
{
    [ExcludeFromCodeCoverage]
    public class SolutionPathInputOption
    {
        public static readonly Option<FileInfo> SolutionPathConfigOption = new Option<FileInfo>(Aliases, Description).ExistingOnly();
        private const string Description = "To indicate the solution for update check";
        private static readonly string[] Aliases = new string[] { "--slnPath", "-sln" };

        public SolutionPathInputOption(string? solutionPath) => this.SolutionPath = solutionPath;

        public string? SolutionPath { get; set; }
    }
}