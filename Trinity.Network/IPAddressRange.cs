using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using Trinity.Core.Runtime;

namespace Trinity.Network
{
    // Code inspired from a StackOverflow post by Richard Szalay.

    [Serializable]
    public sealed class IPAddressRange
    {
        public AddressFamily Family { get; private set; }

        private readonly byte[] _lowerBoundary;

        private readonly byte[] _upperBoundary;

        public IPAddress LowerBoundary { get; private set; }

        public IPAddress UpperBoundary { get; private set; }

        [ContractInvariantMethod]
        private void Invariant()
        {
            Contract.Invariant(_lowerBoundary != null);
            Contract.Invariant(_lowerBoundary.Length >= 4);
            Contract.Invariant(_upperBoundary != null);
            Contract.Invariant(_upperBoundary.Length >= 4);
            Contract.Invariant(_lowerBoundary.Length == _upperBoundary.Length);
            Contract.Invariant(LowerBoundary != null);
            Contract.Invariant(UpperBoundary != null);
            Contract.Invariant(LowerBoundary.GetLength() == UpperBoundary.GetLength());
            Contract.Invariant(LowerBoundary.AddressFamily == UpperBoundary.AddressFamily);
        }

        public IPAddressRange(IPAddress lower, IPAddress upper)
        {
            Contract.Requires(lower.AddressFamily == upper.AddressFamily);
            Contract.Requires(lower.GetLength() == upper.GetLength());

            Family = lower.AddressFamily;
            _lowerBoundary = lower.GetAddressBytes();
            _upperBoundary = upper.GetAddressBytes();
            LowerBoundary = lower;
            UpperBoundary = upper;

            Contract.Assume(_lowerBoundary.Length >= 4);
            Contract.Assert(_upperBoundary.Length >= 4);
        }

        public bool IsInRange(IPAddress address)
        {
            // Some people just have to be like that...
            if (address.AddressFamily != Family || address.GetLength() != _lowerBoundary.Length)
                return false;

            var bytes = address.GetAddressBytes();

            var lowerBoundary = true;
            var upperBoundary = true;

            for (var i = 0; i < _lowerBoundary.Length && (lowerBoundary || upperBoundary); i++)
            {
                var currentByte = bytes[i];
                var lowerByte = _lowerBoundary[i];
                var upperByte = _upperBoundary[i];

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

            Contract.Assume(other._lowerBoundary != null);
            Contract.Assume(other._upperBoundary != null);
            return other.Family == Family && other._lowerBoundary.SequenceEqual(_lowerBoundary) && other._upperBoundary.SequenceEqual(_upperBoundary);
        }

        public override int GetHashCode()
        {
            return unchecked(Family.GetHashCode() + _upperBoundary.Aggregate(0, (acc, b) => acc + HashCodeUtility.GetHashCode(b)) +
                _lowerBoundary.Aggregate(0, (acc, b) => acc + HashCodeUtility.GetHashCode(b)));
        }
    }
}
