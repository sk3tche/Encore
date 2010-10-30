using System;

namespace Trinity.Encore.Framework.Core.Time
{
    public static class Extensions
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
