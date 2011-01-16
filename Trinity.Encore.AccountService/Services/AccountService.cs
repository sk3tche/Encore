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
        [ConfigurationVariable("ipcUri", "net.tcp://127.0.0.1:9501/Encore.AccountService", Static = true)]
        [SuppressMessage("Microsoft.Design", "CA1056", Justification = "This is a configuration variable.")]
        public static string IpcUri { get; set; }

        public AccountData GetAccount(string userName)
        {
            var acc = AccountManager.Instance.FindAccount(x => x.Name == userName);
            return acc != null ? acc.Serialize() : null;
        }

        public void CreateAccount(string accountName, string password, string emailAddress, ClientLocale locale, ClientBoxLevel boxLevel)
        {
            if (accountName.Length < AccountManager.MinNameLength || accountName.Length > AccountManager.MaxNameLength)
                throw new ArgumentException("Account name has an invalid length.");

            if (password.Length < AccountManager.MinPasswordLength || password.Length > AccountManager.MaxPasswordLength)
                throw new ArgumentException("Password has an invalid length.");

            AccountManager.Instance.CreateAccount(accountName, password, emailAddress, boxLevel, locale);
        }

        public void SetLastIP(string userName, IPAddress ip)
        {
            var acc = AccountManager.Instance.FindAccount(x => x.Name == userName);
            if (acc != null)
                acc.LastIP = ip;
        }

        public void SetLastLogin(string userName, DateTime time)
        {
            var acc = AccountManager.Instance.FindAccount(x => x.Name == userName);
            if (acc != null)
                acc.LastLogin = time;
        }

        public AccountBanData GetAccountBan(string userName)
        {
            var ban = BanManager.Instance.FindAccountBan(x => x.Account.Name == userName);
            return ban != null ? ban.Serialize() : null;
        }

        public void CreateAccountBan(string accountName, string notes, DateTime? expiry)
        {
            var acc = AccountManager.Instance.FindAccount(x => x.Name == accountName);

            if (acc == null)
                throw new ArgumentException("No account found.");

            if (acc.Ban != null)
                throw new ArgumentException("Account ban already exists.");

            BanManager.Instance.CreateAccountBan(acc, notes, expiry);
        }

        public IPBanData GetIPBan(IPAddress address)
        {
            var ban = BanManager.Instance.FindIPBan(x => x.Address.Equals(address));
            return ban != null ? ban.Serialize() : null;
        }

        public void CreateIPBan(IPAddress address, string notes, DateTime? expiry)
        {
            var ban = BanManager.Instance.FindIPBan(x => x.Equals(address));

            if (ban != null)
                throw new ArgumentException("IP ban already exists.");

            BanManager.Instance.CreateIPBan(address, notes, expiry);
        }

        public IPRangeBanData GetIPRangeBan(IPAddress address)
        {
            var ban = BanManager.Instance.FindIPRangeBan(x => x.Range.IsInRange(address));
            return ban != null ? ban.Serialize() : null;
        }

        public void CreateIPRangeBan(IPAddressRange range, string notes, DateTime? expiry)
        {
            var ban = BanManager.Instance.FindIPRangeBan(x => x.Range.Equals(range));

            if (ban != null)
                throw new ArgumentException("IP range ban already exists.");

            BanManager.Instance.CreateIPRangeBan(range, notes, expiry);
        }
    }
}
