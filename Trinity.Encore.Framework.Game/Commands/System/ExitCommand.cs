using System;
using Trinity.Encore.Framework.Core.Security;
using Trinity.Encore.Framework.Game.Security;

namespace Trinity.Encore.Framework.Game.Commands.System
{
    [Command("Exit", "Close", "Bye", "Shutdown", "Die", "Kill")]
    public sealed class ExitCommand : Command
    {
        public override string Description
        {
            get { return "Tears down the process gracefully."; }
        }

        public override Type RequiredPermission
        {
            get { return typeof(ConsolePermission); }
        }

        public override bool Execute(CommandArguments args, IPermissible sender)
        {
            CommandConsole.StopConsole = true;
            return true;
        }
    }
}
