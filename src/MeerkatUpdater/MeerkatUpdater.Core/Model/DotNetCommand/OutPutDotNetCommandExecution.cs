using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MeerkatUpdater.Core.Model.DotNetCommand
{
    internal sealed class OutPutDotNetCommandExecution
    {
        internal StringBuilder? OutPut { get; set; }
        internal Task? OutPutTask { get; set; }

        internal static OutPutDotNetCommandExecution FromStreamReader(StreamReader streamReader)
        {
            var output = new OutPutDotNetCommandExecution() { OutPut = new StringBuilder() };
            output.OutPutTask = output.ConsumeStreamReaderAsync(streamReader);
            return output;
        }

        internal async Task ConsumeStreamReaderAsync(StreamReader reader)
        {
            if (reader is null)
                throw new System.ArgumentNullException(nameof(reader));

            await Task.Yield();
            string? line;
            while ((line = await reader.ReadLineAsync().ConfigureAwait(false)) != null)
                OutPut?.AppendLine(line);
        }

        internal string GetOutPutString() => OutPut?.ToString() ?? string.Empty;
    }
}