using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using FluentNHibernate.Cfg;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Criterion;

namespace Trinity.Encore.Framework.Persistence
{
    public sealed class PersistenceManager
    {
        #region Public methods
        /// <summary>
        /// Creates an ISessionFactory based on the given configuration information.
        /// </summary>
        /// <param name="dialect">The SQL dialect to use.</param>
        /// <param name="driverClass">The fully-qualified name of the driver class to use.</param>
        /// <param name="connString">The connection string to be used to establish a connection.</param>
        /// <returns></returns>
        private void CreateSessionFactory(string dialect, string driverClass, string connString)
        {
            Contract.Requires(!string.IsNullOrEmpty(dialect));
            Contract.Requires(!string.IsNullOrEmpty(driverClass));
            Contract.Requires(!string.IsNullOrEmpty(connString));
            Contract.Ensures(Contract.Result<ISessionFactory>() != null);

            PersistenceManager.Config = new Configuration
            {
                Properties = new Dictionary<string, string>
                {
                    { "connection.provider", "NHibernate.Connection.DriverConnectionProvider" },
                    { "dialect", dialect },
                    { "connection.driver_class", driverClass },
                    { "connection.connection_string", connString },
                }
            };

            var fluent = Fluently.Configure(PersistenceManager.Config);

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                var asm1 = assembly;
                fluent.Mappings(x => x.FluentMappings.AddFromAssembly(asm1));
            }

            var factory = fluent.BuildSessionFactory();
            Contract.Assume(factory != null);
            PersistenceManager.SessionFactory = factory;
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Deletes an object of a specified type.
        /// </summary>
        /// <param name="itemsToDelete">The items to delete.</param>
        /// <typeparam name="T">The type of objects to delete.</typeparam>
        public void Delete<T>(T item)
        {
            using (ISession session = Session)
            {
                using (session.BeginTransaction())
                {
                    session.Delete(item);
                    session.Transaction.Commit();
                }
            }
        }

        /// <summary>
        /// Deletes objects of a specified type.
        /// </summary>
        /// <param name="itemsToDelete">The items to delete.</param>
        /// <typeparam name="T">The type of objects to delete.</typeparam>
        public void Delete<T>(IList<T> itemsToDelete)
        {
            using (ISession session = Session)
            {
                foreach (T item in itemsToDelete)
                {
                    using (session.BeginTransaction())
                    {
                        session.Delete(item);
                        session.Transaction.Commit();
                    }
                }
            }
        }

        /// <summary>
        /// Retrieves objects of a specified type where a specified property equals a specified value.
        /// </summary>
        /// <typeparam name="T">The type of the objects to be retrieved.</typeparam>
        /// <param name="propertyName">The name of the property to be tested.</param>
        /// <param name="propertyValue">The value that the named property must hold.</param>
        /// <returns>A list of all objects meeting the specified criteria.</returns>
        public IList<T> RetrieveEquals<T>(string propertyName, object propertyValue)
        {
            using (ISession session = Session)
            {
                // Create a criteria object with the specified criteria
                ICriteria criteria = session.CreateCriteria(typeof(T));
                criteria.Add(Expression.Eq(propertyName, propertyValue));

                // Get the matching objects
                IList<T> matchingObjects = criteria.List<T>();

                // Set return value
                return matchingObjects;
            }
        }

        /// <summary>
        /// Saves an object and its persistent children.
        /// </summary>
        public void Save<T>(T item)
        {
            using (ISession session = Session)
            {
                using (session.BeginTransaction())
                {
                    session.SaveOrUpdate(item);
                    session.Transaction.Commit();
                }
            }
        }
        #endregion

        #region Protected properties
        protected static Configuration Config
        {
            get { return _configuration; }
            private set { _configuration = value; }
        }

        protected static ISessionFactory SessionFactory
        {
            get { return _sessionFactory; }
            private set { _sessionFactory = value; }
        }

        protected static ISession Session
        {
            get
            {
                if (_session == null)
                    _session = SessionFactory.OpenSession();

                return _session;
            }

            set
            {
                _session = value;

                /// If the session is closed, open a new one
                if (!_session.IsOpen)
                    _session = SessionFactory.OpenSession();

            }
        }
        #endregion

        #region Private members
        private static Configuration _configuration = null;
        private static ISessionFactory _sessionFactory = null;
        private static ISession _session = null;
        #endregion
    }
}
