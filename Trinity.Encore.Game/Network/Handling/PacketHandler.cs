using System;
using System.Diagnostics.Contracts;
using System.Reflection;
using Trinity.Network.Connectivity;
using Trinity.Network.Transmission;

namespace Trinity.Encore.Game.Network.Handling
{
    public sealed class PacketHandler<TPacket>
        where TPacket : IncomingPacket
    {
        public PacketHandler(Enum opCode, ConstructorInfo constructor, Type permission)
        {
            Contract.Requires(opCode != null);
            Contract.Requires(constructor != null);
            Contract.Requires(permission != null);

            OpCode = opCode;
            Constructor = constructor;
            Permission = permission;
        }

        public void Invoke(IClient client, TPacket packet)
        {
            Contract.Requires(client != null);
            Contract.Requires(packet != null);

            var handler = (PacketHandlerBase<TPacket>)Constructor.Invoke(null);

            // First, perform reading of the packet. Protocol violations are reported through method calls, avoiding
            // exceptions, as they are generally too slow for something like this.
            var success = handler.Read(client, packet);

            if (!success)
            {
                // If we fail to read, we just dispose the packet. The client is disconnected already.
                packet.Dispose();
                return;
            }

            // We're far enough to do actual handling. We assume that the handler has read data into properties on
            // itself, in order to work with it now.
            handler.Handle(client);

            // Everything went smooth. Dispose of the packet.
            packet.Dispose();
        }

        [ContractInvariantMethod]
        private void Invariant()
        {
            Contract.Invariant(OpCode != null);
            Contract.Invariant(Constructor != null);
            Contract.Invariant(Permission != null);
        }

        public Enum OpCode { get; private set; }

        public ConstructorInfo Constructor { get; private set; }

        public Type Permission { get; private set; }
    }
}
