using System;
using System.Diagnostics.Contracts;
using Trinity.Network.Connectivity;
using Trinity.Network.Transmission;

namespace Trinity.Network.Handling
{
    [ContractClass(typeof(PacketHandlerContracts))]
    public interface IPacketPropagator
    {
        int IncomingHeaderLength { get; }

        int OutgoingHeaderLength { get; }

        PacketHeader HandleHeader(IClient client, byte[] header);

        void HandlePayload(IClient client, int opCode, byte[] payload, int length);

        void WriteHeader(OutgoingPacket packet, byte[] buffer);
    }

    [ContractClassFor(typeof(IPacketPropagator))]
    public abstract class PacketHandlerContracts : IPacketPropagator
    {
        public int IncomingHeaderLength
        {
            get
            {
                Contract.Ensures(Contract.Result<int>() > 0);

                return 0;
            }
        }

        public int OutgoingHeaderLength
        {
            get
            {
                Contract.Ensures(Contract.Result<int>() > 0);

                return 0;
            }
        }

        public PacketHeader HandleHeader(IClient client, byte[] header)
        {
            Contract.Requires(client != null);
            Contract.Requires(header != null);
            Contract.Requires(header.Length == IncomingHeaderLength);
            Contract.Ensures(Contract.Result<PacketHeader>().Length >= 0);
            Contract.Ensures(Contract.Result<PacketHeader>().OpCode >= 0);

            return default(PacketHeader);
        }

        public void HandlePayload(IClient client, int opCode, byte[] payload, int length)
        {
            Contract.Requires(client != null);
            Contract.Requires(opCode >= 0);
            Contract.Requires(payload != null);
            Contract.Requires(length >= 0);
            Contract.Requires(length <= payload.Length);
        }

        public void WriteHeader(OutgoingPacket packet, byte[] buffer)
        {
            Contract.Requires(packet != null);
            Contract.Requires(buffer != null);
            Contract.Requires(buffer.Length >= OutgoingHeaderLength);
        }
    }
}
