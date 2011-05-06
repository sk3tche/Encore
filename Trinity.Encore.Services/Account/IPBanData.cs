using System;
using System.Net;
using System.Runtime.Serialization;

namespace Trinity.Encore.Services.Account
{
    [DataContract]
    public sealed class IPBanData
    {
        [DataMember]
        public IPAddress Address { get; set; }

        [DataMember]
        public string Notes { get; set; }

        [DataMember]
        public DateTime? Expiry { get; set; }
    }
}
