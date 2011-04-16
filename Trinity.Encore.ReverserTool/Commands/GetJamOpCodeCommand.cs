using System;
using Trinity.Core;
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

        public override void Execute(CommandArguments args, ICommandUser sender)
        {
            var opCode = args.NextUInt16();
            if (opCode == 0)
            {
                sender.Respond("Invalid opcode given.");
                return;
            }

            var jamClient = OpCodeUtility.GetJamClientOpCode(opCode);
            sender.Respond(jamClient != null ? "JAM client opcode: {0} ({0:X})".Interpolate(jamClient) :
                "Could not calculate JAM client opcode.");

            var jamClientConn = OpCodeUtility.GetJamClientConnectionOpCode(opCode);
            sender.Respond(jamClientConn != null ? "JAM client connection opcode: {0} ({0:X})".Interpolate(jamClientConn) :
                "Could not calculate JAM client connection opcode.");
        }
    }
}
