using System;
using System.Diagnostics.Contracts;
using System.Threading;
using Trinity.Encore.Framework.Core.Runtime;
using Trinity.Encore.Framework.Core.Security;

namespace Trinity.Encore.Framework.Core.Threading.Actors
{
    [ContractClass(typeof(ActorContracts))]
    public interface IActor : IDisposableResource, IPermissible
    {
        void Join();

        void Post(Action msg);
    }

    [ContractClassFor(typeof(IActor))]
    public abstract class ActorContracts : IActor
    {
        public void Post(Action msg)
        {
            Contract.Requires(msg != null);
        }

        public abstract void Join();

        public abstract void Dispose();

        public abstract bool IsDisposed { get; }

        public abstract void AddPermission(Permission perm);

        public abstract void RemovePermission(Type permType);

        public abstract bool HasPermission(Type permType);
    }
}
