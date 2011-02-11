using System;
using System.Diagnostics.Contracts;
using Trinity.Core.Runtime;
using Trinity.Core.Security;

namespace Trinity.Core.Threading.Actors
{
    [ContractClass(typeof(ActorContracts))]
    public interface IActor : IDisposableResource, IPermissible
    {
        void Join();

        void PostAsync(Action msg);

        IWaitable PostWait(Action msg);
    }

    [ContractClass(typeof(ActorContracts<>))]
    public interface IActor<out TThis> : IActor
        where TThis : IActor<TThis>
    {
        void PostAsync(Action<TThis> msg);

        IWaitable PostWait(Action<TThis> msg);
    }

    [ContractClassFor(typeof(IActor))]
    public abstract class ActorContracts : IActor
    {
        public void PostAsync(Action msg)
        {
            Contract.Requires(msg != null);
        }

        public IWaitable PostWait(Action msg)
        {
            Contract.Requires(msg != null);
            Contract.Ensures(Contract.Result<IWaitable>() != null);

            return null;
        }

        public abstract void Join();

        public abstract void Dispose();

        public abstract bool IsDisposed { get; }

        public abstract void AddPermission(Permission perm);

        public abstract void RemovePermission(Type permType);

        public abstract bool HasPermission(Type permType);
    }

    [ContractClassFor(typeof(IActor<>))]
    public abstract class ActorContracts<TThis> : IActor<TThis>
        where TThis : IActor<TThis>
    {
        public void PostAsync(Action<TThis> msg)
        {
            Contract.Requires(msg != null);
        }

        public IWaitable PostWait(Action<TThis> msg)
        {
            Contract.Requires(msg != null);
            Contract.Ensures(Contract.Result<IWaitable>() != null);

            return null;
        }

        public abstract void Dispose();

        public abstract bool IsDisposed { get; }

        public abstract void AddPermission(Permission perm);

        public abstract void RemovePermission(Type permType);

        public abstract bool HasPermission(Type permType);

        public abstract void Join();

        public abstract void PostAsync(Action msg);

        public abstract IWaitable PostWait(Action msg);
    }
}
