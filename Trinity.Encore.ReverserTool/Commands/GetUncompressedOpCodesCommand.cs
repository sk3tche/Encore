using System;
using System.Linq;
using Trinity.Core;
using Trinity.Core.Security;
using Trinity.Encore.Game.Commands;

namespace Trinity.Encore.ReverserTool.Commands
{
    [Command("Uncompress", "Decondense", "Uncondense")]
    public sealed class GetUncompressedOpCodesCommand : Command
    {
        public override string Description
        {
            get { return "Calculates any uncompressed opcodes for a condensed opcode."; }
        }

        public override void Execute(CommandArguments args, ICommandUser sender)
        {
            var condensedOpCode = args.NextUInt16();
            var opCodes = OpCodeUtility.GetOpCodesForCondensedOpCode(condensedOpCode);

            if (opCodes.Count() == 0)
            {
                sender.Respond("No uncompressed opcodes found.");
                return;
            }

            sender.Respond("Found the following opcodes:");
            foreach (var uncompressedOpCode in opCodes)
                sender.Respond("{0} ({0:X})".Interpolate(uncompressedOpCode));
        }
    }
}
