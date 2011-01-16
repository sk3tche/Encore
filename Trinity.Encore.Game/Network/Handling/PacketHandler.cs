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
        private readonly Action<IClient, TPacket> _invoker;

        public PacketHandler(Enum opCode, MethodInfo method, Type permission)
        {
            Contract.Requires(opCode != null);
            Contract.Requires(method != null);
            Contract.Requires(permission != null);

            OpCode = opCode;
            Method = method;
            Permission = permission;

            // Create a type-safe delegate that can be invoked from packet propagators.
            _invoker = (Action<IClient, TPacket>)Delegate.CreateDelegate(typeof(Action<IClient, TPacket>), method);
            Contract.Assume(_invoker != null); // It'll fail in the earlier call if this isn't true...
        }

        public void Invoke(IClient client, TPacket packet)
        {
            Contract.Requires(client != null);
            Contract.Requires(packet != null);

            _invoker(client, packet);
        }

        [ContractInvariantMethod]
        private void Invariant()
        {
            Contract.Invariant(OpCode != null);
            Contract.Invariant(Method != null);
            Contract.Invariant(Permission != null);
            Contract.Invariant(_invoker != null);
        }

        public Enum OpCode { get; private set; }

        public MethodInfo Method { get; private set; }

        public Type Permission { get; private set; }
    }
}
