using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace MeerkatUpdater.Core.Test.FeaturesForServices
{
    public struct ExecutionParams
    {
        public string? WorkDirectory { get; set; }
        public TimeSpan? MaximumWaitTime { get; set; }

        public List<string> Arguments { get; set; }
    }

    public static class DotNetCommandTableArgumentsTransformer
    {
        private const string StringEmptyKey = "string.empty";

        public static ExecutionParams TransformTableParamsToTargetExecutionMethod(Table table)
        {
            dynamic tableInstance = table.CreateDynamicInstance();
            var command = string.Empty;
            var workDirectory = string.Empty;
            double waitTimeOut = tableInstance.WaitTimeOut;

            if (tableInstance.Command != StringEmptyKey)
                command = tableInstance.Command;

            if (tableInstance.WorkDir != StringEmptyKey)
                workDirectory = tableInstance.WorkDir;

            var timeOut = TimeSpan.FromSeconds(waitTimeOut);

            return new ExecutionParams
            {
                WorkDirectory = workDirectory,
                MaximumWaitTime = timeOut,
                Arguments = new List<string> { command }
            };
        }
    }
}