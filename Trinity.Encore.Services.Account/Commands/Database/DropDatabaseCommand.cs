using System;
using System.Linq;
using Trinity.Encore.Framework.Core.Security;
using Trinity.Encore.Framework.Game.Commands;
using Trinity.Encore.Framework.Game.Security;

namespace Trinity.Encore.Services.Account.Commands.Database
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

            var answer = Console.ReadLine().ToUpper().ToCharArray().SingleOrDefault();
            if (answer == 'Y')
                AccountApplication.Instance.AccountDbContext.Post(x => x.Schema.Drop());

            return true;
        }
    }
}
