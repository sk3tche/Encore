using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Reflection;
using Trinity.Encore.Framework.Core.Collections;
using Trinity.Encore.Framework.Core.Reflection;
using Trinity.Encore.Framework.Network.Connectivity;
using Trinity.Encore.Framework.Network.Handling;
using Trinity.Encore.Framework.Network.Security;
using Trinity.Encore.Framework.Network.Transmission;

namespace Trinity.Encore.Framework.Game.Network.Handling
{
    public abstract class PacketPropagatorBase<TAttribute, TPacket>
        where TAttribute : PacketHandlerAttribute
        where TPacket : IncomingPacket
    {
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
                foreach (var method in type.GetMethods())
                {
                    Contract.Assume(method != null);

                    var attr = method.GetCustomAttribute<TAttribute>();
                    if (attr == null)
                        continue;

                    if (!method.IsPrivate)
                        throw new ReflectionException("Packet handler methods must be private.");

                    if (!method.IsStatic)
                        throw new ReflectionException("Packet handler methods must be static.");

                    if (method.IsGenericMethodDefinition || method.IsGenericMethod)
                        throw new ReflectionException("Packet handler methods must not be generic.");

                    if (method.ReturnType != typeof(void))
                        throw new ReflectionException("Packet handler methods must not return a value.");

                    var parameters = method.GetParameters();
                    if (parameters.Length != 2)
                        throw new ReflectionException("Packet handler methods must only take 2 arguments.");

                    if (parameters[0].ParameterType != typeof(IClient))
                        throw new ReflectionException("The first parameter on packet handler methods must be of type IClient.");

                    if (parameters[1].ParameterType != typeof(TPacket))
                        throw new ReflectionException("The second parameter on packet handler methods must be of type TPacket.");

                    var opCode = attr.OpCode;
                    var handler = new PacketHandler<TPacket>(opCode, method, attr.Permission ?? typeof(ConnectedPermission));
                    AddHandler(((IConvertible)opCode).ToInt32(null), handler);
                }
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
    }
}
