using Trinity.Encore.Framework.Core.Services;
using Trinity.Encore.Framework.Game.Services;
using Trinity.Encore.Framework.Game.Threading;
using Trinity.Encore.Framework.Services;
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

        private ServiceHost<IAccountService> _service;

        protected override void OnStart(string[] args)
        {
            AccountDbContext = new AccountDatabaseContext();
            _service = new ServiceHost<IAccountService>(new AccountService(), AccountService.IpcUri);
        }

        protected override void OnStop()
        {
            AccountDbContext.Dispose();
            _service.Close();
        }
    }
}
