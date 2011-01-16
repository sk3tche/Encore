using System.Diagnostics.Contracts;
using Trinity.Network.Transmission;

namespace Trinity.Network.Connectivity
{
    [ContractClass(typeof(PacketReceiverContracts))]
    public interface IPacketReceiver
    {
        void Send(OutgoingPacket packet);
    }

    [ContractClassFor(typeof(IPacketReceiver))]
    public abstract class PacketReceiverContracts : IPacketReceiver
    {
        public void Send(OutgoingPacket packet)
        {
            Contract.Requires(packet != null);
        }
    }
}
