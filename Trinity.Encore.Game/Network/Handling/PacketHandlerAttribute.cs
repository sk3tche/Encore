using System;
using System.Diagnostics.Contracts;
using JetBrains.Annotations;

namespace Trinity.Encore.Game.Network.Handling
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    [MeansImplicitUse]
    public abstract class PacketHandlerAttribute : Attribute
    {
        [ContractInvariantMethod]
        private void Invariant()
        {
            Contract.Invariant(OpCode != null);
        }

        protected PacketHandlerAttribute(Enum opCode)
        {
            Contract.Requires(opCode != null);

            OpCode = opCode;
        }

        public Enum OpCode { get; private set; }

        public Type Permission { get; set; }
    }
}
