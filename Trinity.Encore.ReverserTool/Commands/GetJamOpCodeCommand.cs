using System;
using Trinity.Core.Security;
using Trinity.Encore.Game.Commands;

namespace Trinity.Encore.ReverserTool.Commands
{
    [Command("GetJam", "ToJam")]
    public sealed class GetJamOpCodeCommand : Command
    {
        public override string Description
        {
            get { return "Calculates the JAM opcode(s) for an opcode."; }
        }

        public override bool Execute(CommandArguments args, IPermissible sender)
        {
            var opCode = args.NextUInt16();
            if (opCode == null || (int)opCode <= 0)
                return false;

            var opc = (int)opCode;
            var jamClient = OpCodeUtility.GetJamClientOpCode(opc);
            if (jamClient != null)
                Console.WriteLine("JAM client opcode: {0} ({0:X})", jamClient);
            else
                Console.WriteLine("Could not calculate JAM client opcode.");

            var jamClientConn = OpCodeUtility.GetJamClientConnectionOpCode(opc);
            if (jamClientConn != null)
                Console.WriteLine("JAM client connection opcode: {0} ({0:X})", jamClientConn);
            else
                Console.WriteLine("Could not calculate JAM client connection opcode.");

            return true;
        }
    }
}
