using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace Trinity.Encore.Framework.Core.Runtime
{
    public static class RuntimeExtensions
    {
        public static void ThrowIfDisposed(this IDisposableResource resource)
        {
            Contract.Requires(resource != null);

            if (resource.IsDisposed)
                throw new ObjectDisposedException(resource.ToString(), "An attempt was made to use a disposed object.");
        }

        public static Task DeferDispose(this IDisposable resource, int delayMilliseconds = 0)
        {
            Contract.Requires(resource != null);
            Contract.Requires(delayMilliseconds >= 0);
            Contract.Ensures(Contract.Result<Task>() != null);

            var task = Task.Factory.StartNewDelayed(delayMilliseconds, resource.Dispose);
            Contract.Assume(task != null);
            return task;
        }
    }
}
