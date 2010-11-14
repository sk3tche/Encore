using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Security;
using System.ServiceModel;
using Trinity.Encore.Framework.Game;
using Trinity.Encore.Framework.Game.Services;

namespace Trinity.Encore.Framework.Services.Account
{
    [ServiceContract(ProtectionLevel = ProtectionLevel.None, SessionMode = SessionMode.Required)]
    public interface IAccountService : IAuthenticatableService
    {
        [OperationContract(IsInitiating = false)]
        AccountRecord GetAccount(Func<AccountRecord, bool> predicate);

        [OperationContract(IsInitiating = false)]
        List<AccountRecord> GetAccounts(Func<AccountRecord, bool> predicate);

        [OperationContract(IsInitiating = false)]
        void CreateAccount(string accountName, string password, string emailAddress, ClientLocale locale = ClientLocale.English,
            ClientBoxLevel boxLevel = ClientBoxLevel.Cataclysm);

        [OperationContract(IsInitiating = false)]
        AccountBanData GetAccountBan(Func<AccountBanData, bool> predicate);

        [OperationContract(IsInitiating = false)]
        List<AccountBanData> GetAccountBans(Func<AccountBanData, bool> predicate);

        [OperationContract(IsInitiating = false)]
        void CreateAccountBan(string accountName, string notes, DateTime? expiry);

        [OperationContract(IsInitiating = false)]
        IPBanData GetIPBan(Func<IPBanData, bool> predicate);

        [OperationContract(IsInitiating = false)]
        List<IPBanData> GetIPBans(Func<IPBanData, bool> predicate);

        [OperationContract(IsInitiating = false)]
        void CreateIPBan(IPAddress address, string notes, DateTime? expiry);
        
        [OperationContract(IsInitiating = false)]
        IPRangeBanData GetIPRangeBan(Func<IPRangeBanData, bool> predicate);

        [OperationContract(IsInitiating = false)]
        List<IPRangeBanData> GetIPRangeBans(Func<IPRangeBanData, bool> predicate);

        [OperationContract(IsInitiating = false)]
        void CreateIPRangeBan(IPAddress address, string notes, DateTime? expiry);
    }
}
