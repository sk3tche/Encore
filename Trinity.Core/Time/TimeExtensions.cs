using System;

namespace Trinity.Core.Time
{
    public static class TimeExtensions
    {
        /// <summary>
        /// Converts a TimeSpan to its equivalent representation in milliseconds (Int64).
        /// </summary>
        /// <param name="span">The time span value to convert.</param>
        public static long ToMilliseconds(this TimeSpan span)
        {
            return (long)span.TotalMilliseconds;
        }
    }
}
