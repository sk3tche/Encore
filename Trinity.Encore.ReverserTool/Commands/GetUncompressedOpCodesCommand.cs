using System;
using System.Linq;
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

        public override bool Execute(CommandArguments args, IPermissible sender)
        {
            var opCode = args.NextUInt16();
            if (opCode == null)
                return false;

            var opCodes = OpCodeUtility.GetOpCodesForCondensedOpCode((int)opCode);
            if (opCodes.Count() == 0)
            {
                Console.WriteLine("No uncompressed opcodes found.");
                return true;
            }

            Console.WriteLine("Found the following opcodes:");
            foreach (var uncompressedOpCode in opCodes)
                Console.WriteLine("{0} ({0:X})", uncompressedOpCode);

            return true;
        }
    }
}
