using System;
using System.Net;
using Trinity.Encore.Framework.Game;
using Trinity.Encore.Framework.Game.Cryptography;
using Trinity.Encore.Framework.Persistence;
using FluentNHibernate;
using FluentNHibernate.Mapping;
using NHibernate;

namespace Trinity.Encore.Services.Account.Database
{
    public sealed class AccountRecord
    {
        public long Id { get; private set; }

        public string Name { get; set; }

        public string EmailAddress { get; set; }

        public byte[] SHA1Password { get; set; }

        public byte[] SHA256Password { get; set; }
        
        public ClientBoxLevel BoxLevel { get; set; }
        
        public ClientLocale Locale { get; set; }

        public DateTime? LastLogin { get; set; }

        public IPAddress LastIP { get; set; }
        
        public long? RecruiterId { get; set; }

        public AccountBanRecord Ban { get; set; }
    }
    
    public sealed class AccountMapping : MappableObject<AccountRecord>
    {
        public AccountMapping()
        {
            Id(c => c.Id).GeneratedBy.HiLo("Account");
            Map(c => c.Name).Not.Nullable().ReadOnly();
            Map(c => c.EmailAddress).Not.Nullable().Update();
            Map(c => c.SHA1Password).Not.Nullable().Update();
            Map(c => c.SHA256Password).Not.Nullable().Update();
            Map(c => c.BoxLevel).Not.Nullable().Update();
            Map(c => c.Locale).Not.Nullable().Update();
            Map(c => c.LastLogin).Nullable().Update();
            Map(c => c.LastIP).Nullable().Update();
            Map(c => c.RecruiterId).Nullable().Update();
            HasOne(c => c.Ban).PropertyRef(c => c.Account).Cascade.SaveUpdate().LazyLoad(Laziness.Proxy);
        }
    }
 }
