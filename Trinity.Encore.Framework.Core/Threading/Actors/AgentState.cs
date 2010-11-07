using System;

namespace Trinity.Encore.Framework.Core.Threading.Actors
{
    [Serializable]
    internal enum AgentState : byte
    {
        Running,
        Stopped
    }
}
