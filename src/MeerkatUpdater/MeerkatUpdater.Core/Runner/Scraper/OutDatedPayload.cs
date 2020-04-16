using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MeerkatUpdater.Core.Runner.Scraper
{
    internal static class OutDatedPayload
    {
        private const int PackageInfoStartAtIndex = 0;
        private const int PackageInfoEndAtIndex = 1;

        private const int HasSinglePackageToExtract = 1;

        private static readonly Regex PackageIdentifyRegex = new Regex("((Project ['`])([A-Za-z.]{1,}['`]) has the following updates to its packages)",
                                                                 RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private static readonly Regex AllUpdatedRegex = new Regex("has no updates given the current sources", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        internal static List<string>? TransformRawTextToPayload(string rawOutPut)
        {
            if (string.IsNullOrWhiteSpace(rawOutPut))
                throw new ArgumentNullException(nameof(rawOutPut));

            var noHeaderOutPut = CleanUpSourceHeader(rawOutPut);
            var allIsUpdated = AllUpdatedRegex.Matches(noHeaderOutPut).Count > 0;
            if (allIsUpdated) return default;

            var listResult = new List<string>();

            var match = PackageIdentifyRegex.Matches(noHeaderOutPut);
            if (match.Count == HasSinglePackageToExtract)
                listResult.Add(noHeaderOutPut);
            else
                listResult.AddRange(ExtractManyPackageInfo(noHeaderOutPut));

            return listResult;
        }

        private static IEnumerable<string> ExtractManyPackageInfo(string noHeaderOutPut)
        {
            while (!string.IsNullOrEmpty(noHeaderOutPut))
            {
                var match = PackageIdentifyRegex.Matches(noHeaderOutPut);
                if (match.Count <= HasSinglePackageToExtract)
                {
                    yield return noHeaderOutPut;
                    break;
                }

                var startAt = match[PackageInfoStartAtIndex].Index;
                var endAt = match[PackageInfoEndAtIndex].Index;

                yield return noHeaderOutPut.Substring(startAt, endAt).Trim();
                noHeaderOutPut = noHeaderOutPut.Substring(endAt);
            }
        }

        private static string CleanUpSourceHeader(string rawOutPut)
        {
            var match = PackageIdentifyRegex.Match(rawOutPut);
            if (match.Success)
                return rawOutPut.Substring(match.Index).Trim();
            return rawOutPut;
        }
    }
}