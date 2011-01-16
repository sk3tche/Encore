using System;
using System.Collections.Concurrent;
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
    [ContractClass(typeof(PacketPropagatorBaseContracts<,>))]
    public abstract class PacketPropagatorBase<TAttribute, TPacket> : IPacketPropagator
        where TAttribute : PacketHandlerAttribute
        where TPacket : IncomingPacket
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

            foreach (var method in asm.GetTypes().SelectMany(type => type.GetMethods()))
            {
                Contract.Assume(method != null);

                var attr = method.GetCustomAttribute<TAttribute>();
                if (attr == null)
                    continue;

                if (!method.IsPrivate)
                    throw new ReflectionException("Packet handler methods must be private.");

                if (!method.IsStatic)
                    throw new ReflectionException("Packet handler methods must be static.");

                if (method.IsGenericMethod)
                    throw new ReflectionException("Packet handler methods must not be generic.");

                if (method.ReturnType != typeof(void))
                    throw new ReflectionException("Packet handler methods must not return a value.");

                var parameters = method.GetParameters();
                if (parameters.Length != 2)
                    throw new ReflectionException("Packet handler methods must only take 2 arguments.");

                if (parameters[0].ParameterType != typeof(IClient))
                    throw new ReflectionException("The first parameter on packet handler methods must be of type {0}.".Interpolate(typeof(IClient)));

                if (parameters[1].ParameterType != typeof(TPacket))
                    throw new ReflectionException("The second parameter on packet handler methods must be of type {0}.".Interpolate(typeof(TPacket)));

                var opCode = attr.OpCode;
                var handler = new PacketHandler<TPacket>(opCode, method, attr.Permission ?? typeof(ConnectedPermission));
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
            PacketHandler<TPacket> handler;
            _handlers.TryGetValue(opCode, out handler);
            return handler; // Can be null.
        }

        protected abstract TPacket CreatePacket(int opCode, byte[] payload, int length);

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

            var packet = CreatePacket(opCode, payload, length);
            client.Post(() => handler.Invoke(client, packet));
        }

        public abstract int HeaderLength { get; }

        public abstract PacketHeader HandleHeader(IClient client, byte[] header);
    }

    [ContractClassFor(typeof(PacketPropagatorBase<,>))]
    public abstract class PacketPropagatorBaseContracts<TAttribute, TPacket> : PacketPropagatorBase<TAttribute, TPacket>
        where TAttribute : PacketHandlerAttribute
        where TPacket : IncomingPacket
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
