using System;
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

        public override bool Execute(CommandArguments args, IPermissible sender)
        {
            var opCode = args.NextUInt16();
            if (opCode == null || (int)opCode <= 0)
                return false;

            var result = OpCodeUtility.CompressOpCode((int)opCode);
            if (result == null)
            {
                Console.WriteLine("Could not condense opcode.");
                return true;
            }

            Console.WriteLine("Condensed opcode: {0}", result);
            return true;
        }
    }
}
