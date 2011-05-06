using System;
using System.Net;
using System.Runtime.Serialization;
using Trinity.Encore.Game;
using Trinity.Encore.Game.Cryptography;

namespace Trinity.Encore.Services.Account
{
    [DataContract]
    public sealed class AccountData
    {
        [DataMember]
        public long Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string EmailAddress { get; set; }

        [DataMember]
        public Password Password { get; set; }

        [DataMember]
        public ClientBoxLevel BoxLevel { get; set; }

        [DataMember]
        public ClientLocale Locale { get; set; }

        [DataMember]
        public DateTime? LastLogin { get; set; }

        [DataMember]
        public IPAddress LastIP { get; set; }

        [DataMember]
        public long? RecruiterId { get; set; }
    }
}
