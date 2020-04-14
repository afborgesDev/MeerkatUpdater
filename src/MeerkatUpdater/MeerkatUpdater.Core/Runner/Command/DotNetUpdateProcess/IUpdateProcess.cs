using System.Threading.Tasks;

namespace MeerkatUpdater.Core.Runner.Command.DotNetUpdateProcess
{
    /// <summary>
    /// Centralize the outdate work flow to <br/>
    /// - Count <br/>
    /// - Set Configurations <br/>
    /// - Update <br/>
    /// </summary>
    public interface IUpdateProcess
    {
        /// <summary>
        /// Do the OutDated process by checking and trying to update
        /// </summary>
        /// <returns></returns>
        Task Execute();
    }
}