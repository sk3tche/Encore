using System;

namespace Trinity.Core.Threading.Actors
{
    [Serializable]
    public enum SchedulerType : byte
    {
        SingleThread = 0,
        ThreadPool = 1,
        TaskParallelLibrary = 2,
    }
}
