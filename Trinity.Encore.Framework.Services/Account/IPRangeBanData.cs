using System;
using System.Runtime.Serialization;
using Trinity.Encore.Framework.Network;

namespace Trinity.Encore.Framework.Services.Account
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
