using System;
using System.Diagnostics.Contracts;
using System.Net;
using Trinity.Encore.AccountService.Bans;
using Trinity.Encore.AccountService.Database.Implementation;
using Trinity.Network;
using Trinity.Persistence.Mapping;

namespace Trinity.Encore.AccountService.Database
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
            Id(c => c.Id);
            Map(c => c.Address).Length(IPAddress.IPv6Loopback.GetLength());
            Map(c => c.Notes).Nullable().Length(BanManager.MaxNotesLength);
            Map(c => c.Expiry).Nullable();
        }
    }
}
