using System;
using System.Net;
using System.Runtime.Serialization;
using Trinity.Encore.Framework.Game;
using Trinity.Encore.Framework.Game.Cryptography;
using Trinity.Encore.Framework.Persistence;
using FluentNHibernate;
using FluentNHibernate.Mapping;
using NHibernate;

namespace Trinity.Encore.Framework.Services.Account
{
    [DataContract]
    public sealed class AccountRecord 
    {
        [DataMember]
        public long Id { get; private set; }

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

    public sealed class AccountMapping : MappableObject<AccountRecord>
    {
        public AccountMapping()
        {
            Id(c => c.Id).GeneratedBy.HiLo("Account");
            Map(c => c.Name).Not.Nullable();
            Map(c => c.EmailAddress).Not.Nullable();
            Map(c => c.BoxLevel).Not.Nullable();
            Map(c => c.Locale).Not.Nullable();
            Map(c => c.LastLogin).Not.Nullable();
            Map(c => c.LastIP).Not.Nullable();
            Map(c => c.RecruiterId).Not.Nullable();
        }
    }
}
