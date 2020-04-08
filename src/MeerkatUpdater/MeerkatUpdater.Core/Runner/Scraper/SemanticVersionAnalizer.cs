using MeerkatUpdater.Core.Runner.Model.PackageInfo;
using System;
using System.Text.RegularExpressions;

namespace MeerkatUpdater.Core.Runner.Scraper
{
    /// <summary>
    /// Define the semantic version for each installed package
    /// </summary>
    public static class SemanticVersionAnalizer
    {
        private static readonly Regex HasLetterRegex = new Regex("([a-zA-Z]{1,})", RegexOptions.Compiled);
        private static readonly Regex CleanUpVersionLetter = new Regex("([a-zA-Z-]{1,})", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        /// <summary>
        /// Analyse and add the semantic version for each installed package
        /// </summary>
        /// <param name="projectInfo"></param>
        public static void SetSemanticVersionChange(ref ProjectInfo projectInfo)
        {
            if (projectInfo == null) return;

            foreach (var lib in projectInfo.InstalledPackages)
                lib.SemanticVersionChange = GetSemanticVersion(lib, false);
        }

        private static SemanticVersion GetSemanticVersion(InstalledPackage lib, bool useUnstableVersions)
        {
            const int MajorIndex = 0;
            const int MinorIndex = 1;
            const int PatchIndex = 2;

            var currentVersion = lib?.Current?.Version ?? string.Empty;
            var latestVersion = lib?.Latest?.Version ?? string.Empty;

            if (currentVersion.Equals(latestVersion, StringComparison.InvariantCultureIgnoreCase))
                return SemanticVersion.NoUpdate;

            var splitedCurrentVersion = currentVersion.Split(".");
            var splitedLatestVersion = latestVersion.Split(".");

            if (splitedCurrentVersion[MajorIndex] != splitedLatestVersion[MajorIndex] &&
                LatestVersionIsGreaterThanCurrent(splitedCurrentVersion[MajorIndex], splitedLatestVersion[MajorIndex], useUnstableVersions))
            {
                return SemanticVersion.Major;
            }

            if (splitedCurrentVersion[MinorIndex] != splitedLatestVersion[MinorIndex] &&
                LatestVersionIsGreaterThanCurrent(splitedCurrentVersion[MinorIndex], splitedLatestVersion[MinorIndex], useUnstableVersions))
            {
                return SemanticVersion.Minor;
            }

            if (splitedCurrentVersion[PatchIndex] != splitedLatestVersion[PatchIndex] &&
                LatestVersionIsGreaterThanCurrent(splitedCurrentVersion[PatchIndex], splitedLatestVersion[PatchIndex], useUnstableVersions))
            {
                return SemanticVersion.Path;
            }

            return SemanticVersion.NoUpdate;
        }

        private static bool LatestVersionIsGreaterThanCurrent(string currentVersion, string latestVersion, bool useUnstableVersions)
        {
            if (int.TryParse(currentVersion, out var intCurrentVerstion) && int.TryParse(latestVersion, out var intLatestVerstion))
                return intLatestVerstion > intCurrentVerstion;

            var currentHasLetter = HasLetterRegex.IsMatch(currentVersion);
            var latestHasLetter = HasLetterRegex.IsMatch(latestVersion);

            if (currentHasLetter && !latestHasLetter) return true;

            if (!currentHasLetter && latestHasLetter && useUnstableVersions) return true;

            if (currentHasLetter && latestHasLetter)
            {
                var cleanCurrentVersion = CleanUpVersion(currentVersion);
                var cleanLatestVersion = CleanUpVersion(latestVersion);

                if (double.TryParse(cleanCurrentVersion, out var numberCurrentVersion) &&
                    double.TryParse(cleanLatestVersion, out var numberLatestVersion))
                {
                    return numberLatestVersion > numberCurrentVersion;
                }
            }

            return false;
        }

        private static string CleanUpVersion(string version)
        {
            var clean = CleanUpVersionLetter.Replace(version, string.Empty)
                                            .Trim();
            return clean.Replace("-", string.Empty, StringComparison.InvariantCultureIgnoreCase)
                        .Replace(".", string.Empty, StringComparison.InvariantCultureIgnoreCase)
                        .Trim();
        }
    }
}