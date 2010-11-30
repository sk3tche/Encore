using System;
using System.Diagnostics;
using FluentNHibernate.Cfg;
using Trinity.Encore.Framework.Game.Threading;
using Trinity.Encore.Services.Account.Database;

namespace Trinity.Encore.Services.Account
{
    public sealed class AccountApplication : ActorApplication<AccountApplication>
    {
        private AccountApplication()
        {
        }

        public static AccountDatabaseContext DbContext { get; private set; }

        protected override void OnStart(string[] args)
        {
            DbContext = new AccountDatabaseContext();
        }

        protected override void OnStop()
        {
            DbContext.Dispose();
            DbContext = null;
        }
    }
}
