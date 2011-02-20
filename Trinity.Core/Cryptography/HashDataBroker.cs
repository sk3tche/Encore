using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using Trinity.Core.Runtime;

namespace Trinity.Core.Cryptography
{
    [Serializable]
    public sealed class HashDataBroker : IEquatable<HashDataBroker>
    {
        public HashDataBroker(byte[] data)
        {
            Contract.Requires(data != null);

            _rawData = data;
        }

        [ContractInvariantMethod]
        private void Invariant()
        {
            Contract.Invariant(_rawData != null);
        }

        private readonly byte[] _rawData;

        public byte[] GetRawData()
        {
            Contract.Ensures(Contract.Result<byte[]>().Length == Length);

            return (byte[])_rawData.Clone();
        }

        public int Length
        {
            get
            {
                Contract.Ensures(Contract.Result<int>() >= 0);
                Contract.Ensures(Contract.Result<int>() == _rawData.Length);

                return _rawData.Length;
            }
        }

        public static implicit operator HashDataBroker(byte[] data)
        {
            Contract.Requires(data != null);
            Contract.Ensures(!ReferenceEquals(Contract.Result<HashDataBroker>(), null));

            return new HashDataBroker(data);
        }

        public static implicit operator HashDataBroker(BigInteger integer)
        {
            Contract.Requires(integer != null);
            Contract.Ensures(!ReferenceEquals(Contract.Result<HashDataBroker>(), null));

            return new HashDataBroker(integer.GetBytes());
        }

        public static implicit operator HashDataBroker(string str)
        {
            Contract.Requires(str != null);
            Contract.Ensures(!ReferenceEquals(Contract.Result<HashDataBroker>(), null));

            return new HashDataBroker(Encoding.UTF8.GetBytes(str));
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as HashDataBroker);
        }

        public bool Equals(HashDataBroker other)
        {
            if (other == null)
                return false;

            Contract.Assume(other._rawData != null);
            return _rawData.SequenceEqual(other._rawData);
        }

        public override int GetHashCode()
        {
            return unchecked(_rawData.Aggregate(0, (acc, b) => acc + HashCodeUtility.GetHashCode(b)));
        }

        public static bool operator ==(HashDataBroker a, HashDataBroker b)
        {
            var oa = a as object;
            var ob = b as object;

            if (oa == null && ob == null)
                return true;

            if (oa == null || ob == null)
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(HashDataBroker a, HashDataBroker b)
        {
            return !(a == b);
        }
    }
}
