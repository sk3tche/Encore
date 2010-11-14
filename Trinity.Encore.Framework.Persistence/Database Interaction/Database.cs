using System;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Trinity.Encore.Framework.Persistence
{
    class Database
    {
        private readonly string _hostname;
        private readonly uint _port;
        private readonly string _username;
        private readonly string _password;
        private readonly string _schema;

        /// <summary>
        /// Constructor of Database object. Parameters are taken from our configuration source,
        /// and should be verified before passed on here.
        /// </summary>
        public Database(string hostname, uint port, string username, string password, string schema)
        {
            _hostname = hostname;
            _port = port;
            _username = username;
            _password = password;
            _schema = schema;
        }

        /// <summary>
        /// This method will call the connect method of the underlying database layer.
        /// </summary>
        public virtual void Connect()
        {
        }

        /// <summary>
        /// This method will call the disconnect method of the underlying database layer.
        /// </summary>
        public virtual void Disconnect()
        {
        }

        /// <summary>
        /// This method will reconnect using Connect and Disconnect methods.
        /// If there is a direct reconnect method in the lower database layer, that will
        /// be called instead.
        /// </summary>
        public virtual void Reconnect()
        {
            /// Todo: call a reconnect function on the lower layer if present instead
            Disconnect();
            Connect();
        }
    }
}