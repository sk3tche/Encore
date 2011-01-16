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

        private ServiceHost<IAccountService> _accountHost;

        protected override void OnStart(string[] args)
        {
            AccountDbContext = new AccountDatabaseContext();
            _accountHost = new ServiceHost<IAccountService>(new Services.AccountService(), Services.AccountService.IpcUri);
        }

        protected override void OnStop()
        {
            _accountHost.Close();
        }
    }
}
