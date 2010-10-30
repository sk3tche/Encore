using System;
using System.Diagnostics.Contracts;

namespace Trinity.Encore.Framework.Core.Time
{
    public sealed class Timer : IComparable<Timer>
    {
        public Timer()
        {
            // Start active.
            Active = true;
        }

        public void Update(TimeSpan diff)
        {
            if (!Active)
                return;

            if (_time >= IntervalMilliseconds)
            {
                // Fire ze event!
                var evt = Event;
                if (evt != null)
                    evt();

                _time = 0;
            }

            _time += diff.ToMilliseconds();
        }

        private long _time;

        /// <summary>
        /// The event that is triggered when the timer ticks.
        /// </summary>
        public Action Event { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates whether or not the timer should
        /// perform updates and trigger the bound event.
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Interval between firing the bound event.
        /// </summary>
        public long IntervalMilliseconds { get; set; }

        public int CompareTo(Timer other)
        {
            if (other == null || this > other)
                return 1;

            if (this < other)
                return -1;

            return 0;
        }

        public static bool operator >(Timer a, Timer b)
        {
            Contract.Requires(a != null);
            Contract.Requires(b != null);

            return a.IntervalMilliseconds > b.IntervalMilliseconds;
        }

        public static bool operator <(Timer a, Timer b)
        {
            Contract.Requires(a != null);
            Contract.Requires(b != null);

            return a.IntervalMilliseconds < b.IntervalMilliseconds;
        }
    }
}
