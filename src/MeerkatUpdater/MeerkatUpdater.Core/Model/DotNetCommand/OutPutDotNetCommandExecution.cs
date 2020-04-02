using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MeerkatUpdater.Core.Model.DotNetCommand
{
    /// <summary>
    /// Class used to look at execution process on CLI
    /// </summary>
    public sealed class OutPutDotNetCommandExecution
    {
        /// <summary>
        /// All lines from StandardOutput
        /// </summary>
        public StringBuilder? OutPut { get; set; }

        /// <summary>
        /// All lines from StandardErrorOutput
        /// </summary>
        public Task? OutPutTask { get; set; }

        /// <summary>
        /// Wraps the class and creates by using a separated task to read the output
        /// </summary>
        /// <param name="streamReader"></param>
        /// <returns></returns>
        public static OutPutDotNetCommandExecution FromStreamReader(StreamReader streamReader)
        {
            if (streamReader is null)
                throw new ArgumentNullException(nameof(streamReader));

            var output = new OutPutDotNetCommandExecution() { OutPut = new StringBuilder() };
            output.OutPutTask = output.ConsumeStreamReaderAsync(streamReader);
            return output;
        }

        /// <summary>
        /// Safe return the string from the output
        /// </summary>
        /// <returns></returns>
        public string GetOutPutString() => OutPut?.ToString() ?? string.Empty;

        private async Task ConsumeStreamReaderAsync(StreamReader reader)
        {
            await Task.Yield();
            string? line;
            while ((line = await reader.ReadLineAsync().ConfigureAwait(false)) != null)
                OutPut?.AppendLine(line);
        }
    }
}