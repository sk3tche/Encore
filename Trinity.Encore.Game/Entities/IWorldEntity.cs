using System;
using System.Diagnostics.Contracts;
using Mono.GameMath;
using Trinity.Core.Security;
using Trinity.Core.Threading;
using Trinity.Encore.Game.Partitioning;

namespace Trinity.Encore.Game.Entities
{
    [ContractClass(typeof(WorldEntityContracts))]
    public interface IWorldEntity : IEntity
    {
        Vector3 Position { get; }

        QuadTreeNode Node { get; set; }
    }

    [ContractClassFor(typeof(IWorldEntity))]
    public abstract class WorldEntityContracts : IWorldEntity
    {
        public abstract Vector3 Position { get; }

        public QuadTreeNode Node
        {
            get
            {
                Contract.Ensures(Contract.Result<QuadTreeNode>() != null);

                return null;
            }
            set { Contract.Requires(value != null); }
        }

        public abstract void Dispose();

        public abstract bool IsDisposed { get; }

        public abstract void AddPermission(Permission perm);

        public abstract void RemovePermission(Type permType);

        public abstract bool HasPermission(Type permType);

        public abstract void Join();

        public abstract void PostAsync(Action msg);

        public abstract IWaitable PostWait(Action msg);

        public abstract EntityGuid Guid { get; }
    }
}
