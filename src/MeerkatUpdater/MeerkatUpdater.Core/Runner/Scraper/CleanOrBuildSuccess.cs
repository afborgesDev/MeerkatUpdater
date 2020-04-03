using System.Text.RegularExpressions;

namespace MeerkatUpdater.Core.Runner.Scraper
{
    /// <summary>
    /// Scraper to output dotnet CLI that can be used for Clean and Build commands
    /// </summary>
    public static class CleanOrBuildSuccess
    {
        private static readonly Regex BuildSucceededRegex = new Regex("[Bb]uild succeeded", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex NoErrorRegex = new Regex(@"0 [Ee]rror\(s\)", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// Determine if the output result is for a succeeed result
        /// </summary>
        /// <param name="output"></param>
        /// <returns></returns>
        public static bool IsSucceed(string? output)
        {
            if (string.IsNullOrEmpty(output)) return false;

            var buildSucceeded = BuildSucceededRegex.Match(output).Success;
            var noError = NoErrorRegex.Match(output).Success;

            return buildSucceeded && noError;
        }
    }
}