using System;
using System.Diagnostics.Contracts;
using System.Net;
using FluentNHibernate.Mapping;
using Trinity.Encore.Framework.Game;
using Trinity.Encore.Framework.Game.Cryptography;
using Trinity.Encore.Framework.Persistence;
using Trinity.Encore.Framework.Services.Account;

namespace Trinity.Encore.Services.Account.Database
{
    public class AccountRecord
    {
        /// <summary>
        /// Constructs a new AccountRecord object.
        /// 
        /// This should be used only by the underlying database layer.
        /// </summary>
        protected AccountRecord()
        {
        }

        /// <summary>
        /// Constructs a new AccountRecord object.
        /// 
        /// Should be inserted into the database.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <param name="sha1"></param>
        /// <param name="sha256"></param>
        /// <param name="boxLevel"></param>
        /// <param name="locale"></param>
        public AccountRecord(string name, string email, byte[] sha1, byte[] sha256,
            ClientBoxLevel boxLevel = ClientBoxLevel.Cataclysm,
            ClientLocale locale = ClientLocale.English)
        {
            Contract.Requires(!string.IsNullOrEmpty(name));
            Contract.Requires(!string.IsNullOrEmpty(email));
            Contract.Requires(sha1 != null);
            Contract.Requires(sha256 != null);

            Name = name;
            EmailAddress = email;
            SHA1Password = sha1;
            SHA256Password = sha256;
            BoxLevel = boxLevel;
            Locale = locale;
        }

        public long Id { get; set; }

        public string Name { get; set; }

        public string EmailAddress { get; set; }

        public byte[] SHA1Password { get; set; }

        public byte[] SHA256Password { get; set; }
        
        public ClientBoxLevel BoxLevel { get; set; }
        
        public ClientLocale Locale { get; set; }

        public DateTime? LastLogin { get; set; }

        public byte[] LastIP { get; set; }
        
        public AccountRecord Recruiter { get; set; }

        public AccountBanRecord Ban { get; set; }

        public AccountData Serialize()
        {
            Contract.Assume(SHA1Password != null);
            Contract.Assume(SHA1Password.Length == 20);
            Contract.Assume(SHA256Password != null);
            Contract.Assume(SHA256Password.Length == 32);

            return new AccountData
            {
                Id = Id,
                Name = Name,
                Password = new Password(SHA1Password, SHA256Password),
                BoxLevel = BoxLevel,
                Locale = Locale,
                LastLogin = LastLogin,
                LastIP = LastIP != null ? new IPAddress(LastIP) : null,
                RecruiterId = Recruiter != null ? Recruiter.Id : 0,
            };
        }
    }
    
    public sealed class AccountMapping : MappableObject<AccountRecord>
    {
        public AccountMapping()
        {
            Id(c => c.Id).Not.Nullable().GeneratedBy.Increment().Unique();
            Map(c => c.Name).Not.Nullable().ReadOnly();
            Map(c => c.EmailAddress).Not.Nullable().Update();
            Map(c => c.SHA1Password).Not.Nullable().Update();
            Map(c => c.SHA256Password).Not.Nullable().Update();
            Map(c => c.BoxLevel).Not.Nullable().Update();
            Map(c => c.Locale).Not.Nullable().Update();
            Map(c => c.LastLogin).Nullable().Update();
            Map(c => c.LastIP).Nullable().Update();
            Map(c => c.Recruiter).Nullable().Update();
            References(x => x.Recruiter).Cascade.SaveUpdate().LazyLoad(Laziness.Proxy);
            HasOne(c => c.Ban).PropertyRef(c => c.Account).Cascade.All().LazyLoad(Laziness.Proxy);
        }
    }
 }
