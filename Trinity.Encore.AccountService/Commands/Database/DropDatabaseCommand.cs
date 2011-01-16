using System;
using System.Globalization;
using System.Linq;
using Trinity.Core.Security;
using Trinity.Encore.Game.Commands;
using Trinity.Encore.Game.Security;

namespace Trinity.Encore.AccountService.Commands.Database
{
    [Command("DropDb", "DbDrop")]
    public sealed class DropDatabaseCommand : Command
    {
        public override string Description
        {
            get { return "Drops the database schema and all contained data."; }
        }

        public override Type RequiredPermission
        {
            get { return typeof(ConsolePermission); }
        }

        public override bool Execute(CommandArguments args, IPermissible sender)
        {
            Console.WriteLine("Executing this command will permanently drop the entire database. Continue? (Y/N)");

            var answer = Console.ReadLine().ToUpper(CultureInfo.InvariantCulture).ToCharArray().SingleOrDefault();
            if (answer == 'Y')
                AccountApplication.Instance.AccountDbContext.Post(x => x.Schema.Drop());

            return true;
        }
    }
}
