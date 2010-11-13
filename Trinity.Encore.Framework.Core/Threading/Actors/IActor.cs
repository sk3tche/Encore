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
        TargetPort<Action> IncomingMessages { get; }

        SourcePort<Action> OutgoingMessages { get; }

        CancellationToken CancellationToken { get; }

        CancellationTokenSource CancellationTokenSource { get; }

        IDisposable LinkTo(Actor other, bool unlinkAfterOneMsg = false);
    }

    [ContractClassFor(typeof(IActor))]
    public abstract class ActorContracts : IActor
    {
        public abstract void Dispose();

        public abstract bool IsDisposed { get; }

        public TargetPort<Action> IncomingMessages
        {
            get
            {
                Contract.Ensures(Contract.Result<TargetPort<Action>>() != null);

                return null;
            }
        }

        public SourcePort<Action> OutgoingMessages
        {
            get
            {
                Contract.Ensures(Contract.Result<SourcePort<Action>>() != null);

                return null;
            }
        }

        public abstract CancellationToken CancellationToken { get; }

        public CancellationTokenSource CancellationTokenSource
        {
            get
            {
                Contract.Ensures(Contract.Result<CancellationTokenSource>() != null);

                return null;
            }
        }

        public IDisposable LinkTo(Actor other, bool unlinkAfterOneMsg)
        {
            Contract.Requires(other != null);
            Contract.Ensures(Contract.Result<IDisposable>() != null);

            return null;
        }

        public abstract void AddPermission(Permission perm);

        public abstract void RemovePermission(Type permType);

        public abstract bool HasPermission(Type permType);
    }
}
