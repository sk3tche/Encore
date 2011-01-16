using System;
using System.Diagnostics.Contracts;
using System.Net;
using Trinity.Core.Runtime.Serialization;
using Trinity.Encore.AccountService.Database;
using Trinity.Encore.Services.Account;

namespace Trinity.Encore.AccountService.Bans
{
    public sealed class IPBan : IMemberwiseSerializable<IPBanData>
    {
        public IPBan(IPBanRecord record)
        {
            Contract.Requires(record != null);

            Record = record;
        }

        /// <summary>
        /// Deletes the IPBan from the backing storage. This object is considered invalid once
        /// this method has been executed.
        /// </summary>
        internal void Delete()
        {
            Record.Delete();
        }

        public IPBanData Serialize()
        {
            Contract.Ensures(Contract.Result<IPBanData>() != null);

            return new IPBanData
            {
                Address = Address,
                Notes = Notes,
                Expiry = Expiry,
            };
        }

        [ContractInvariantMethod]
        private void Invariant()
        {
            Contract.Invariant(Record != null);
        }

        /// <summary>
        /// Gets the underlying record of this IPBan. Should not be manipulated
        /// directly.
        /// </summary>
        public IPBanRecord Record { get; private set; }

        public IPAddress Address
        {
            get
            {
                Contract.Ensures(Contract.Result<IPAddress>() != null);

                var addr = Record.Address;
                Contract.Assume(addr != null);
                return new IPAddress(addr);
            }
        }

        public string Notes
        {
            get { return Record.Notes; }
            set
            {
                Record.Notes = value;
                Record.Update();
            }
        }

        public DateTime? Expiry
        {
            get { return Record.Expiry; }
            set
            {
                Record.Expiry = value;
                Record.Update();
            }
        }
    }
}
