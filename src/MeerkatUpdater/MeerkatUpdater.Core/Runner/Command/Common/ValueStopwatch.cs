using System;
using System.Diagnostics;
using System.Globalization;

namespace MeerkatUpdater.Core.Runner.Command.Common
{
    /// <summary>
    /// [Inspiration code](https://github.com/dotnet/extensions/blob/master/src/Shared/src/ValueStopwatch/ValueStopwatch.cs)
    /// </summary>
    internal struct ValueStopwatch
    {
        private static readonly double _timestampToTicks = TimeSpan.TicksPerSecond / (double)Stopwatch.Frequency;

        private readonly long _startTimestamp;

        private ValueStopwatch(long startTimestamp) => this._startTimestamp = startTimestamp;

        public TimeSpan Elapsed
        {
            get
            {
                if (_startTimestamp == 0)
                    throw new InvalidOperationException(DefaultMessages.InvalidOperationForValueStopwatch);

                var end = Stopwatch.GetTimestamp();
                var timestampDelta = end - _startTimestamp;
                var ticks = (long)(_timestampToTicks * timestampDelta);
                return new TimeSpan(ticks);
            }
        }

        public static ValueStopwatch StartNew() => new ValueStopwatch(Stopwatch.GetTimestamp());

        public string GetStringFullTimeFormated() => Elapsed.ToString("hh:mm:ss.fff", CultureInfo.InvariantCulture);
    }
}