using System;
using System.Diagnostics.Contracts;
using System.Net;
using FluentNHibernate.Mapping;
using Trinity.Encore.Framework.Game;
using Trinity.Encore.Framework.Game.Cryptography;
using Trinity.Encore.Framework.Persistence;
using Trinity.Encore.Framework.Persistence.Mapping;
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

        public virtual long Id { get; protected set; }

        public virtual string Name { get; protected set; }

        public virtual string EmailAddress { get; set; }

        public virtual byte[] SHA1Password { get; set; }

        public virtual byte[] SHA256Password { get; set; }
        
        public virtual ClientBoxLevel BoxLevel { get; set; }

        public virtual ClientLocale Locale { get; set; }

        public virtual DateTime? LastLogin { get; set; }

        public virtual byte[] LastIP { get; set; }

        public virtual AccountRecord Recruiter { get; set; }

        public virtual AccountBanRecord Ban { get; set; }

        public virtual AccountData Serialize()
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
            References(x => x.Recruiter).Nullable().Cascade.SaveUpdate().LazyLoad(Laziness.Proxy);
            HasOne(c => c.Ban).Cascade.All().LazyLoad(Laziness.Proxy);
        }
    }
 }
