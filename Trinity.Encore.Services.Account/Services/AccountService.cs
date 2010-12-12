using System;
using System.Diagnostics.Contracts;
using System.Net;
using Trinity.Encore.Framework.Core.Configuration;
using Trinity.Encore.Framework.Core.Services;
using Trinity.Encore.Framework.Game;
using Trinity.Encore.Framework.Network;
using Trinity.Encore.Framework.Services.Account;
using Trinity.Encore.Services.Account.Accounts;
using Trinity.Encore.Services.Account.Bans;

namespace Trinity.Encore.Services.Account.Services
{
    public sealed class AccountService : IAccountService
    {
        [ConfigurationVariable("ipcUri", "net.tcp://127.0.0.1:9501/Encore.AccountService", Static = true)]
        public static string IpcUri { get; set; }

        public AccountData GetAccount(string username)
        {
            var acc = AccountManager.Instance.FindAccount(x => x.Name == username);
            return acc != null ? acc.Serialize() : null;
        }

        public void CreateAccount(string accountName, string password, string emailAddress, ClientLocale locale, ClientBoxLevel boxLevel)
        {
            if (accountName.Length < AccountManager.MinNameLength || accountName.Length > AccountManager.MaxNameLength)
                throw new ArgumentException();

            if (password.Length < AccountManager.MinPasswordLength || password.Length > AccountManager.MaxPasswordLength)
                throw new ArgumentException();

            AccountManager.Instance.CreateAccount(accountName, password, emailAddress, boxLevel, locale);
        }

        public AccountBanData GetAccountBan(string username)
        {
            var ban = BanManager.Instance.FindAccountBan(x => x.Account.Name == username);
            return ban != null ? ban.Serialize() : null;
        }

        public void CreateAccountBan(string accountName, string notes, DateTime? expiry)
        {
            var acc = AccountManager.Instance.FindAccount(x => x.Name == accountName);
            if (acc == null || acc.Ban != null)
                throw new ArgumentException();

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
                throw new ArgumentException();

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
                throw new ArgumentException();

            BanManager.Instance.CreateIPRangeBan(range, notes, expiry);
        }
    }
}
