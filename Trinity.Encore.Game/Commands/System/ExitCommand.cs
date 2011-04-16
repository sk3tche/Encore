using System;
using Trinity.Core.Security;
using Trinity.Encore.Game.Security;

namespace Trinity.Encore.Game.Commands.System
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

        public override void Execute(CommandArguments args, ICommandUser sender)
        {
            CommandConsole.StopConsole = true;
        }
    }
}
