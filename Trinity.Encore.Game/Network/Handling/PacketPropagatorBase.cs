using System;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Trinity.Core;
using Trinity.Core.Collections;
using Trinity.Core.Logging;
using Trinity.Core.Reflection;
using Trinity.Network.Connectivity;
using Trinity.Network.Handling;
using Trinity.Network.Security;
using Trinity.Network.Transmission;

namespace Trinity.Encore.Game.Network.Handling
{
    [SuppressMessage("Microsoft.Design", "CA1005", Justification = "All type parameters are needed.")]
    [ContractClass(typeof(PacketPropagatorBaseContracts<,,>))]
    public abstract class PacketPropagatorBase<TAttribute, TPacket, THandler> : IPacketPropagator
        where TAttribute : PacketHandlerAttribute
        where TPacket : IncomingPacket
        where THandler : PacketHandlerBase<TPacket>
    {
        private static readonly LogProxy _log = new LogProxy("PacketPropagatorBase");

        private readonly ConcurrentDictionary<int, PacketHandler<TPacket>> _handlers =
            new ConcurrentDictionary<int, PacketHandler<TPacket>>();

        [ContractInvariantMethod]
        private void Invariant()
        {
            Contract.Invariant(_handlers != null);
        }

        protected PacketPropagatorBase()
        {
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                Contract.Assume(asm != null);
                AddHandlers(asm);
            }
        }

        public void AddHandlers(Assembly asm)
        {
            Contract.Requires(asm != null);

            foreach (var type in asm.GetTypes())
            {
                Contract.Assume(type != null);

                var attr = type.GetCustomAttribute<TAttribute>();
                if (attr == null)
                    continue;

                var handlerType = typeof(PacketHandlerBase<TPacket>);
                if (!type.IsAssignableTo(handlerType))
                    throw new ReflectionException("Packet handler classes must inherited {0}.".Interpolate(handlerType));

                if (type.IsGenericTypeDefinition)
                    throw new ReflectionException("Packet handler classes must not be generic.");

                var ctor = type.GetConstructor(Type.EmptyTypes);
                if (ctor == null)
                    throw new ReflectionException("Packet handler classes must have a public parameterless constructor.");

                var opCode = attr.OpCode;
                var handler = new PacketHandler<TPacket>(opCode, ctor, attr.Permission ?? typeof(ConnectedPermission));
                AddHandler(((IConvertible)opCode).ToInt32(null), handler);
            }
        }

        public void AddHandler(int opCode, PacketHandler<TPacket> handler)
        {
            Contract.Requires(handler != null);

            _handlers.Add(opCode, handler);
        }

        public void RemoveHandler(int opCode)
        {
            _handlers.Remove(opCode);
        }

        public PacketHandler<TPacket> GetHandler(int opCode)
        {
            return _handlers.TryGet(opCode);
        }

        protected abstract TPacket CreatePacket(int opCode, byte[] payload, int length);

        public abstract int IncomingHeaderLength { get; }

        public abstract PacketHeader HandleHeader(IClient client, byte[] header);

        public void HandlePayload(IClient client, int opCode, byte[] payload, int length)
        {
            var handler = GetHandler(opCode);
            if (handler == null)
            {
                client.Disconnect();
                _log.Warn("Client {0} sent an unhandled opcode {1} - disconnected.", client, opCode.ToString("X8", CultureInfo.InvariantCulture));
                return;
            }

            var permission = handler.Permission;
            Contract.Assume(permission != null);

            if (!client.HasPermission(permission))
            {
                client.Disconnect();
                _log.Warn("Client {0} sent opcode {1} which requires permission {2} - disconnected.", client,
                    opCode.ToString("X8", CultureInfo.InvariantCulture), permission.Name);
                return;
            }

            // Invoke the packet handler. Exceptions are caught in the client actor's context.
            var packet = CreatePacket(opCode, payload, length);
            client.PostAsync(() => handler.Invoke(client, packet));
        }

        public abstract void WriteHeader(OutgoingPacket packet, byte[] buffer);
    }

    [ContractClassFor(typeof(PacketPropagatorBase<,,>))]
    public abstract class PacketPropagatorBaseContracts<TAttribute, TPacket, THandler> : PacketPropagatorBase<TAttribute, TPacket, THandler>
        where TAttribute : PacketHandlerAttribute
        where TPacket : IncomingPacket
        where THandler : PacketHandlerBase<TPacket>
    {
        protected override TPacket CreatePacket(int opCode, byte[] payload, int length)
        {
            Contract.Requires(opCode >= 0);
            Contract.Requires(payload != null);
            Contract.Requires(length >= 0);
            Contract.Requires(length <= payload.Length);
            Contract.Ensures(Contract.Result<TPacket>() != null);

            return null;
        }
    }
}
