using System;
using System.Collections.Concurrent;
using System.Diagnostics.Contracts;
using Trinity.Encore.Framework.Core.Collections;

namespace Trinity.Encore.Framework.Core.Security
{
    /// <summary>
    /// A default implementation of IPermissible.
    /// </summary>
    public abstract class RestrictedObject : IPermissible
    {
        private readonly ConcurrentDictionary<Type, Permission> _permissions = new ConcurrentDictionary<Type, Permission>();

        [ContractInvariantMethod]
        private void Invariant()
        {
            Contract.Invariant(_permissions != null);
        }

        public void AddPermission(Permission perm)
        {
            _permissions.Add(perm.GetType(), perm);
        }

        public void RemovePermission(Type permType)
        {
            _permissions.Remove(permType);
        }

        public bool HasPermission(Type permType)
        {
            return _permissions.TryGet(permType) != null;
        }
    }
}
