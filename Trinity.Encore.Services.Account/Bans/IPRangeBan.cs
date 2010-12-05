using System;
using System.Diagnostics.Contracts;
using System.Net;
using Trinity.Encore.Framework.Network;
using Trinity.Encore.Framework.Services.Account;
using Trinity.Encore.Services.Account.Database;

namespace Trinity.Encore.Services.Account.Bans
{
    public sealed class IPRangeBan
    {
        public IPRangeBan(IPRangeBanRecord record)
        {
            Contract.Requires(record != null);

            Record = record;
        }

        /// <summary>
        /// Deletes the IPRangeBan from the backing storage. This object is considered invalid once
        /// this method has been executed.
        /// </summary>
        internal void Delete()
        {
            Record.Delete();
        }

        public IPRangeBanData Serialize()
        {
            Contract.Ensures(Contract.Result<IPRangeBanData>() != null);

            return new IPRangeBanData
            {
                Range = Range,
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
        /// Gets the underlying record of this IPRangeBan. Should not be manipulated
        /// directly.
        /// </summary>
        public IPRangeBanRecord Record { get; private set; }

        public IPAddressRange Range
        {
            get
            {
                Contract.Ensures(Contract.Result<IPAddressRange>() != null);

                var lower = Record.LowerAddress;
                Contract.Assume(lower != null);
                var lowerAddr = new IPAddress(lower);
                var upper = Record.UpperAddress;
                Contract.Assume(upper != null);
                var upperAddr = new IPAddress(upper);
                Contract.Assume(lowerAddr.AddressFamily == upperAddr.AddressFamily);
                Contract.Assume(lowerAddr.GetLength() == upperAddr.GetLength());

                return new IPAddressRange(lowerAddr, upperAddr);
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
