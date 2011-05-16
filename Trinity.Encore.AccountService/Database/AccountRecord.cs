using System;
using System.Diagnostics.Contracts;
using Trinity.Encore.AccountService.Accounts;
using Trinity.Encore.AccountService.Database.Implementation;
using Trinity.Encore.Game;
using Trinity.Encore.Game.Cryptography;
using Trinity.Persistence.Mapping;

namespace Trinity.Encore.AccountService.Database
{
    public class AccountRecord : AccountDatabaseRecord
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
        public AccountRecord(string name, string email, byte[] sha1, byte[] sha256, ClientBoxLevel boxLevel = ClientBoxLevel.Cataclysm,
            ClientLocale locale = ClientLocale.English)
        {
            Contract.Requires(!string.IsNullOrEmpty(name));
            Contract.Requires(name.Length >= Constants.Accounts.MinNameLength);
            Contract.Requires(name.Length <= Constants.Accounts.MaxNameLength);
            Contract.Requires(!string.IsNullOrEmpty(email));
            Contract.Requires(sha1 != null);
            Contract.Requires(sha1.Length == Password.SHA1Length);
            Contract.Requires(sha256 != null);
            Contract.Requires(sha256.Length == Password.SHA256Length);

            Name = name;
            EmailAddress = email;
            SHA1Password = sha1;
            SHA256Password = sha256;
            BoxLevel = boxLevel;
            Locale = locale;
        }

        public virtual long Id { get; protected set; }

        public virtual string Name { get; protected /*private*/ set; }

        public virtual string EmailAddress { get; set; }

        public virtual byte[] SHA1Password { get; set; }

        public virtual byte[] SHA256Password { get; set; }
        
        public virtual ClientBoxLevel BoxLevel { get; set; }

        public virtual ClientLocale Locale { get; set; }

        public virtual DateTime? LastLogin { get; set; }

        public virtual byte[] LastIP { get; set; }

        public virtual AccountRecord Recruiter { get; set; }
    }
    
    public sealed class AccountMapping : MappableObject<AccountRecord>
    {
        public AccountMapping()
        {
            Id(c => c.Id);
            Map(c => c.Name).Length(Constants.Accounts.MaxNameLength);
            Map(c => c.EmailAddress);
            Map(c => c.SHA1Password).Length(Password.SHA1Length);
            Map(c => c.SHA256Password).Length(Password.SHA256Length);
            Map(c => c.BoxLevel);
            Map(c => c.Locale);
            Map(c => c.LastLogin).Nullable();
            Map(c => c.LastIP).Nullable();
            References(x => x.Recruiter).Nullable();
        }
    }
 }
