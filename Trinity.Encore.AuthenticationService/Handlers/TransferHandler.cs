using System.Diagnostics.Contracts;
using Trinity.Core.IO;
using Trinity.Encore.Game.Network;
using Trinity.Encore.Game.Network.Handling;
using Trinity.Encore.Game.Network.Transmission;
using Trinity.Network.Connectivity;

namespace Trinity.Encore.AuthenticationService.Handlers
{
    public static class TransferHandler
    {
        [AuthPacketHandler(GruntOpCode.TransferComplete)]
        public static void HandleTransferComplete(IClient client, IncomingAuthPacket packet)
        {
            Contract.Requires(client != null);
            Contract.Requires(packet != null);
        }

        [AuthPacketHandler(GruntOpCode.TransferResume)]
        public static void HandleTransferResume(IClient client, IncomingAuthPacket packet)
        {
            Contract.Requires(client != null);
            Contract.Requires(packet != null);

            packet.ReadUInt64(); // file position
        }

        [AuthPacketHandler(GruntOpCode.TransferCancel)]
        public static void HandleTransferCancel(IClient client, IncomingAuthPacket packet)
        {
            Contract.Requires(client != null);
            Contract.Requires(packet != null);
        }

        public static void SendTransferInitiate(IClient client)
        {
            Contract.Requires(client != null);

            using (var packet = new OutgoingAuthPacket(GruntOpCode.TransferInitiate, 1 + 1 + 8 + 16))
            {
                packet.Write((byte)0); // file type length
                packet.Write(new byte[0]); // file type, "Patch" or "Survey", not a C string
                packet.Write((ulong)0); // file length
                packet.Write(new byte[16]); // md5 of the file contents

                client.Send(packet);
            }
        }

        public static void SendTransferData(IClient client)
        {
            Contract.Requires(client != null);

            using (var packet = new OutgoingAuthPacket(GruntOpCode.TransferData, 2))
            {
                packet.Write((ushort)0); // chunk length
                packet.Write(new byte[0]); // chunk data
            }
        }
    }
}
