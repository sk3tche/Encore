using System;
using System.Diagnostics.Contracts;
using System.Net;
using Trinity.Core.Security;
using Trinity.Core.Threading.Actors;
using Trinity.Network.Encryption;
using Trinity.Network.Transmission;

namespace Trinity.Network.Connectivity
{
    [ContractClass(typeof(ClientContracts))]
    public interface IClient : IPacketReceiver, IActor
    {
        void Disconnect();

        IPacketCrypt Crypt { get; set; }

        EndPoint EndPoint { get; }

        dynamic UserData { get; }
    }

    [ContractClassFor(typeof(IClient))]
    public abstract class ClientContracts : IClient
    {
        public abstract void Disconnect();

        public abstract IPacketCrypt Crypt { get; set; }

        public abstract EndPoint EndPoint { get; }

        public dynamic UserData
        {
            get
            {
                Contract.Ensures(Contract.Result<object>() != null);

                return null;
            }
        }

        public abstract void Send(OutgoingPacket packet);

        public abstract void AddPermission(Permission perm);

        public abstract void RemovePermission(Type permType);

        public abstract bool HasPermission(Type permType);

        public abstract void Dispose();

        public abstract bool IsDisposed { get; }

        public abstract void Join();

        public abstract void Post(Action msg);
    }
}
