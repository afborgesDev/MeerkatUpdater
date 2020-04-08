using MeerkatUpdater.Core.Runner.Scraper.Common;
using System;
using System.Collections.Generic;
using System.IO;

namespace MeerkatUpdater.Core.Runner.Scraper
{
    /// <summary>
    /// Scrap the project list to extract the paths
    /// </summary>
    public static class ProjectPathUpdater
    {
        /// <summary>
        /// Scrap the project list to extract the paths
        /// </summary>
        /// <param name="output"></param>
        /// <returns></returns>
        public static Dictionary<string, string>? DiscoverProjectPath(string? output)
        {
            if (string.IsNullOrWhiteSpace(output))
                throw new ArgumentNullException(nameof(output));

            var cleanOutPut = SharedRegex.CleanUpUsingCountProjectRegex(output);

            if (string.IsNullOrEmpty(cleanOutPut))
                return new Dictionary<string, string>();

            var returnList = new Dictionary<string, string>();
            foreach (var projectLine in cleanOutPut.Split(Environment.NewLine))
            {
                if (string.IsNullOrEmpty(projectLine))
                    continue;

                returnList.Add(Path.GetFileNameWithoutExtension(projectLine), projectLine);
            }

            return returnList;
        }
    }
}