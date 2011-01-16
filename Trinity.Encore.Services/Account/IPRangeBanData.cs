using System;
using System.Runtime.Serialization;
using Trinity.Network;

namespace Trinity.Encore.Services.Account
{
    [DataContract(IsReference = true)]
    public sealed class IPRangeBanData
    {
        [DataMember]
        public IPAddressRange Range { get; set; }

        [DataMember]
        public string Notes { get; set; }

        [DataMember]
        public DateTime? Expiry { get; set; }
    }
}
