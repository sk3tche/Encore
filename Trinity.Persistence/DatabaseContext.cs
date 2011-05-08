using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;
using FluentNHibernate;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Linq;
using Trinity.Core.Threading.Actors;
using Trinity.Persistence.Schema;

namespace Trinity.Persistence
{
    [ContractClass(typeof(DatabaseContextContracts))]
    public abstract class DatabaseContext : Actor<DatabaseContext>
    {
        [ContractInvariantMethod]
        private void Invariant()
        {
            Contract.Invariant(SessionFactory != null);
            Contract.Invariant(Schema != null);
            Contract.Invariant(Configuration != null);
        }

        protected internal Configuration Configuration { get; private set; }

        protected ISessionFactory SessionFactory { get; private set; }

        public SchemaInfo Schema { get; private set; }

        protected DatabaseContext(DatabaseType type, string connString)
        {
            Contract.Requires(!string.IsNullOrEmpty(connString));

            Configure(type, connString);
        }

        protected override void Dispose(bool disposing)
        {
            SessionFactory.Dispose();

            base.Dispose(disposing);
        }

        [SuppressMessage("Microsoft.Maintainability", "CA1502", Justification = "Switch statements are not that evil...")]
        private static IPersistenceConfigurer CreateConfiguration(DatabaseType type, string connString)
        {
            Contract.Requires(!string.IsNullOrEmpty(connString));
            Contract.Ensures(Contract.Result<IPersistenceConfigurer>() != null);

            IPersistenceConfigurer config;

            switch (type)
            {
                case DatabaseType.DB2:
                    config = DB2Configuration.Standard.ConnectionString(connString);
                    break;
                case DatabaseType.MsSql2005:
                    config = MsSqlConfiguration.MsSql2005.ConnectionString(connString);
                    break;
                case DatabaseType.MsSql2008:
                    config = MsSqlConfiguration.MsSql2008.ConnectionString(connString);
                    break;
                case DatabaseType.MsSqlCe:
                    config = MsSqlCeConfiguration.Standard.ConnectionString(connString);
                    break;
                case DatabaseType.MySql:
                    config = MySQLConfiguration.Standard.ConnectionString(connString);
                    break;
                case DatabaseType.Oracle10:
                    config = OracleClientConfiguration.Oracle10.ConnectionString(connString);
                    break;
                case DatabaseType.OracleData10:
                    config = OracleDataClientConfiguration.Oracle10.ConnectionString(connString);
                    break;
                case DatabaseType.PostgreSql:
                    config = PostgreSQLConfiguration.Standard.ConnectionString(connString);
                    break;
                case DatabaseType.SQLite:
                    config = SQLiteConfiguration.Standard.ConnectionString(connString);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("type");
            }

            Contract.Assume(config != null);
            return config;
        }

        /// <summary>
        /// Configures the DatabaseContext.
        /// </summary>
        /// <param name="dbType">The type of SQL server to connect to.</param>
        /// <param name="connString">The connection string to be used to establish a connection.</param>
        private void Configure(DatabaseType dbType, string connString)
        {
            Contract.Requires(!string.IsNullOrEmpty(connString));
            Contract.Ensures(SessionFactory != null);
            Contract.Ensures(Schema != null);
            Contract.Ensures(Configuration != null);

            var fluent = Fluently.Configure();
            fluent.Database(CreateConfiguration(dbType, connString));

            foreach (var mappingType in CreateMappings().Select(mapping => mapping.GetType()))
            {
                var type = mappingType;
                fluent.Mappings(x => x.FluentMappings.Add(type));
            }

            foreach (var convention in CreateConventions())
            {
                var conv = convention;
                fluent.Mappings(x => x.FluentMappings.Conventions.Add(conv));
            }

            var config = fluent.BuildConfiguration();
            Contract.Assume(config != null);
            Configuration = config;

            var factory = fluent.BuildSessionFactory();
            Contract.Assume(factory != null);
            SessionFactory = factory;

            Schema = new SchemaInfo(this);
        }

        protected abstract IEnumerable<IMappingProvider> CreateMappings();

        protected virtual IEnumerable<IConvention> CreateConventions()
        {
            Contract.Ensures(Contract.Result<IEnumerable<IConvention>>() != null);

            yield break;
        }

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

        /// <summary>
        /// Adds an entity and its persistent children to the database.
        /// </summary>
        /// <param name="item">The entity to add.</param>
        public void Add(object item)
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
        /// <param name="itemsToSave">The entities to add.</param>
        public void Add(IEnumerable<object> itemsToSave)
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
        /// <param name="item">The entity to update.</param>
        public void Update(object item)
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
        /// <param name="itemsToSave">The entities to update.</param>
        public void Update(IEnumerable<object> itemsToSave)
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
        /// <param name="item">The entity to delete.</param>
        public void Delete(object item)
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
        /// <param name="itemsToDelete">The entities to delete.</param>
        public void Delete(IEnumerable<object> itemsToDelete)
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
                var linq = session.Query<T>();
                Contract.Assume(linq != null);
                return linq.Where(criteria).ToList();
            }
        }

        /// <summary>
        /// Retrieves all entities of a given type.
        /// </summary>
        /// <typeparam name="T">The type of the entities to be retrieved.</typeparam>
        /// <returns>A list of all entities of the given type.</returns>
        public IEnumerable<T> FindAll<T>()
            where T : class
        {
            Contract.Ensures(Contract.Result<IEnumerable<T>>() != null);

            return Find<T>(x => true);
        }
    }

    [ContractClassFor(typeof(DatabaseContext))]
    public abstract class DatabaseContextContracts : DatabaseContext
    {
        protected DatabaseContextContracts(DatabaseType type, string connString)
            : base(type, connString)
        {
        }

        protected override IEnumerable<IMappingProvider> CreateMappings()
        {
            Contract.Ensures(Contract.Result<IEnumerable<IMappingProvider>>() != null);

            return null;
        }
    }
}
