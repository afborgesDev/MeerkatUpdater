using MeerkatUpdater.Core.Config.Model;
using System.Collections.Generic;
using System.CommandLine;
using System.Diagnostics.CodeAnalysis;

namespace MeerkatUpdater.Console.Options.InputOptions
{
    [ExcludeFromCodeCoverage]
    public class AllowedVersionToUpdateOption
    {
        public static readonly Option<HashSet<SemanticVersion>> AllowedVersionToUpdateConfigOption = new Option<HashSet<SemanticVersion>>(Aliases, Description);
        private const string Description = "The supported versions during the update [Major, Minor, Path, NoUpdate]";
        private static readonly string[] Aliases = new string[] { "--sources", "-s" };

        public AllowedVersionToUpdateOption(HashSet<SemanticVersion>? allowedVersions) => AllowedVersionsToUpdate = allowedVersions;

        public HashSet<SemanticVersion>? AllowedVersionsToUpdate { get; }
    }
}