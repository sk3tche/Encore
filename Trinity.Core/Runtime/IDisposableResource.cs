using System;

namespace Trinity.Core.Runtime
{
    public interface IDisposableResource : IDisposable
    {
        bool IsDisposed { get; }
    }
}
