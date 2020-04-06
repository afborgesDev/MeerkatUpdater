using System.Text.RegularExpressions;

namespace MeerkatUpdater.Core.Runner.Scraper.Common
{
    /// <summary>
    /// The shared regex for the outdated project
    /// </summary>
    public static class SharedRegex
    {
        /// <summary>
        /// Removes the header from the command list to have only the body
        /// </summary>
        public static readonly Regex CleanUpCountProjectsRegex = new Regex(@"([Pp]roject\(s\))(\r\n|\n)( {1,})?(-){1,}",
                                                                     RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);

        /// <summary>
        /// Replaces the match for a empty string and trim the result.
        /// </summary>
        /// <param name="output"></param>
        /// <returns></returns>
        public static string? CleanUpUsingCountProjectRegex(string? output)
        {
            if (string.IsNullOrWhiteSpace(output))
                return output;

            return CleanUpCountProjectsRegex.Replace(output, string.Empty).Trim();
        }
    }
}