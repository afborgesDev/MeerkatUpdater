﻿using MeerkatUpdater.Core.Config.Manager;
using MeerkatUpdater.Core.Runner.Command.Common;
using MeerkatUpdater.Core.Runner.Command.DotNet;
using MeerkatUpdater.Core.Runner.Scraper;
using Microsoft.Extensions.Logging;
using System;

namespace MeerkatUpdater.Core.Runner.Command.DotNetBuild
{
    /// <summary>
    /// Wrap the dotnet build command
    /// </summary>
    public class Build : IBuild
    {
        private readonly IDotNetCommand dotNetCommand;
        private readonly IConfigManager configManager;
        private readonly ILogger<Build> logger;

        /// <summary>
        /// Default constructor for DI
        /// </summary>
        /// <param name="dotNetCommand"></param>
        /// <param name="configManager"></param>
        /// <param name="logger"></param>
        public Build(IDotNetCommand dotNetCommand, IConfigManager configManager, ILogger<Build> logger) =>
            (this.dotNetCommand, this.configManager, this.logger) = (dotNetCommand, configManager, logger);

        /// <summary>
        /// Exceute the dotnet build command <br/>
        /// <see href="https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-build">microsoft documentation</see>
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="NullReferenceException"></exception>
        public bool Execute()
        {
            var solutionPath = this.configManager.GetConfigurations().SolutionPath ?? string.Empty;
            var outputPath = this.configManager.GetConfigurations().GetTargetOutPutPath();
            this.logger.LogInformation(DefaultMessages.LOG_BuildProject);
            var result = this.dotNetCommand.RunCommand(DotnetCommandConst.BuildCommand,
                                                       solutionPath,
                                                       DotnetCommandConst.TargetOutPutParam,
                                                       outputPath);

            return CleanOrBuildSuccess.IsSucceed(result.Output);
        }
    }
}