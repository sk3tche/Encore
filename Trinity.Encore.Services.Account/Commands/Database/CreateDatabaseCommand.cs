using System;
using System.Globalization;
using System.Linq;
using Trinity.Encore.Framework.Core.Security;
using Trinity.Encore.Framework.Game.Commands;
using Trinity.Encore.Framework.Game.Security;

namespace Trinity.Encore.Services.Account.Commands.Database
{
    [Command("CreateDb", "RecreateDb", "DbCreate", "DbRecreate")]
    public sealed class CreateDatabaseCommand : Command
    {
        public override string Description
        {
            get { return "Creates (or recreates) the database schema."; }
        }

        public override Type RequiredPermission
        {
            get { return typeof(ConsolePermission); }
        }

        public override bool Execute(CommandArguments args, IPermissible sender)
        {
            Console.WriteLine("Executing this command will permanently overwrite the entire database. Continue? (Y/N)");

            var answer = Console.ReadLine().ToUpper(CultureInfo.InvariantCulture).ToCharArray().SingleOrDefault();
            if (answer == 'Y')
                AccountApplication.Instance.AccountDbContext.Post(x => x.Schema.Create());

            return true;
        }
    }
}
