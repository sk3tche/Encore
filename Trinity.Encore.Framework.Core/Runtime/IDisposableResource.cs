using System;

namespace Trinity.Encore.Framework.Core.Runtime
{
    public interface IDisposableResource : IDisposable
    {
        bool IsDisposed { get; }
    }
}
