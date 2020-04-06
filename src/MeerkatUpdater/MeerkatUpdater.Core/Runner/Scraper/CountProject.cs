using MeerkatUpdater.Core.Runner.Scraper.Common;
using System;

namespace MeerkatUpdater.Core.Runner.Scraper
{
    /// <summary>
    /// Scrap the result of a sln list to have the number of projects into a solution
    /// </summary>
    public static class CountProject
    {
        /// <summary>
        /// Clean the output and than count the number of projects inside a solution
        /// </summary>
        /// <param name="output"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static int? Execute(string? output)
        {
            if (string.IsNullOrWhiteSpace(output))
                throw new ArgumentNullException(nameof(output));

            var cleanOutput = SharedRegex.CleanUpUsingCountProjectRegex(output);

            if (string.IsNullOrWhiteSpace(cleanOutput))
                return null;

            return CountItemsOnOutput(cleanOutput);
        }

        private static int? CountItemsOnOutput(string? cleanOutput) => cleanOutput?.Split(Environment.NewLine).Length;
    }
}