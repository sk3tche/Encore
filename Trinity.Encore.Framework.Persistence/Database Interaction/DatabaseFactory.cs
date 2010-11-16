using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using FluentNHibernate.Cfg;
using NHibernate;
using NHibernate.Cfg;

namespace Trinity.Encore.Framework.Persistence
{
    /// <summary>
    /// Creates instances of ISessionFactory.
    /// </summary>
    public static class DatabaseFactory
    {
        /// <summary>
        /// Creates an ISessionFactory based on the given configuration information.
        /// </summary>
        /// <param name="dialect">The SQL dialect to use.</param>
        /// <param name="driverClass">The fully-qualified name of the driver class to use.</param>
        /// <param name="connString">The connection string to be used to establish a connection.</param>
        /// <returns></returns>
        public static ISessionFactory CreateSessionFactory(string dialect, string driverClass, string connString)
        {
            Contract.Requires(!string.IsNullOrEmpty(dialect));
            Contract.Requires(!string.IsNullOrEmpty(driverClass));
            Contract.Requires(!string.IsNullOrEmpty(connString));
            Contract.Ensures(Contract.Result<ISessionFactory>() != null);

            var cfg = new Configuration
            {
                Properties = new Dictionary<string, string>
                {
                    { "connection.provider", "NHibernate.Connection.DriverConnectionProvider" },
                    { "dialect", dialect },
                    { "connection.driver_class", driverClass },
                    { "connection.connection_string", connString },
                }
            };

            var fluent = Fluently.Configure(cfg);

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                var asm1 = assembly;
                fluent.Mappings(x => x.FluentMappings.AddFromAssembly(asm1));
            }

            var factory = fluent.BuildSessionFactory();
            Contract.Assume(factory != null);
            return factory;
        }
    }
}
