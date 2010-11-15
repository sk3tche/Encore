using System;
using FluentNHibernate.Mapping;
using Trinity.Encore.Framework.Persistence;

namespace Trinity.Encore.Services.Account.Database
{
    public sealed class AccountBanRecord
    {
        public AccountRecord Account { get; private set; }

        public string Notes { get; set; }

        public DateTime? Expiry { get; set; }
    }

    public sealed class AccountBanMapping : MappableObject<AccountBanRecord>
    {
        public AccountBanMapping()
        {
            HasOne(c => c.Account).PropertyRef(c => c.Ban).Constrained().LazyLoad(Laziness.Proxy);
            Map(c => c.Notes).Nullable().Update();
            Map(c => c.Expiry).Nullable().Update();
        }
    }
}
