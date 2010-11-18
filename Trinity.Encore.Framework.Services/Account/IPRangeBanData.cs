using System;
using System.Diagnostics.Contracts;
using System.Runtime.Serialization;
using Trinity.Encore.Framework.Network;

namespace Trinity.Encore.Framework.Services.Account
{
    [DataContract(IsReference = true)]
    public sealed class IPRangeBanData
    {
        [ContractInvariantMethod]
        private void Invariant()
        {
            Contract.Invariant(Range != null);
        }

        [DataMember]
        public IPAddressRange Range { get; set; }

        [DataMember]
        public string Notes { get; set; }

        [DataMember]
        public DateTime? Expiry { get; set; }
    }
}
