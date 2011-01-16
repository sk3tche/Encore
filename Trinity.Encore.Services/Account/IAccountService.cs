using System;
using System.Diagnostics.Contracts;
using System.Net;
using System.Net.Security;
using System.ServiceModel;
using Trinity.Encore.Game;
using Trinity.Network;

namespace Trinity.Encore.Services.Account
{
    [ContractClass(typeof(AccountServiceContracts))]
    [ServiceContract(ProtectionLevel = ProtectionLevel.None, SessionMode = SessionMode.Required, CallbackContract = typeof(IEmptyCallbackService))]
    public interface IAccountService
    {
        [OperationContract]
        AccountData GetAccount(string userName);

        [OperationContract]
        void CreateAccount(string accountName, string password, string emailAddress, ClientLocale locale = ClientLocale.English,
            ClientBoxLevel boxLevel = ClientBoxLevel.Cataclysm);

        [OperationContract]
        void SetLastIP(string userName, IPAddress ip);

        [OperationContract]
        void SetLastLogin(string userName, DateTime time);

        [OperationContract]
        AccountBanData GetAccountBan(string userName);

        [OperationContract]
        void CreateAccountBan(string accountName, string notes = null, DateTime? expiry = null);

        [OperationContract]
        IPBanData GetIPBan(IPAddress address);

        [OperationContract]
        void CreateIPBan(IPAddress address, string notes = null, DateTime? expiry = null);

        [OperationContract]
        IPRangeBanData GetIPRangeBan(IPAddress address);

        [OperationContract]
        void CreateIPRangeBan(IPAddressRange range, string notes = null, DateTime? expiry = null);
    }

    [ContractClassFor(typeof(IAccountService))]
    public abstract class AccountServiceContracts : IAccountService
    {
        public AccountData GetAccount(string userName)
        {
            Contract.Requires(!string.IsNullOrEmpty(userName));

            return null;
        }

        public void CreateAccount(string accountName, string password, string emailAddress, ClientLocale locale, ClientBoxLevel boxLevel)
        {
            Contract.Requires(!string.IsNullOrEmpty(accountName));
            Contract.Requires(!string.IsNullOrEmpty(password));
            Contract.Requires(!string.IsNullOrEmpty(emailAddress));
        }

        public void SetLastIP(string userName, IPAddress ip)
        {
            Contract.Requires(!string.IsNullOrEmpty(userName));
        }

        public void SetLastLogin(string userName, DateTime time)
        {
            Contract.Requires(!string.IsNullOrEmpty(userName));
        }

        public AccountBanData GetAccountBan(string userName)
        {
            Contract.Requires(!string.IsNullOrEmpty(userName));

            return null;
        }

        public void CreateAccountBan(string accountName, string notes, DateTime? expiry)
        {
            Contract.Requires(!string.IsNullOrEmpty(accountName));
        }

        public IPBanData GetIPBan(IPAddress address)
        {
            Contract.Requires(address != null);

            return null;
        }

        public void CreateIPBan(IPAddress address, string notes, DateTime? expiry)
        {
            Contract.Requires(address != null);
        }

        public IPRangeBanData GetIPRangeBan(IPAddress address)
        {
            Contract.Requires(address != null);

            return null;
        }

        public void CreateIPRangeBan(IPAddressRange range, string notes, DateTime? expiry)
        {
            Contract.Requires(range != null);
        }
    }
}
