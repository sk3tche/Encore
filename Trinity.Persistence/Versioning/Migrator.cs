using System;
using System.Diagnostics.Contracts;
using System.Reflection;

namespace Trinity.Persistence.Versioning
{
    /// <summary>
    /// Migrates a database between versions.
    /// </summary>
    public sealed class Migrator
    {
        [ContractInvariantMethod]
        private void Invariant()
        {
            Contract.Invariant(_migrator != null);
        }

        private readonly global::Migrator.Migrator _migrator;

        public Migrator(DatabaseType type, string connString, Assembly asm)
        {
            Contract.Requires(!string.IsNullOrEmpty(connString));
            Contract.Requires(asm != null);

            var dialect = GetDialectNameForType(type);
            _migrator = new global::Migrator.Migrator(dialect, connString, asm, false, new NullLogger());
        }

        private static string GetDialectNameForType(DatabaseType type)
        {
            Contract.Ensures(!string.IsNullOrEmpty(Contract.Result<string>()));

            switch (type)
            {
                case DatabaseType.MsSql2005:
                    return typeof(global::Migrator.Providers.SqlServer.SqlServer2005Dialect).FullName;
                case DatabaseType.MsSql2008:
                    return typeof(global::Migrator.Providers.SqlServer.SqlServerDialect).FullName;
                case DatabaseType.MsSqlCe:
                    return typeof(global::Migrator.Providers.SqlServer.SqlServerCeDialect).FullName;
                case DatabaseType.MySql:
                    return typeof(global::Migrator.Providers.Mysql.MysqlDialect).FullName;
                case DatabaseType.Oracle10:
                case DatabaseType.OracleData10:
                    return typeof(global::Migrator.Providers.Oracle.OracleDialect).FullName;
                case DatabaseType.PostgreSql:
                    return typeof(global::Migrator.Providers.PostgreSQL.PostgreSQLDialect).FullName;
                case DatabaseType.SQLite:
                    return typeof(global::Migrator.Providers.SQLite.SQLiteDialect).FullName;
                case DatabaseType.DB2:
                    throw new NotSupportedException("The given database type does not support migrations.");
            }

            throw new ArgumentOutOfRangeException("type");
        }

        /// <summary>
        /// Migrate towards a given schema version.
        /// </summary>
        /// <param name="version">The version to migrate to.</param>
        public void MigrateTo(long version)
        {
            _migrator.MigrateTo(version);
        }

        /// <summary>
        /// Migrate to the latest schema version.
        /// </summary>
        public void MigrateToLatest()
        {
            _migrator.MigrateToLastVersion();
        }
    }
}
