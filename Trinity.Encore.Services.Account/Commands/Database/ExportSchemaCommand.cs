using System;
using Trinity.Encore.Framework.Core.Security;
using Trinity.Encore.Framework.Game.Commands;
using Trinity.Encore.Framework.Game.Security;

namespace Trinity.Encore.Services.Account.Commands.Database
{
    [Command("ExportSchema", "ExportDbSchema", "SchemaExport", "DbSchemaExport")]
    public sealed class ExportSchemaCommand : Command
    {
        public override string Description
        {
            get { return "Exports the database schema to a file."; }
        }

        public override Type RequiredPermission
        {
            get { return typeof(ConsolePermission); }
        }

        public override bool Execute(CommandArguments args, IPermissible sender)
        {
            var fileName = args.NextString();
            if (string.IsNullOrEmpty(fileName))
                return false;

            AccountApplication.Instance.AccountDbContext.Post(x => x.Schema.Export(fileName));

            return true;
        }
    }
}
