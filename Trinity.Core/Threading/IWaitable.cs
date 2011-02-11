using System;

namespace Trinity.Core.Threading
{
    public interface IWaitable
    {
        bool Wait();

        bool Wait(TimeSpan timeout);

        bool WaitExitContext(TimeSpan timeout);
    }
}
