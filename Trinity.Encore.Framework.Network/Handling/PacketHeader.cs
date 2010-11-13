using System;
using System.Diagnostics.Contracts;

namespace Trinity.Encore.Framework.Network.Handling
{
    [Serializable]
    public struct PacketHeader
    {
        public int Length;

        public int OpCode;

        [ContractInvariantMethod]
        private void Invariant()
        {
            Contract.Invariant(Length >= 0);
            Contract.Invariant(OpCode >= 0);
        }

        public PacketHeader(int length, int opCode)
        {
            Contract.Requires(length >= 0);
            Contract.Requires(opCode >= 0);
            Contract.Ensures(Contract.ValueAtReturn(out this).Length == length);
            Contract.Ensures(Contract.ValueAtReturn(out this).OpCode == opCode);

            Length = length;
            OpCode = opCode;
        }
    }
}
