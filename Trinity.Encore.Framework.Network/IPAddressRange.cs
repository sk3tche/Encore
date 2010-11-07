using System;
using System.Diagnostics.Contracts;
using System.Net;
using System.Net.Sockets;

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
            Contract.Requires(lower.GetAddressBytes().Length == upper.GetAddressBytes().Length);

            Family = lower.AddressFamily;
            LowerBoundary = lower.GetAddressBytes();
            UpperBoundary = upper.GetAddressBytes();

            Contract.Assert(LowerBoundary.Length >= 4);
            Contract.Assert(UpperBoundary.Length >= 4);
        }

        public bool IsInRange(IPAddress address)
        {
            // Some people just have to be like that...
            if (address.AddressFamily != Family || address.GetAddressBytes().Length != LowerBoundary.Length)
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
    }
}
