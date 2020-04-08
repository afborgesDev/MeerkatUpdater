using MeerkatUpdater.Core.Runner.Model.PackageInfo;
using System;
using System.Collections.Generic;

namespace MeerkatUpdater.Core.Runner.Scraper
{
    /// <summary>
    /// Scrap and transform the outdated command result into projectinfo list
    /// </summary>
    public static class Outdated
    {
        /// <summary>
        /// Transform the result of outdated command into a projectinfo list
        /// </summary>
        /// <param name="output"></param>
        /// <returns></returns>
        public static List<ProjectInfo>? TransformOutPutToProjectInfo(string? output)
        {
            if (string.IsNullOrWhiteSpace(output))
                throw new ArgumentNullException(nameof(output));

            var payloads = OutDatedPayload.TransformRawTextToPayload(output);

            if (payloads is null || payloads.Count == 0)
                throw new ArgumentNullException(DefaultMessages.ErrorOnConvertOutDateOutPutIntoPayload);

            return OutPutPayloadToProjectInfo.TransformPayloadsToProjectInfo(payloads);
        }
    }
}