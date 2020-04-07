using MeerkatUpdater.Core.Runner.Model.PackageInfo;
using System;
using System.Collections.Generic;

namespace MeerkatUpdater.Core.Runner.Scraper
{
    public static class Outdated
    {
        public static List<ProjectInfo>? TransformOutPutToProjectInfo(string? output)
        {
            if (string.IsNullOrWhiteSpace(output))
                throw new ArgumentNullException(nameof(output));

            var payload = OutDatedPayload.TransformRawTextToPayload(output);

            if (payload is null || payload.Count == 0)
                throw new ArgumentNullException(DefaultMessages.ErrorOnConvertOutDateOutPutIntoPayload);
        }
    }
}