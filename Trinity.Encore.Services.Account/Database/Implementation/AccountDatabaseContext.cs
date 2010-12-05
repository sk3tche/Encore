using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FluentNHibernate;
using Trinity.Encore.Framework.Core.Configuration;
using Trinity.Encore.Framework.Persistence;

namespace Trinity.Encore.Services.Account.Database.Implementation
{
    public sealed class AccountDatabaseContext : DatabaseContext
    {
        [ConfigurationVariable("sqlType", DatabaseType.MySql, Static = true)]
        public static DatabaseType DatabaseType { get; set; }

        [ConfigurationVariable("sqlDialect", "NHibernate.Dialect.MySQLDialect", Static = true)]
        public static string SqlDialect { get; set; }

        [ConfigurationVariable("sqlDriverClass", "NHibernate.Driver.MySqlDataDriver", Static = true)]
        public static string DriverClass { get; set; }

        [ConfigurationVariable("sqlConnectionString", "Server=127.0.0.1;Database=Encore.Account;User ID=Encore;Password=Encore",
            Static = true)]
        public static string ConnectionString { get; set; }

        [SuppressMessage("Microsoft.Contracts", "Requires", Justification = "Configuration checked on startup.")]
        public AccountDatabaseContext()
            : base(DatabaseType, SqlDialect, DriverClass, ConnectionString)
        {
        }

        protected override IEnumerable<IMappingProvider> CreateMappings()
        {
            yield return new AccountMapping();
            yield return new AccountBanMapping();
            yield return new IPBanMapping();
            yield return new IPRangeBanMapping();
        }
    }
}
