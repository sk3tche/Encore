using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Security.Cryptography;
using Trinity.Encore.Framework.Core.Collections;
using Trinity.Encore.Framework.Core.Logging;
using Trinity.Encore.Framework.Game;
using Trinity.Encore.Framework.Game.Cryptography;
using Trinity.Encore.Framework.Game.Threading;
using Trinity.Encore.Services.Account.Database;

namespace Trinity.Encore.Services.Account.Accounts
{
    public sealed class AccountManager : SingletonActor<AccountManager>
    {
        public const int MinNameLength = 3;

        public const int MaxNameLength = 20;

        public const int MinPasswordLength = 3;

        public const int MaxPasswordLength = 16;

        private static readonly LogProxy _log = new LogProxy("AccountManager");

        private readonly List<Account> _accounts = new List<Account>();

        [ContractInvariantMethod]
        private void Invariant()
        {
            Contract.Invariant(_accounts != null);
        }

        private AccountManager()
        {
            _log.Info("Loading accounts...");

            foreach (var acc in AccountApplication.Instance.AccountDbContext.FindAll<AccountRecord>().Select(account => new Account(account)))
                AddAccount(acc);

            _log.Info("Loaded {0} accounts.", _accounts.Count);
        }

        public static Password CreatePassword(string username, string password)
        {
            Contract.Requires(!string.IsNullOrEmpty(username));
            Contract.Requires(username.Length >= MinNameLength);
            Contract.Requires(username.Length <= MaxNameLength);
            Contract.Requires(!string.IsNullOrEmpty(password));
            Contract.Requires(password.Length >= MinPasswordLength);
            Contract.Requires(password.Length <= MaxPasswordLength);
            Contract.Ensures(Contract.Result<Password>() != null);

            byte[] sha1;
            byte[] sha256;

            using (var hash = new SHA1Managed())
                sha1 = Password.GenerateCredentialsHash(hash, username, password, false);

            using (var hash = new SHA256Managed())
                sha256 = Password.GenerateCredentialsHash(hash, username, password, false);

            Contract.Assume(sha1.Length == Password.SHA1Length);
            Contract.Assume(sha256.Length == Password.SHA256Length);
            return new Password(sha1, sha256);
        }

        public void AddAccount(Account acc)
        {
            Contract.Requires(acc != null);

            lock (_accounts)
                _accounts.Add(acc);
        }

        public void RemoveAccount(Account acc)
        {
            Contract.Requires(acc != null);

            lock (_accounts)
                _accounts.Remove(acc);
        }

        public Account CreateAccount(string username, string password, string email, ClientBoxLevel boxLevel = ClientBoxLevel.Cataclysm,
            ClientLocale locale = ClientLocale.English)
        {
            Contract.Requires(!string.IsNullOrEmpty(username));
            Contract.Requires(username.Length >= MinNameLength);
            Contract.Requires(username.Length <= MaxNameLength);
            Contract.Requires(!string.IsNullOrEmpty(password));
            Contract.Requires(password.Length >= MinPasswordLength);
            Contract.Requires(password.Length <= MaxPasswordLength);
            Contract.Requires(!string.IsNullOrEmpty(email));
            Contract.Ensures(Contract.Result<Account>() != null);

            var pw = CreatePassword(username, password);
            var sha1 = pw.SHA1Password.GetBytes();
            Contract.Assume(sha1.Length == Password.SHA1Length);
            var sha256 = pw.SHA256Password.GetBytes();
            Contract.Assume(sha256.Length == Password.SHA256Length);

            var rec = new AccountRecord(username, email, sha1, sha256, boxLevel, locale);
            rec.Create();

            var acc = new Account(rec);
            AddAccount(acc);
            return acc;
        }

        public void DeleteAccount(Account acc)
        {
            Contract.Requires(acc != null);

            acc.Delete();
            RemoveAccount(acc);
        }

        public IEnumerable<Account> FindAccounts(Func<Account, bool> predicate)
        {
            Contract.Requires(predicate != null);
            Contract.Ensures(Contract.Result<IEnumerable<Account>>() != null);

            lock (_accounts)
                return _accounts.Where(predicate).Force();
        }

        public Account FindAccount(Func<Account, bool> predicate)
        {
            Contract.Requires(predicate != null);

            lock (_accounts)
                return _accounts.SingleOrDefault(predicate);
        }
    }
}
