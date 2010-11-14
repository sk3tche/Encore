using System;
using System.Diagnostics.Contracts;
using System.Data.SQLite;
using NHibernate;
using FluentNHibernate;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Mapping;

namespace Trinity.Encore.Framework.Persistence
{
    public static abstract class DatabaseFactory
    {
    }

    public static class MySQLDatabaseFactory : DatabaseFactory
    {
        /// <summary>
        /// Creates a session factory for a MySQL Driver database
        /// </summary>
        /// <param name="connectionString">http://www.connectionstrings.com</param>
        /// <returns></returns>
        private static ISessionFactory CreateSessionFactory(string connectionString)
        {
            return
                Fluently.Configure()
                .Database(MySQLConfiguration.Standard.ConnectionString(connectionString))
                /*.Mappings() */
                .BuildSessionFactory();
        }
    }

    public static class PostgreSQLDatabaseFactory : DatabaseFactory
    {
        /// <summary>
        /// Creates a session factory for a PostgreSQL Driver database
        /// </summary>
        /// <param name="connectionString">http://www.connectionstrings.com</param>
        /// <returns></returns>
        private static ISessionFactory CreateSessionFactory(string connectionString)
        {
            return
                Fluently.Configure()
                .Database(PostgreSQLConfiguration.Standard.ConnectionString(connectionString))
                /*.Mappings() */
                .BuildSessionFactory();
        }
    }

  
}