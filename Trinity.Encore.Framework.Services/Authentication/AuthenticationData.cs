using System.Diagnostics.Contracts;
using System.Runtime.Serialization;
using Trinity.Encore.Framework.Core.Cryptography;

namespace Trinity.Encore.Framework.Services.Authentication
{
    [DataContract(IsReference = true)]
    public sealed class AuthenticationData
    {
        [ContractInvariantMethod]
        private void Invariant()
        {
            Contract.Invariant(SessionKey != null);
            Contract.Invariant(Salt != null);
            Contract.Invariant(Verifier != null);
        }

        [DataMember]
        public BigInteger SessionKey { get; set; }

        [DataMember]
        public BigInteger Salt { get; set; }

        [DataMember]
        public BigInteger Verifier { get; set; }
    }
}
