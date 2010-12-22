using System;
using System.Diagnostics.Contracts;

namespace Trinity.Encore.Framework.Core.Time
{
    public static class TimeUtility
    {
        public static readonly DateTime UnixEpochStart = new DateTime(1970, 1, 1, 0, 0, 0, 0);

        public static DateTime UnixTimeToDateTime(long unixTime)
        {
            Contract.Requires(unixTime > 0);

            return UnixEpochStart.AddSeconds(unixTime);
        }

        public static long DateTimeToUnixTime(DateTime timeValue)
        {
            return (long)(timeValue - UnixEpochStart).TotalSeconds;
        }

        /// <summary>
        /// Gets the current Unix time.
        /// </summary>
        public static long GetUnixTime()
        {
            var ts = (DateTime.UtcNow - UnixEpochStart);
            return (long)ts.TotalSeconds;
        }

        /// <summary>
        /// Gets the current Unix time, in milliseconds.
        /// </summary>
        public static long GetUnixTimeMilliseconds()
        {
            var ts = (DateTime.UtcNow - UnixEpochStart);
            return ts.ToMilliseconds();
        }
    }
}
