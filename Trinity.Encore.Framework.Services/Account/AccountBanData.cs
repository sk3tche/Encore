using System;
using System.Runtime.Serialization;

namespace Trinity.Encore.Framework.Services.Account
{
    [DataContract]
    public sealed class AccountBanData
    {
        [DataMember]
        public long AccountId { get; set; }

        [DataMember]
        public string Notes { get; set; }

        [DataMember]
        public DateTime? Expiry { get; set; }
    }
}
