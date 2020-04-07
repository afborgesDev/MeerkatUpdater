using MeerkatUpdater.Core.Runner.Model.PackageInfo;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MeerkatUpdater.Core.Runner.Scraper
{
    internal static class OutPutPayloadToProjectInfo
    {
        private const string ProjectNameIdentify = "Project `";
        private const string EscapeIdentify = "`";
        private const string UnrecognizedMessage = "Unrecognized";
        private const string StartRegionBySquareBrackets = "[";
        private const string EndRegionBySquareBrackets = "]";
        private const int StartPositionForLibNameIndex = 2;
        private const int StartPositionForCleanedLibNameIndex = 0;
        private const string BlankSpaceSeparator = " ";
        private const int LibVersionHasNecessaryInformationForExtractVersion = 3;
        private const int CurrentVersionIndex = 0;
        private const int LatestVersionIndex = 2;

        private static readonly Regex PackageNameIdentifyRegex = new Regex("([Pp]roject `)([A-Za-z0-9._-]{1,})(`)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly Regex FrameWorkIdentifyRegex = new Regex(@"\[[a-z0-9.]{1,}\]", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly Regex PackageVersionInfosRegex = new Regex("(> [A-Za-z._-]{1,}( ){1,}[a-z0-9. -]{1,})", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly Regex LibVersionInfoRegex = new Regex("[a-z0-9.-]{1,}", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private static List<ProjectInfo>? TransformPayloadsToProjectInfo(List<string> payloads)
        {
            var result = new List<ProjectInfo>();
            foreach (var payload in payloads)
            {
                var packageName = GetPayloadPackageName(payload);
                var frameWork = GetPayloadFrameWork(payload);
                var installedPackages = GetInstalledPackages(payload);
                var projectInfo = new ProjectInfo
                {
                    Name = packageName,
                    TargetFramework = frameWork,
                    InstalledPackages = installedPackages
                };
                SemanticVersionAnalizer.SetSemanticVersionChange(ref projectInfo);
                result.Add(projectInfo);
            }

            return result;
        }

        private static string GetPayloadPackageName(string payload)
        {
            var math = PackageNameIdentifyRegex.Match(payload);
            if (math.Success)
            {
                return math.Value.Replace(ProjectNameIdentify, string.Empty, StringComparison.InvariantCultureIgnoreCase)
                                 .Replace(EscapeIdentify, string.Empty, StringComparison.InvariantCultureIgnoreCase).Trim();
            }
            return string.Empty;
        }

        private static string GetPayloadFrameWork(string payload)
        {
            var match = FrameWorkIdentifyRegex.Match(payload);
            if (match.Success)
            {
                return match.Value.Replace(StartRegionBySquareBrackets, string.Empty, StringComparison.InvariantCultureIgnoreCase)
                                  .Replace(EndRegionBySquareBrackets, string.Empty, StringComparison.InvariantCultureIgnoreCase).Trim();
            }
            return UnrecognizedMessage;
        }

        private static IEnumerable<InstalledPackage> GetInstalledPackages(string payload)
        {
            var matches = PackageVersionInfosRegex.Matches(payload);
            if (matches.Count == 0) return new List<InstalledPackage>();

            var list = new List<InstalledPackage>();
            for (var i = 0; i <= matches.Count - 1; i++)
            {
                var libLine = matches[i].Value;
                var libName = ExtractLibName(libLine);
                var (currentVersion, latestVersion) = ExtractLibVersion(libLine.Replace(libName, string.Empty, StringComparison.InvariantCultureIgnoreCase).Trim());
                list.Add(new InstalledPackage
                {
                    Name = libName,
                    Current = VersionInfo.FromVersionText(currentVersion),
                    Latest = VersionInfo.FromVersionText(latestVersion)
                });
            }
            return list;
        }

        private static string ExtractLibName(string libLine)
        {
            var cleanLibLine = libLine.Substring(StartPositionForLibNameIndex).Trim();
            return cleanLibLine.Substring(StartPositionForCleanedLibNameIndex, cleanLibLine.IndexOf(BlankSpaceSeparator, StringComparison.InvariantCultureIgnoreCase));
        }

        private static (string? currentVersion, string? latestVersion) ExtractLibVersion(string libLine)
        {
            var matches = LibVersionInfoRegex.Matches(libLine);
            if (matches.Count < LibVersionHasNecessaryInformationForExtractVersion) return (default, default);

            return (matches[CurrentVersionIndex].Value, matches[LatestVersionIndex].Value);
        }
    }
}