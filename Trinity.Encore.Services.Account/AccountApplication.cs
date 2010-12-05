using System;
using System.Diagnostics;
using FluentNHibernate.Cfg;
using Trinity.Encore.Framework.Game.Threading;
using Trinity.Encore.Services.Account.Database;
using Trinity.Encore.Services.Account.Database.Implementation;

namespace Trinity.Encore.Services.Account
{
    public sealed class AccountApplication : ActorApplication<AccountApplication>
    {
        private AccountApplication()
        {
        }

        public AccountDatabaseContext AccountDbContext { get; private set; }

        protected override void OnStart(string[] args)
        {
            AccountDbContext = new AccountDatabaseContext();
        }

        protected override void OnStop()
        {
            AccountDbContext.Dispose();
        }
    }
}
