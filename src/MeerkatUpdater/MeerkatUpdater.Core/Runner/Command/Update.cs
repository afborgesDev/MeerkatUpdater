using MeerkatUpdater.Core.Runner.Model.PackageInfo;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MeerkatUpdater.Core.Runner.Command
{
    public static class Update
    {
        private const int MaxDegreeParallelForPackagesToUpdate = 10;
        private static readonly ParallelOptions parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = MaxDegreeParallelForPackagesToUpdate };

        public static void Execute(string workDirectory, List<ProjectInfo> toUpdateProjectInfo)
        {
        }
    }
}