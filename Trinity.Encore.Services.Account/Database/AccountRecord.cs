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
        
        public string PasswordHash { get; set; }
        
        public ClientBoxLevel BoxLevel { get; set; }
        
        public ClientLocale Locale { get; set; }

        public DateTime? LastLogin { get; set; }

        public IPAddress LastIP { get; set; }
        
        public long? RecruiterId { get; set; }

    }
    
    public sealed class AccountMapping : MappableObject<AccountRecord>
    {
        public AccountMapping()
        {
            Id(c => c.Id).GeneratedBy.HiLo("Account");
            Map(c => c.Name).Not.Nullable();
            Map(c => c.BoxLevel).Not.Nullable();
            Map(c => c.Locale).Not.Nullable();
            Map(c => c.LastLogin).Not.Nullable();
            Map(c => c.LastIP).Not.Nullable();
            Map(c => c.RecruiterId).Not.Nullable();
        }
    }
 }
