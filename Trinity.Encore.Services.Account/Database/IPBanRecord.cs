using System;
using System.Diagnostics.Contracts;
using System.Net;
using Trinity.Encore.Framework.Network;
using Trinity.Encore.Framework.Persistence.Mapping;
using Trinity.Encore.Services.Account.Bans;
using Trinity.Encore.Services.Account.Database.Implementation;

namespace Trinity.Encore.Services.Account.Database
{
    public class IPBanRecord : AccountDatabaseRecord
    {
        /// <summary>
        /// Constructs a new IPBanRecord object.
        /// 
        /// Should be used only by the underlying database layer.
        /// </summary>
        protected IPBanRecord()
        {
        }

        /// <summary>
        /// Constructs a new IPBanRecord object.
        /// 
        /// Should be inserted into the database.
        /// </summary>
        /// <param name="address"></param>
        public IPBanRecord(byte[] address)
        {
            Contract.Requires(address != null);

            Address = address;
        }

        public virtual long Id { get; protected /*private*/ set; }

        public virtual byte[] Address { get; protected /*private*/ set; }

        public virtual string Notes { get; set; }

        public virtual DateTime? Expiry { get; set; }
    }

    public sealed class IPBanMapping : MappableObject<IPBanRecord>
    {
        public IPBanMapping()
        {
            Id(c => c.Id).Not.Nullable().GeneratedBy.Increment().Unique();
            Map(c => c.Address).Not.Nullable().Length(IPAddress.Loopback.GetLength());
            Map(c => c.Notes).Nullable().Update().Length(BanManager.MaxNotesLength);
            Map(c => c.Expiry).Nullable().Update();
        }
    }
}
