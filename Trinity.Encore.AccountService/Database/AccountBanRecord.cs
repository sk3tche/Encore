using System;
using System.Diagnostics.Contracts;
using Trinity.Encore.AccountService.Bans;
using Trinity.Encore.AccountService.Database.Implementation;
using Trinity.Persistence.Mapping;

namespace Trinity.Encore.AccountService.Database
{
    public class AccountBanRecord : AccountDatabaseRecord
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

        public virtual long Id { get; protected /*private*/ set; }

        public virtual AccountRecord Account { get; protected /*private*/ set; }

        public virtual string Notes { get; set; }

        public virtual DateTime? Expiry { get; set; }
    }

    public sealed class AccountBanMapping : MappableObject<AccountBanRecord>
    {
        public AccountBanMapping()
        {
            Id(c => c.Id);
            HasOne(c => c.Account).Constrained();
            Map(c => c.Notes).Nullable().Length(BanManager.MaxNotesLength);
            Map(c => c.Expiry).Nullable();
        }
    }
}
