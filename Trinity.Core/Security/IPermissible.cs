using System;
using System.Diagnostics.Contracts;

namespace Trinity.Core.Security
{
    /// <summary>
    /// An abstract way to manage permissions on an object.
    /// </summary>
    [ContractClass(typeof(PermissibleContracts))]
    public interface IPermissible
    {
        void AddPermission(Permission perm);

        void RemovePermission(Type permType);

        bool HasPermission(Type permType);
    }

    [ContractClassFor(typeof(IPermissible))]
    public abstract class PermissibleContracts : IPermissible
    {
        public void AddPermission(Permission perm)
        {
            Contract.Requires(perm != null);
        }

        public void RemovePermission(Type permType)
        {
            Contract.Requires(permType != null);
        }

        public bool HasPermission(Type permType)
        {
            Contract.Requires(permType != null);

            return false;
        }
    }
}
