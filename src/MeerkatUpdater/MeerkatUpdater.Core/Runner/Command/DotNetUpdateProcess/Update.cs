using MeerkatUpdater.Core.Runner.Model.PackageInfo;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MeerkatUpdater.Core.Runner.Command.DotNetUpdateProcess
{
    public static class Update
    {
        private const int MaxDegreeParallelForPackagesToUpdate = 10;
        private static readonly ParallelOptions parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = MaxDegreeParallelForPackagesToUpdate };

        public static void Execute(List<ProjectInfo> toUpdateProjectInfo)
        {
        }
    }
}