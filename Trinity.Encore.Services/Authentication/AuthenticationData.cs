using System.Runtime.Serialization;
using Trinity.Core.Cryptography;

namespace Trinity.Encore.Services.Authentication
{
    [DataContract]
    public sealed class AuthenticationData
    {
        [DataMember]
        public BigInteger SessionKey { get; set; }

        [DataMember]
        public BigInteger Salt { get; set; }

        [DataMember]
        public BigInteger Verifier { get; set; }
    }
}
