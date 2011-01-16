using System;

namespace Trinity.Core.Time
{
    public sealed class Timer
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
    }
}
