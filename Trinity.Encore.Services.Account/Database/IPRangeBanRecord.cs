using System;
using System.Diagnostics.Contracts;
using System.Net;
using Trinity.Encore.Framework.Network;
using Trinity.Encore.Framework.Persistence.Mapping;
using Trinity.Encore.Services.Account.Bans;
using Trinity.Encore.Services.Account.Database.Implementation;

namespace Trinity.Encore.Services.Account.Database
{
    public class IPRangeBanRecord : AccountDatabaseRecord<IPRangeBanRecord>
    {
        /// <summary>
        /// Constructs a new IPRangeBanRecord object.
        /// 
        /// Should be used only by the underlying database layer.
        /// </summary>
        protected IPRangeBanRecord()
        {
        }

        /// <summary>
        /// Constructs a new IPRangeBanRecord object.
        /// 
        /// Should be inserted into the database.
        /// </summary>
        /// <param name="lowerAddress"></param>
        /// <param name="upperAddress"></param>
        /// <param name="notes"></param>
        /// <param name="expiry"></param>
        public IPRangeBanRecord(byte[] lowerAddress, byte[] upperAddress)
        {
            Contract.Requires(lowerAddress != null);
            Contract.Requires(upperAddress != null);

            LowerAddress = lowerAddress;
            UpperAddress = upperAddress;
        }

        public virtual long Id { get; protected /*private*/ set; }

        public virtual byte[] LowerAddress { get; protected /*private*/ set; }

        public virtual byte[] UpperAddress { get; protected /*private*/ set; }

        public virtual string Notes { get; set; }

        public virtual DateTime? Expiry { get; set; }
    }

    public sealed class IPRangeBanMapping : MappableObject<IPRangeBanRecord>
    {
        public IPRangeBanMapping()
        {
            Id(c => c.Id).Not.Nullable().GeneratedBy.Increment().Unique();
            Map(c => c.LowerAddress).Not.Nullable().Length(IPAddress.Loopback.GetLength());
            Map(c => c.UpperAddress).Not.Nullable().Length(IPAddress.Loopback.GetLength());
            Map(c => c.Notes).Nullable().Update().Length(BanManager.MaxNotesLength); ;
            Map(c => c.Expiry).Nullable().Update();
        }
    }
}
