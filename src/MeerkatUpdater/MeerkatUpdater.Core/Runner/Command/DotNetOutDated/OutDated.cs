using MeerkatUpdater.Core.Config.Manager;
using MeerkatUpdater.Core.Runner.Command.Common;
using MeerkatUpdater.Core.Runner.Command.DotNet;
using MeerkatUpdater.Core.Runner.Model.PackageInfo;
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

        /// <summary>
        /// Default constructor for DI
        /// </summary>
        /// <param name="dotNetCommand"></param>
        /// <param name="configManager"></param>
        public OutDated(IDotNetCommand dotNetCommand, IConfigManager configManager) =>
            (this.dotNetCommand, this.configManager) = (dotNetCommand, configManager);

        /// <summary>
        /// Executes the dotnet package --outdated command to have the items that need update <br/>
        /// <see href="https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-list-package">Microsoft documentation</see>
        /// </summary>
        /// <returns></returns>
        public List<ProjectInfo>? Execute()
        {
            var solutionPath = this.configManager.GetConfigurations().SolutionPath ?? string.Empty;

            var result = this.dotNetCommand.RunCommand(DotnetCommandConst.ListCommand,
                                                       solutionPath,
                                                       DotnetCommandConst.PackageCommand,
                                                       DotnetCommandConst.OutDatedParam);

            return Scraper.Outdated.TransformOutPutToProjectInfo(result.Output);
        }
    }
}