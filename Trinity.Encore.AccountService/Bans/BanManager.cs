using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net;
using Trinity.Core.Collections;
using Trinity.Core.Logging;
using Trinity.Encore.AccountService.Database;
using Trinity.Encore.Game.Threading;
using Trinity.Network;

namespace Trinity.Encore.AccountService.Bans
{
    public sealed class BanManager : SingletonActor<BanManager>
    {
        public const int MaxNotesLength = 512;

        private static readonly LogProxy _log = new LogProxy("BanManager");

        private readonly List<AccountBan> _accountBans = new List<AccountBan>();

        private readonly List<IPBan> _ipBans = new List<IPBan>();

        private readonly List<IPRangeBan> _ipRangeBans = new List<IPRangeBan>();

        private BanManager()
        {
            _log.Info("Loading account bans...");

            var accountBans = AccountApplication.Instance.AccountDbContext.FindAll<AccountBanRecord>();
            foreach (var accBan in accountBans.Select(accountBan => new AccountBan(accountBan)))
                AddAccountBan(accBan);

            _log.Info("Loaded {0} account bans.", _accountBans.Count);

            _log.Info("Loading IP bans...");

            var ipBans = AccountApplication.Instance.AccountDbContext.FindAll<IPBanRecord>();
            foreach (var ipBan in ipBans.Select(ipBan => new IPBan(ipBan)))
                AddIPBan(ipBan);

            _log.Info("Loaded {0} IP bans.", _ipBans.Count);

            _log.Info("Loading IP range bans...");

            var ipRangeBans = AccountApplication.Instance.AccountDbContext.FindAll<IPRangeBanRecord>();
            foreach (var ipRangeBan in ipRangeBans.Select(ipRangeBan => new IPRangeBan(ipRangeBan)))
                AddIPRangeBan(ipRangeBan);

            _log.Info("Loaded {0} IP range bans.", _ipRangeBans.Count);
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

            _accountBans.Add(ban);
        }

        public void RemoveAccountBan(AccountBan ban)
        {
            Contract.Requires(ban != null);

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

        public void DeleteAccountBan(AccountBan ban)
        {
            Contract.Requires(ban != null);

            ban.Delete();
            RemoveAccountBan(ban);
        }

        public IEnumerable<AccountBan> FindAccountBans(Func<AccountBan, bool> predicate)
        {
            Contract.Requires(predicate != null);
            Contract.Ensures(Contract.Result<IEnumerable<AccountBan>>() != null);

            return _accountBans.Where(predicate).Force();
        }

        public AccountBan FindAccountBan(Func<AccountBan, bool> predicate)
        {
            Contract.Requires(predicate != null);

            return _accountBans.SingleOrDefault(predicate);
        }

        #endregion

        #region IP bans

        public void AddIPBan(IPBan ban)
        {
            Contract.Requires(ban != null);

            _ipBans.Add(ban);
        }

        public void RemoveIPBan(IPBan ban)
        {
            Contract.Requires(ban != null);

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

        public void DeleteIPBan(IPBan ban)
        {
            Contract.Requires(ban != null);

            ban.Delete();
            RemoveIPBan(ban);
        }

        public IEnumerable<IPBan> FindIPBans(Func<IPBan, bool> predicate)
        {
            Contract.Requires(predicate != null);
            Contract.Ensures(Contract.Result<IEnumerable<IPBan>>() != null);

            return _ipBans.Where(predicate).Force();
        }

        public IPBan FindIPBan(Func<IPBan, bool> predicate)
        {
            Contract.Requires(predicate != null);

            return _ipBans.SingleOrDefault(predicate);
        }

        #endregion

        #region IP range bans

        public void AddIPRangeBan(IPRangeBan ban)
        {
            Contract.Requires(ban != null);

            _ipRangeBans.Add(ban);
        }

        public void RemoveIPRangeBan(IPRangeBan ban)
        {
            Contract.Requires(ban != null);

            _ipRangeBans.Remove(ban);
        }

        public IPRangeBan CreateIPRangeBan(IPAddressRange range, string notes, DateTime? expiry)
        {
            Contract.Requires(range != null);
            Contract.Ensures(Contract.Result<IPRangeBan>() != null);

            var rec = new IPRangeBanRecord(range.LowerBoundary.GetAddressBytes(), range.UpperBoundary.GetAddressBytes())
            {
                Notes = notes,
                Expiry = expiry,
            };

            rec.Create();

            var ban = new IPRangeBan(rec);
            AddIPRangeBan(ban);
            return ban;
        }

        public void DeleteIPRangeBan(IPRangeBan ban)
        {
            Contract.Requires(ban != null);

            ban.Delete();
            RemoveIPRangeBan(ban);
        }

        public IEnumerable<IPRangeBan> FindIPRangeBans(Func<IPRangeBan, bool> predicate)
        {
            Contract.Requires(predicate != null);
            Contract.Ensures(Contract.Result<IEnumerable<IPRangeBan>>() != null);

            return _ipRangeBans.Where(predicate).Force();
        }

        public IPRangeBan FindIPRangeBan(Func<IPRangeBan, bool> predicate)
        {
            Contract.Requires(predicate != null);

            return _ipRangeBans.SingleOrDefault(predicate);
        }

        #endregion
    }
}
