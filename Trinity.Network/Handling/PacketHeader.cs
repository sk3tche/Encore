using System;
using System.Diagnostics.Contracts;
using Trinity.Core.Runtime;

namespace Trinity.Network.Handling
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

        public override bool Equals(object obj)
        {
            return obj is PacketHeader && Equals((PacketHeader)obj);
        }

        public bool Equals(PacketHeader other)
        {
            return other.Length == Length && other.OpCode == OpCode;
        }

        public override int GetHashCode()
        {
            return HashCodeUtility.GetHashCode(Length, OpCode);
        }

        public static bool operator ==(PacketHeader a, PacketHeader b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(PacketHeader a, PacketHeader b)
        {
            return !(a == b);
        }
    }
}
