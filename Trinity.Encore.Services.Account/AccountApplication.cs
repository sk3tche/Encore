using Trinity.Encore.Framework.Core.Services;
using Trinity.Encore.Framework.Game.Threading;
using Trinity.Encore.Framework.Services.Account;
using Trinity.Encore.Services.Account.Database.Implementation;
using Trinity.Encore.Services.Account.Services;

namespace Trinity.Encore.Services.Account
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
            _accountHost = new ServiceHost<IAccountService>(new AccountService(), AccountService.IpcUri);
        }

        protected override void OnStop()
        {
            _accountHost.Close();
        }
    }
}
