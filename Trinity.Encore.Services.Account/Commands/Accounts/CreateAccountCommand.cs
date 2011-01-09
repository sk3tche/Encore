using System;
using Trinity.Encore.Framework.Core.Security;
using Trinity.Encore.Framework.Game;
using Trinity.Encore.Framework.Game.Commands;
using Trinity.Encore.Framework.Game.Security;
using Trinity.Encore.Services.Account.Accounts;

namespace Trinity.Encore.Services.Account.Commands.Accounts
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

        public override bool Execute(CommandArguments args, IPermissible sender)
        {
            var name = args.NextString();
            if (string.IsNullOrEmpty(name))
                return false;

            if (name.Length < AccountManager.MinNameLength || name.Length > AccountManager.MaxNameLength)
                return false;

            var password = args.NextString();
            if (string.IsNullOrEmpty(password))
                return false;

            if (password.Length < AccountManager.MinPasswordLength || password.Length > AccountManager.MaxPasswordLength)
                return false;

            var email = args.NextString();
            if (string.IsNullOrEmpty(email))
                return false;

            var box = args.NextEnum<ClientBoxLevel>() ?? ClientBoxLevel.Cataclysm;
            var locale = args.NextEnum<ClientLocale>() ?? ClientLocale.English;

            AccountManager.Instance.CreateAccount(name, password, email, box, locale);
            return true;
        }
    }
}
