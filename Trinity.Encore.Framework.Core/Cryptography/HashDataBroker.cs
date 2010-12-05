using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using Trinity.Encore.Framework.Core.Cryptography.SRP;

namespace Trinity.Encore.Framework.Core.Cryptography
{
    [Serializable]
    public sealed class HashDataBroker : IEquatable<HashDataBroker>
    {
        public HashDataBroker(byte[] data)
        {
            Contract.Requires(data != null);

            RawData = data;
        }

        [ContractInvariantMethod]
        private void Invariant()
        {
            Contract.Invariant(RawData != null);
        }

        public byte[] RawData { get; private set; }

        public int Length
        {
            get
            {
                Contract.Ensures(Contract.Result<int>() >= 0);
                Contract.Ensures(Contract.Result<int>() == RawData.Length);

                return RawData.Length;
            }
        }

        public static implicit operator HashDataBroker(byte[] data)
        {
            Contract.Requires(data != null);
            Contract.Ensures(Contract.Result<HashDataBroker>() != null);

            return new HashDataBroker(data);
        }

        public static implicit operator HashDataBroker(BigInteger integer)
        {
            Contract.Requires(integer != null);
            Contract.Ensures(Contract.Result<HashDataBroker>() != null);

            return new HashDataBroker(integer.GetBytes());
        }

        public static implicit operator HashDataBroker(string str)
        {
            Contract.Requires(str != null);
            Contract.Ensures(Contract.Result<HashDataBroker>() != null);

            return new HashDataBroker(Encoding.UTF8.GetBytes(str));
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as HashDataBroker);
        }

        public bool Equals(HashDataBroker other)
        {
            return other != null && RawData.SequenceEqual(other.RawData);
        }

        public override int GetHashCode()
        {
            return unchecked(RawData.Aggregate(0, (acc, b) => acc + Utilities.GetHashCode(b)));
        }

        public static bool operator ==(HashDataBroker a, HashDataBroker b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(HashDataBroker a, HashDataBroker b)
        {
            return !(a == b);
        }
    }
}
