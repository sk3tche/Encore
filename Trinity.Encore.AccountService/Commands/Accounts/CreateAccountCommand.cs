using System;
using System.Text;
using Trinity.Core;
using Trinity.Core.Security;
using Trinity.Encore.AccountService.Accounts;
using Trinity.Encore.Game;
using Trinity.Encore.Game.Commands;
using Trinity.Encore.Game.Security;

namespace Trinity.Encore.AccountService.Commands.Accounts
{
    [Command("CreateAccount", "AccountCreate", "AccCreate", "CreateAcc")]
    public sealed class CreateAccountCommand : Command
    {
        public override string Description
        {
            get { return "Creates and saves an account."; }
        }

        public override Type RequiredPermission
        {
            get { return typeof(RootPermission); }
        }

        public override void Execute(CommandArguments args, ICommandUser sender)
        {
            var name = args.NextString();
            var password = args.NextString();
            var email = args.NextString();
            var box = args.NextEnum<ClientBoxLevel>(ClientBoxLevel.Cataclysm);
            var locale = args.NextEnum<ClientLocale>(ClientLocale.English);

            if (string.IsNullOrEmpty(name))
            {
                sender.Respond("No name given.");
                return;
            }

            if (name.Length < AccountManager.MinNameLength || name.Length > AccountManager.MaxNameLength)
            {
                sender.Respond("Name must be between {0} and {1} characters long.".Interpolate(AccountManager.MinNameLength,
                    AccountManager.MaxNameLength));
                return;
            }

            if (string.IsNullOrEmpty(password))
            {
                sender.Respond("No password given.");
                return;
            }

            if (password.Length < AccountManager.MinPasswordLength || password.Length > AccountManager.MaxPasswordLength)
            {
                sender.Respond("Password must be between {0} and {1} characters long.".Interpolate(AccountManager.MinPasswordLength,
                    AccountManager.MaxPasswordLength));
                return;
            }

            if (string.IsNullOrEmpty(email))
            {
                sender.Respond("No email given.");
                return;
            }

            AccountManager.Instance.PostAsync(x => x.CreateAccount(name, password, email, box, locale));
        }
    }
}
