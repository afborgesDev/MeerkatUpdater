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

        /// <summary>
        /// Default constructor for DI
        /// </summary>
        /// <param name="dotNetCommand"></param>
        public OutDated(IDotNetCommand dotNetCommand) => this.dotNetCommand = dotNetCommand;

        /// <summary>
        /// Executes the dotnet package --outdated command to have the items that need update <br/>
        /// <see href="https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-list-package">Microsoft documentation</see>
        /// </summary>
        /// <returns></returns>
        public List<ProjectInfo>? Execute()
        {
            var result = this.dotNetCommand.RunCommand(DotnetCommandConst.PackageCommand, DotnetCommandConst.OutDatedParam);
            return Scraper.Outdated.TransformOutPutToProjectInfo(result.Output);
        }
    }
}