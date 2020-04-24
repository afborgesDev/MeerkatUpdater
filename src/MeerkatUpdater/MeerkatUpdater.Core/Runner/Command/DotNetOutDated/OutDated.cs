using MeerkatUpdater.Core.Config.Manager;
using MeerkatUpdater.Core.Runner.Command.Common;
using MeerkatUpdater.Core.Runner.Command.DotNet;
using MeerkatUpdater.Core.Runner.Command.DotNetBuild;
using MeerkatUpdater.Core.Runner.Model.PackageInfo;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace MeerkatUpdater.Core.Runner.Command.DotNetOutDated
{
    /// <summary>
    /// Warp the dotnet package --outdated
    /// </summary>
    public class OutDated : IOutDated
    {
        private readonly IDotNetCommand dotNetCommand;
        private readonly IConfigManager configManager;
        private readonly IBuild build;
        private readonly ILogger<OutDated> logger;

        /// <summary>
        /// Default constructor for DI
        /// </summary>
        /// <param name="dotNetCommand"></param>
        /// <param name="build"></param>
        /// <param name="configManager"></param>
        /// <param name="logger"></param>
        public OutDated(IDotNetCommand dotNetCommand, IBuild build, IConfigManager configManager, ILogger<OutDated> logger) =>
            (this.dotNetCommand, this.build, this.configManager, this.logger) = (dotNetCommand, build, configManager, logger);

        /// <summary>
        /// Executes the dotnet package --outdated command to have the items that need update <br/>
        /// <see href="https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-list-package">Microsoft documentation</see>
        /// </summary>
        /// <returns></returns>
        public List<ProjectInfo>? Execute()
        {
            this.build.Execute();
            var solutionPath = this.configManager.GetConfigurations().SolutionPath ?? string.Empty;
            this.logger.LogInformation(DefaultMessages.LOG_OutDateProject);
            var result = this.dotNetCommand.RunCommand(DotnetCommandConst.ListCommand,
                                                       solutionPath,
                                                       DotnetCommandConst.PackageCommand,
                                                       DotnetCommandConst.OutDatedParam);

            return Scraper.Outdated.TransformOutPutToProjectInfo(result.Output);
        }
    }
}