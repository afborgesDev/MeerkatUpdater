using System;

namespace MeerkatUpdater.Config.Test.GeneralUse
{
    public static class StringUtils
    {
        private const string StringEmptyIdentify = "string.empty";
        private const string NullIdentify = "null";

        public static string TableStringPareser(string originalValue)
        {
            if (string.IsNullOrWhiteSpace(originalValue))
                return originalValue;

            if (originalValue.Equals(StringEmptyIdentify, StringComparison.InvariantCultureIgnoreCase))
                return string.Empty;

            if (originalValue.Equals(NullIdentify, StringComparison.InvariantCultureIgnoreCase))
                return default;

            return originalValue;
        }
    }
}