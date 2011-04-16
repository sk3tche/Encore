using System;
using Trinity.Core;
using Trinity.Core.Security;
using Trinity.Encore.Game.Commands;

namespace Trinity.Encore.ReverserTool.Commands
{
    [Command("Condense", "Compress")]
    public sealed class CompressOpCodeCommand : Command
    {
        public override string Description
        {
            get { return "Condenses a world server opcode."; }
        }

        public override void Execute(CommandArguments args, ICommandUser sender)
        {
            var opCode = args.NextUInt16();
            if (opCode == 0)
            {
                sender.Respond("Invalid opcode given.");
                return;
            }

            var result = OpCodeUtility.CompressOpCode(opCode);
            if (result == null)
            {
                sender.Respond("Could not condense opcode.");
                return;
            }

            sender.Respond("Condensed opcode: {0}".Interpolate(result));
        }
    }
}
