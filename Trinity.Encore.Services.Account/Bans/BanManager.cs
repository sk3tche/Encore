using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net;
using Trinity.Encore.Framework.Game.Threading;
using Trinity.Encore.Framework.Network;
using Trinity.Encore.Services.Account.Database;

namespace Trinity.Encore.Services.Account.Bans
{
    public sealed class BanManager : SingletonActor<BanManager>
    {
        public const int MaxNotesLength = 512;

        private readonly List<AccountBan> _accountBans = new List<AccountBan>();

        private readonly List<IPBan> _ipBans = new List<IPBan>();

        private readonly List<IPRangeBan> _ipRangeBans = new List<IPRangeBan>();

        private BanManager()
        {
        }

        [ContractInvariantMethod]
        private void Invariant()
        {
            Contract.Invariant(_accountBans != null);
            Contract.Invariant(_ipBans != null);
            Contract.Invariant(_ipRangeBans != null);
        }

        #region Account bans

        public void AddAccountBan(AccountBan ban)
        {
            Contract.Requires(ban != null);

            lock (_accountBans)
                _accountBans.Add(ban);
        }

        public void RemoveAccountBan(AccountBan ban)
        {
            Contract.Requires(ban != null);

            lock (_accountBans)
                _accountBans.Remove(ban);
        }

        public AccountBan CreateAccountBan(Accounts.Account acc, string notes, DateTime? expiry)
        {
            Contract.Requires(acc != null);
            Contract.Ensures(Contract.Result<AccountBan>() != null);

            var rec = new AccountBanRecord(acc.Record)
            {
                Notes = notes,
                Expiry = expiry,
            };

            rec.Create();

            var ban = new AccountBan(rec);
            AddAccountBan(ban);
            return ban;
        }

        public IEnumerable<AccountBan> FindAccountBans(Func<AccountBan, bool> predicate)
        {
            Contract.Requires(predicate != null);
            Contract.Ensures(Contract.Result<IEnumerable<AccountBan>>() != null);

            lock (_accountBans)
                return _accountBans.Where(predicate);
        }

        public AccountBan FindAccountBan(Func<AccountBan, bool> predicate)
        {
            Contract.Requires(predicate != null);

            lock (_accountBans)
                return _accountBans.SingleOrDefault(predicate);
        }

        #endregion

        #region IP bans

        public void AddIPBan(IPBan ban)
        {
            Contract.Requires(ban != null);

            lock (_ipBans)
                _ipBans.Add(ban);
        }

        public void RemoveIPBan(IPBan ban)
        {
            Contract.Requires(ban != null);

            lock (_ipBans)
                _ipBans.Remove(ban);
        }

        public IPBan CreateIPBan(IPAddress ip, string notes, DateTime? expiry)
        {
            Contract.Requires(ip != null);
            Contract.Ensures(Contract.Result<IPBan>() != null);

            var rec = new IPBanRecord(ip.GetAddressBytes())
            {
                Notes = notes,
                Expiry = expiry,
            };

            rec.Create();

            var ban = new IPBan(rec);
            AddIPBan(ban);
            return ban;
        }

        public IEnumerable<IPBan> FindIPBans(Func<IPBan, bool> predicate)
        {
            Contract.Requires(predicate != null);
            Contract.Ensures(Contract.Result<IEnumerable<IPBan>>() != null);

            lock (_ipBans)
                return _ipBans.Where(predicate);
        }

        public IPBan FindIPBan(Func<IPBan, bool> predicate)
        {
            Contract.Requires(predicate != null);

            lock (_ipBans)
                return _ipBans.SingleOrDefault(predicate);
        }

        #endregion

        #region IP range bans

        public void AddIPRangeBan(IPRangeBan ban)
        {
            Contract.Requires(ban != null);

            lock (_ipRangeBans)
                _ipRangeBans.Add(ban);
        }

        public void RemoveIPRangeBan(IPRangeBan ban)
        {
            Contract.Requires(ban != null);

            lock (_ipRangeBans)
                _ipRangeBans.Remove(ban);
        }

        public IPRangeBan CreateIPRangeBan(IPAddressRange range, string notes, DateTime? expiry)
        {
            Contract.Requires(range != null);
            Contract.Ensures(Contract.Result<IPRangeBan>() != null);

            var rec = new IPRangeBanRecord(range.LowerBoundary, range.UpperBoundary)
            {
                Notes = notes,
                Expiry = expiry,
            };

            rec.Create();

            var ban = new IPRangeBan(rec);
            AddIPRangeBan(ban);
            return ban;
        }

        public IEnumerable<IPRangeBan> FindIPRangeBans(Func<IPRangeBan, bool> predicate)
        {
            Contract.Requires(predicate != null);
            Contract.Ensures(Contract.Result<IEnumerable<IPRangeBan>>() != null);

            lock (_ipRangeBans)
                return _ipRangeBans.Where(predicate);
        }

        public IPRangeBan FindIPRangeBan(Func<IPRangeBan, bool> predicate)
        {
            Contract.Requires(predicate != null);

            lock (_ipRangeBans)
                return _ipRangeBans.SingleOrDefault(predicate);
        }

        #endregion
    }
}
