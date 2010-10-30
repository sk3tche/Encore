using System.Diagnostics.Contracts;

namespace Trinity.Encore.Framework.Core.Time
{
    [ContractClass(typeof(TimerHolderContracts))]
    public interface ITimerHolder
    {
        void AddTimer(Timer timer);

        void RemoveTimer(Timer timer);

        void RemoveAllTimers();
    }

    [ContractClassFor(typeof(ITimerHolder))]
    public abstract class TimerHolderContracts : ITimerHolder
    {
        public void AddTimer(Timer timer)
        {
            Contract.Requires(timer != null);
        }

        public void RemoveTimer(Timer timer)
        {
            Contract.Requires(timer != null);
        }

        public abstract void RemoveAllTimers();
    }
}
