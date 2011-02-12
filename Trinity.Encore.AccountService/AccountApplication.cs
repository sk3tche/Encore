using System.Diagnostics.Contracts;
using Trinity.Core.Configuration;
using Trinity.Core.Services;
using Trinity.Encore.AccountService.Database.Implementation;
using Trinity.Encore.Game.Threading;
using Trinity.Encore.Services.Account;

namespace Trinity.Encore.AccountService
{
    public sealed class AccountApplication : ActorApplication<AccountApplication>
    {
        private AccountApplication()
        {
        }

        public AccountDatabaseContext AccountDbContext { get; private set; }

        private ServiceHost<IAccountService, Services.AccountService> _accountHost;

        protected override void OnStart(string[] args)
        {
            if (string.IsNullOrWhiteSpace(Services.AccountService.IpcUri))
                throw new ConfigurationValueException("Invalid IPC URI string.");

            AccountDbContext = new AccountDatabaseContext();

            Contract.Assume(!string.IsNullOrEmpty(Services.AccountService.IpcUri));
            _accountHost = new ServiceHost<IAccountService, Services.AccountService>(new Services.AccountService(), Services.AccountService.IpcUri);
            _accountHost.Open();
        }

        protected override void OnStop()
        {
            _accountHost.Close();
        }
    }
}
