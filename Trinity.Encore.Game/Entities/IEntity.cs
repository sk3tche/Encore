using System;
using System.Diagnostics.Contracts;
using Trinity.Core.Security;
using Trinity.Core.Threading;
using Trinity.Core.Threading.Actors;

namespace Trinity.Encore.Game.Entities
{
    [ContractClass(typeof(EntityContracts))]
    public interface IEntity : IActor
    {
        EntityGuid Guid { get; }
    }

    [ContractClassFor(typeof(IEntity))]
    public abstract class EntityContracts : IEntity
    {
        public EntityGuid Guid
        {
            get
            {
                Contract.Ensures(Contract.Result<EntityGuid>() != EntityGuid.Zero);

                return default(EntityGuid);
            }
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
