using MeerkatUpdater.Core.Runner.Scraper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeerkatUpdater.Core.Runner.Model.PackageInfo
{
    /// <summary>
    /// Information about the project into a solution
    /// </summary>
    public sealed class ProjectInfo
    {
        /// <summary>
        /// The project name
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// The project targetframework
        /// </summary>
        public string? TargetFramework { get; set; }

        /// <summary>
        /// The project path
        /// </summary>
        public string? Path { get; set; }

        /// <summary>
        /// The list of instaled packages into the project
        /// </summary>
        public IEnumerable<InstalledPackage> InstalledPackages { get; set; } = new List<InstalledPackage>();

        /// <summary>
        /// Build a new complet project setting the semantic version for each installed package
        /// </summary>
        /// <param name="name"></param>
        /// <param name="targetFrameWork"></param>
        /// <param name="installedPackages"></param>
        /// <returns></returns>
        public static ProjectInfo BuildCompleteProjectinfo(string? name, string? targetFrameWork, IEnumerable<InstalledPackage> installedPackages)
        {
            var projectInfo = new ProjectInfo
            {
                Name = name,
                TargetFramework = targetFrameWork,
                InstalledPackages = installedPackages
            };

            SemanticVersionAnalizer.SetSemanticVersionChange(ref projectInfo);

            return projectInfo;
        }

        /// <summary>
        /// Indicate if any package was updated and than needs to be pushed into the repository
        /// </summary>
        /// <returns></returns>
        public bool HasItemsToCommit() => InstalledPackages.Any(x => x.UpdatedAt != null);

        /// <summary>
        /// Returns a report about the project
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var builder = new StringBuilder();
            BuildHeader(ref builder);
            BuildBody(ref builder);
            BuildFooter(ref builder);
            return builder.ToString();
        }

        private void BuildFooter(ref StringBuilder builder) => builder.Append(Environment.NewLine);

        private void BuildBody(ref StringBuilder builder)
        {
            foreach (var installedPackages in InstalledPackages)
            {
                builder.Append("    * Name: ").Append(installedPackages.Name).Append(Environment.NewLine);
                builder.Append("        Version: ").Append(installedPackages?.Current?.Version).Append(Environment.NewLine);
                builder.Append("         Latest: ").Append(installedPackages?.Latest?.Version).Append(Environment.NewLine);
            }
        }

        private void BuildHeader(ref StringBuilder builder)
        {
            builder.Append(">>     Project name: ").Append(Name).Append(Environment.NewLine);
            builder.Append(">> Target frameWork: ").Append(TargetFramework).Append(Environment.NewLine);
            builder.Append(">>   Installed libs:").Append(Environment.NewLine);
        }
    }
}