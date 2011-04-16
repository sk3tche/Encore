using System;
using System.Diagnostics.Contracts;
using System.Text;
using Trinity.Core.Security;

namespace Trinity.Encore.Game.Commands
{
    [ContractClass(typeof(CommandUserContracts))]
    public interface ICommandUser : IPermissible
    {
        void Respond(string response);
    }

    [ContractClassFor(typeof(ICommandUser))]
    public abstract class CommandUserContracts : ICommandUser
    {
        public void Respond(string response)
        {
            Contract.Requires(response != null);
        }

        public abstract void AddPermission(Permission perm);

        public abstract void RemovePermission(Type permType);

        public abstract bool HasPermission(Type permType);
    }
}
