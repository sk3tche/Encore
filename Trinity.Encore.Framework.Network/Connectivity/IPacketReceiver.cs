using System.Diagnostics.Contracts;
using Trinity.Encore.Framework.Network.Transmission;

namespace Trinity.Encore.Framework.Network.Connectivity
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
