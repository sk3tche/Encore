using System.Diagnostics.Contracts;
using Trinity.Encore.Framework.Network.Connectivity;

namespace Trinity.Encore.Framework.Network.Handling
{
    [ContractClass(typeof(PacketHandlerContracts))]
    public interface IPacketPropagator
    {
        int HeaderLength { get; }

        PacketHeader HandleHeader(IClient client, byte[] header);

        void HandlePayload(IClient client, int opCode, byte[] payload, int length);
    }

    [ContractClassFor(typeof(IPacketPropagator))]
    public abstract class PacketHandlerContracts : IPacketPropagator
    {
        public int HeaderLength
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
            Contract.Requires(header.Length == HeaderLength);
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
    }
}
