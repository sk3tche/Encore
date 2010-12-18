using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using Trinity.Encore.Framework.Core;
using Trinity.Encore.Framework.Core.Runtime;

namespace Trinity.Encore.Framework.Network
{
    // Code inspired from a StackOverflow post by Richard Szalay.
    [Serializable]
    public sealed class IPAddressRange
    {
        public AddressFamily Family { get; private set; }

        public byte[] LowerBoundary { get; private set; }

        public byte[] UpperBoundary { get; private set; }

        [ContractInvariantMethod]
        private void Invariant()
        {
            Contract.Invariant(LowerBoundary != null);
            Contract.Invariant(LowerBoundary.Length >= 4);
            Contract.Invariant(UpperBoundary != null);
            Contract.Invariant(UpperBoundary.Length >= 4);
            Contract.Invariant(LowerBoundary.Length == UpperBoundary.Length);
        }

        public IPAddressRange(IPAddress lower, IPAddress upper)
        {
            Contract.Requires(lower.AddressFamily == upper.AddressFamily);
            Contract.Requires(lower.GetLength() == upper.GetLength());

            Family = lower.AddressFamily;
            LowerBoundary = lower.GetAddressBytes();
            UpperBoundary = upper.GetAddressBytes();

            Contract.Assert(LowerBoundary.Length >= 4);
            Contract.Assert(UpperBoundary.Length >= 4);
        }

        public bool IsInRange(IPAddress address)
        {
            // Some people just have to be like that...
            if (address.AddressFamily != Family || address.GetLength() != LowerBoundary.Length)
                return false;

            var bytes = address.GetAddressBytes();

            var lowerBoundary = true;
            var upperBoundary = true;

            for (var i = 0; i < LowerBoundary.Length && (lowerBoundary || upperBoundary); i++)
            {
                var currentByte = bytes[i];
                var lowerByte = LowerBoundary[i];
                var upperByte = UpperBoundary[i];

                if ((lowerBoundary && currentByte < lowerByte) || (upperBoundary && currentByte > upperByte))
                    return false;

                lowerBoundary &= currentByte == lowerByte;
                upperBoundary &= currentByte == upperByte;
            }

            return true;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as IPAddressRange);
        }

        public bool Equals(IPAddressRange other)
        {
            if (other == null)
                return false;

            return other.Family == Family && other.LowerBoundary.SequenceEqual(LowerBoundary) && other.UpperBoundary.SequenceEqual(UpperBoundary);
        }

        public override int GetHashCode()
        {
            return unchecked(Family.GetHashCode() + UpperBoundary.Aggregate(0, (acc, b) => acc + HashCodeUtility.GetHashCode(b)) +
                LowerBoundary.Aggregate(0, (acc, b) => acc + HashCodeUtility.GetHashCode(b)));
        }
    }
}
