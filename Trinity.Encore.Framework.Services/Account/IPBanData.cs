using System;
using System.Diagnostics.Contracts;
using System.Net;
using System.Runtime.Serialization;

namespace Trinity.Encore.Framework.Services.Account
{
    [DataContract(IsReference = true)]
    public sealed class IPBanData
    {
        [ContractInvariantMethod]
        private void Invariant()
        {
            Contract.Invariant(Address != null);
        }

        [DataMember]
        public IPAddress Address { get; set; }

        [DataMember]
        public string Notes { get; set; }

        [DataMember]
        public DateTime? Expiry { get; set; }
    }
}
