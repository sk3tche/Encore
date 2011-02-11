using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Security.Cryptography;
using Trinity.Core.Collections;
using Trinity.Core.Logging;
using Trinity.Encore.AccountService.Database;
using Trinity.Encore.Game;
using Trinity.Encore.Game.Cryptography;
using Trinity.Encore.Game.Threading;

namespace Trinity.Encore.AccountService.Accounts
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

            var accounts = AccountApplication.Instance.AccountDbContext.FindAll<AccountRecord>();
            foreach (var acc in accounts.Select(account => new Account(account)))
            {
                Contract.Assume(acc != null);
                AddAccount(acc);
            }

            _log.Info("Loaded {0} accounts.", _accounts.Count);
        }

        public static Password CreatePassword(string userName, string password)
        {
            Contract.Requires(!string.IsNullOrEmpty(userName));
            Contract.Requires(userName.Length >= MinNameLength);
            Contract.Requires(userName.Length <= MaxNameLength);
            Contract.Requires(!string.IsNullOrEmpty(password));
            Contract.Requires(password.Length >= MinPasswordLength);
            Contract.Requires(password.Length <= MaxPasswordLength);
            Contract.Ensures(Contract.Result<Password>() != null);

            byte[] sha1;
            byte[] sha256;

            using (var hash = new SHA1Managed())
                sha1 = Password.GenerateCredentialsHash(hash, userName, password, false);

            using (var hash = new SHA256Managed())
                sha256 = Password.GenerateCredentialsHash(hash, userName, password, false);

            Contract.Assume(sha1.Length == Password.SHA1Length);
            Contract.Assume(sha256.Length == Password.SHA256Length);
            return new Password(sha1, sha256);
        }

        public void AddAccount(Account acc)
        {
            Contract.Requires(acc != null);

            _accounts.Add(acc);
        }

        public void RemoveAccount(Account acc)
        {
            Contract.Requires(acc != null);

            _accounts.Remove(acc);
        }

        public Account CreateAccount(string userName, string password, string email, ClientBoxLevel boxLevel = ClientBoxLevel.Cataclysm,
            ClientLocale locale = ClientLocale.English)
        {
            Contract.Requires(!string.IsNullOrEmpty(userName));
            Contract.Requires(userName.Length >= MinNameLength);
            Contract.Requires(userName.Length <= MaxNameLength);
            Contract.Requires(!string.IsNullOrEmpty(password));
            Contract.Requires(password.Length >= MinPasswordLength);
            Contract.Requires(password.Length <= MaxPasswordLength);
            Contract.Requires(!string.IsNullOrEmpty(email));
            Contract.Ensures(Contract.Result<Account>() != null);

            var pw = CreatePassword(userName, password);
            var sha1 = pw.SHA1Password.GetBytes();
            Contract.Assume(sha1.Length == Password.SHA1Length);
            var sha256 = pw.SHA256Password.GetBytes();
            Contract.Assume(sha256.Length == Password.SHA256Length);

            var rec = new AccountRecord(userName, email, sha1, sha256, boxLevel, locale);
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

            return _accounts.Where(predicate).Force();
        }

        public Account FindAccount(Func<Account, bool> predicate)
        {
            Contract.Requires(predicate != null);

            return _accounts.SingleOrDefault(predicate);
        }
    }
}
