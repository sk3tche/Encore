using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using FluentNHibernate.Cfg;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Criterion;
using NHibernate.Linq;
using Trinity.Encore.Framework.Core.Reflection;
using Trinity.Encore.Framework.Persistence.Schema;

namespace Trinity.Encore.Framework.Persistence
{
    public class DatabaseContext
    {
        [ContractInvariantMethod]
        private void Invariant()
        {
            Contract.Invariant(Config != null);
            Contract.Invariant(SessionFactory != null);
            Contract.Invariant(Schema != null);
        }

        public DatabaseContext(string dialect, string driverClass, string connString)
        {
            Contract.Requires(!string.IsNullOrEmpty(dialect));
            Contract.Requires(!string.IsNullOrEmpty(driverClass));
            Contract.Requires(!string.IsNullOrEmpty(connString));

            Configure(dialect, driverClass, connString);
        }

        #region Private methods

        /// <summary>
        /// Configures the DatabaseContext.
        /// </summary>
        /// <param name="dialect">The SQL dialect to use.</param>
        /// <param name="driverClass">The fully-qualified name of the driver class to use.</param>
        /// <param name="connString">The connection string to be used to establish a connection.</param>
        private void Configure(string dialect, string driverClass, string connString)
        {
            Contract.Requires(!string.IsNullOrEmpty(dialect));
            Contract.Requires(!string.IsNullOrEmpty(driverClass));
            Contract.Requires(!string.IsNullOrEmpty(connString));
            Contract.Ensures(Config != null);
            Contract.Ensures(SessionFactory != null);
            Contract.Ensures(Schema != null);

            Config = new Configuration
            {
                Properties = new Dictionary<string, string>
                {
                    { "connection.provider", "NHibernate.Connection.DriverConnectionProvider" },
                    { "dialect", dialect },
                    { "connection.driver_class", driverClass },
                    { "connection.connection_string", connString },
                }
            };

            var fluent = Fluently.Configure(Config);

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                var asm = assembly;
                fluent.Mappings(x => x.FluentMappings.AddFromAssembly(asm));
            }

            var factory = fluent.BuildSessionFactory();
            Contract.Assume(factory != null);
            SessionFactory = factory;

            Schema = new SchemaInfo(this);
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// Creates a disposable database session.
        /// </summary>
        /// <returns>A unit-of-work session which should be disposed ASAP.</returns>
        protected ISession CreateSession()
        {
            Contract.Ensures(Contract.Result<ISession>() != null);

            var session = SessionFactory.OpenSession();
            Contract.Assume(session != null);
            return session;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Adds an entity and its persistent children to the database.
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <param name="item">The entity to add.</param>
        public void Add<T>(T item)
            where T : class
        {
            Contract.Requires(item != null);

            using (var session = CreateSession())
            {
                using (session.BeginTransaction())
                {
                    session.Persist(item);
                    session.Transaction.Commit();
                }
            }
        }

        /// <summary>
        /// Adds a list of entities and their persistent children to the database.
        /// </summary>
        /// <typeparam name="T">The type of the entities.</typeparam>
        /// <param name="itemsToSave">The entities to add.</param>
        public void Add<T>(IEnumerable<T> itemsToSave)
            where T : class
        {
            Contract.Requires(itemsToSave != null);

            using (var session = CreateSession())
            {
                using (session.BeginTransaction())
                {
                    foreach (var item in itemsToSave)
                        session.Persist(item);

                    session.Transaction.Commit();
                }
            }
        }

        /// <summary>
        /// Saves the updated values of an entity and its persistent children to the database.
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <param name="item">The entity to update.</param>
        public void Update<T>(T item)
            where T : class
        {
            Contract.Requires(item != null);

            using (var session = CreateSession())
            {
                using (session.BeginTransaction())
                {
                    session.Update(item);
                    session.Transaction.Commit();
                }
            }
        }

        /// <summary>
        /// Saves the updated values of a list of entities and their persistent children to the database.
        /// </summary>
        /// <typeparam name="T">The type of the entities.</typeparam>
        /// <param name="itemsToSave">The entities to update.</param>
        public void Update<T>(IEnumerable<T> itemsToSave)
            where T : class
        {
            Contract.Requires(itemsToSave != null);

            using (var session = CreateSession())
            {
                using (session.BeginTransaction())
                {
                    foreach (var item in itemsToSave)
                        session.Update(item);

                    session.Transaction.Commit();
                }
            }
        }

        /// <summary>
        /// Deletes an entity from the database.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <param name="item">The entity to delete.</param>
        public void Delete<T>(T item)
            where T : class
        {
            Contract.Requires(item != null);

            using (var session = CreateSession())
            {
                using (session.BeginTransaction())
                {
                    session.Delete(item);
                    session.Transaction.Commit();
                }
            }
        }

        /// <summary>
        /// Deletes a list of entities from the database.
        /// </summary>
        /// <typeparam name="T">The type of the entities.</typeparam>
        /// <param name="itemsToDelete">The entities to delete.</param>
        public void Delete<T>(IEnumerable<T> itemsToDelete)
            where T : class
        {
            Contract.Requires(itemsToDelete != null);

            using (var session = CreateSession())
            {
                using (session.BeginTransaction())
                {
                    foreach (var item in itemsToDelete)
                        session.Delete(item);

                    session.Transaction.Commit();
                }
            }
        }

        /// <summary>
        /// Retrieves a list of entities matching the given criteria.
        /// </summary>
        /// <typeparam name="T">The type of the entities to be retrieved.</typeparam>
        /// <param name="criteria">The criteria to use when searching.</param>
        /// <returns>A list of all entities meeting the specified criteria.</returns>
        public IEnumerable<T> Find<T>(Func<T, bool> criteria)
            where T : class
        {
            Contract.Requires(criteria != null);
            Contract.Ensures(Contract.Result<IEnumerable<T>>() != null);

            using (var session = CreateSession())
            {
                var linq = session.Linq<T>();
                Contract.Assume(linq != null);
                return linq.Where(criteria);
            }
        }

        #endregion

        #region Public properties

        public SchemaInfo Schema { get; private set; }

        #endregion

        #region Protected properties

        protected internal Configuration Config { get; private set; }

        protected ISessionFactory SessionFactory { get; private set; }

        #endregion
    }
}
