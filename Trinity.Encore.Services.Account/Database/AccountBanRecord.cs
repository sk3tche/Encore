using System;
using System.Diagnostics.Contracts;
using FluentNHibernate.Mapping;
using Trinity.Encore.Framework.Persistence;
using Trinity.Encore.Framework.Persistence.Mapping;
using Trinity.Encore.Framework.Services.Account;

namespace Trinity.Encore.Services.Account.Database
{
    public class AccountBanRecord
    {
        /// <summary>
        /// Constructs a new AccountBanRecord object.
        /// 
        /// Should be used only by the underlying database layer.
        /// </summary>
        protected AccountBanRecord()
        {
        }

        /// <summary>
        /// Constructs a new AccountBanRecord object.
        /// 
        /// Should be inserted into the database.
        /// </summary>
        /// <param name="account"></param>
        public AccountBanRecord(AccountRecord account)
        {
            Contract.Requires(account != null);

            Account = account;
        }

        public AccountRecord Account { get; private set; }

        public string Notes { get; set; }

        public DateTime? Expiry { get; set; }

        public AccountBanData Serialize()
        {
            return new AccountBanData
            {
                AccountId = Account.Id,
                Notes = Notes,
                Expiry = Expiry,
            };
        }
    }

    public sealed class AccountBanMapping : MappableObject<AccountBanRecord>
    {
        public AccountBanMapping()
        {
            HasOne(c => c.Account).PropertyRef(c => c.Ban).Constrained().Cascade.SaveUpdate().LazyLoad(Laziness.Proxy);
            Map(c => c.Notes).Nullable().Update();
            Map(c => c.Expiry).Nullable().Update();
        }
    }
}
