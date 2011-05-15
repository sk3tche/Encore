using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.ServiceModel;
using Trinity.Core.Configuration;
using Trinity.Encore.AccountService.Accounts;
using Trinity.Encore.AccountService.Bans;
using Trinity.Encore.Game;
using Trinity.Encore.Services.Account;
using Trinity.Network;

namespace Trinity.Encore.AccountService.Services
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, IncludeExceptionDetailInFaults = true,
        ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public sealed class AccountService : IAccountService
    {
        [ConfigurationVariable("IpcUri", "net.tcp://127.0.0.1:9501", Static = true)]
        [SuppressMessage("Microsoft.Design", "CA1056", Justification = "This is a configuration variable.")]
        public static string IpcUri { get; set; }

        public AccountData GetAccount(string userName)
        {
            Account acc = null;
            AccountManager.Instance.PostWait(mgr => acc = mgr.FindAccount(x => x.Name == userName)).Wait();
            return acc != null ? acc.Serialize() : null;
        }

        public void CreateAccount(string accountName, string password, string emailAddress, ClientLocale locale, ClientBoxLevel boxLevel)
        {
            if (accountName.Length < Constants.Accounts.MinNameLength || accountName.Length > Constants.Accounts.MaxNameLength)
                throw new ArgumentException("Account name has an invalid length.");

            if (password.Length < Constants.Accounts.MinPasswordLength || password.Length > Constants.Accounts.MaxPasswordLength)
                throw new ArgumentException("Password has an invalid length.");

            AccountManager.Instance.PostAsync(x => x.CreateAccount(accountName, password, emailAddress, boxLevel, locale));
        }

        public void SetLastIP(string userName, IPAddress ip)
        {
            AccountManager.Instance.PostAsync(mgr =>
            {
                var acc = mgr.FindAccount(x => x.Name == userName);
                if (acc != null)
                    acc.LastIP = ip;
            });
        }

        public void SetLastLogin(string userName, DateTime time)
        {
            AccountManager.Instance.PostAsync(mgr =>
            {
                var acc = mgr.FindAccount(x => x.Name == userName);
                if (acc != null)
                    acc.LastLogin = time;
            });
        }

        public AccountBanData GetAccountBan(string userName)
        {
            AccountBan ban = null;
            BanManager.Instance.PostWait(mgr => ban = mgr.FindAccountBan(x => x.Account.Name == userName)).Wait();
            return ban != null ? ban.Serialize() : null;
        }

        public void CreateAccountBan(string accountName, string notes, DateTime? expiry)
        {
            Account acc = null;
            AccountManager.Instance.PostWait(mgr => acc = mgr.FindAccount(x => x.Name == accountName)).Wait();

            if (acc == null)
                throw new ArgumentException("No account found.");

            if (acc.Ban != null)
                throw new ArgumentException("Account ban already exists.");

            BanManager.Instance.PostAsync(mgr => mgr.CreateAccountBan(acc, notes, expiry));
        }

        public IPBanData GetIPBan(IPAddress address)
        {
            IPBan ban = null;
            BanManager.Instance.PostWait(mgr => ban = mgr.FindIPBan(x => x.Address.Equals(address))).Wait();
            return ban != null ? ban.Serialize() : null;
        }

        public void CreateIPBan(IPAddress address, string notes, DateTime? expiry)
        {
            IPBan ban = null;
            BanManager.Instance.PostWait(mgr => ban = mgr.FindIPBan(x => x.Equals(address))).Wait();

            if (ban != null)
                throw new ArgumentException("IP ban already exists.");

            BanManager.Instance.PostAsync(mgr => mgr.CreateIPBan(address, notes, expiry));
        }

        public IPRangeBanData GetIPRangeBan(IPAddress address)
        {
            IPRangeBan ban = null;
            BanManager.Instance.PostWait(mgr => ban = mgr.FindIPRangeBan(x => x.Range.IsInRange(address))).Wait();
            return ban != null ? ban.Serialize() : null;
        }

        public void CreateIPRangeBan(IPAddressRange range, string notes, DateTime? expiry)
        {
            IPRangeBan ban = null;
            BanManager.Instance.PostWait(mgr => ban = mgr.FindIPRangeBan(x => x.Range.Equals(range))).Wait();

            if (ban != null)
                throw new ArgumentException("IP range ban already exists.");

            BanManager.Instance.PostAsync(mgr => mgr.CreateIPRangeBan(range, notes, expiry));
        }
    }
}
