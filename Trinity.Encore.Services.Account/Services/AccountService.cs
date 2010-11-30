using System;
using System.Collections.Generic;
using System.Net;
using Trinity.Encore.Framework.Core.Cryptography;
using Trinity.Encore.Framework.Game;
using Trinity.Encore.Framework.Services.Account;

namespace Trinity.Encore.Services.Account.Services
{
    public sealed class AccountService : IAccountService
    {
        public void Authenticate(string username, BigInteger password)
        {
            throw new NotImplementedException();
        }

        public AccountData GetAccount(Func<AccountData, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public List<AccountData> GetAccounts(Func<AccountData, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public void CreateAccount(string accountName, string password, string emailAddress,
            ClientLocale locale = ClientLocale.English, ClientBoxLevel boxLevel = ClientBoxLevel.Cataclysm)
        {
            throw new NotImplementedException();
        }

        public AccountBanData GetAccountBan(Func<AccountBanData, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public List<AccountBanData> GetAccountBans(Func<AccountBanData, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public void CreateAccountBan(string accountName, string notes, DateTime? expiry)
        {
            throw new NotImplementedException();
        }

        public IPBanData GetIPBan(Func<IPBanData, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public List<IPBanData> GetIPBans(Func<IPBanData, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public void CreateIPBan(IPAddress address, string notes, DateTime? expiry)
        {
            throw new NotImplementedException();
        }

        public IPRangeBanData GetIPRangeBan(Func<IPRangeBanData, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public List<IPRangeBanData> GetIPRangeBans(Func<IPRangeBanData, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public void CreateIPRangeBan(IPAddress address, string notes, DateTime? expiry)
        {
            throw new NotImplementedException();
        }
    }
}
