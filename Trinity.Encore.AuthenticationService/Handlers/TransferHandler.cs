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
        public static void SendTransferInitiate(IClient client)
        {
            Contract.Requires(client != null);

            using (var packet = new OutgoingAuthPacket(GruntOpCode.TransferInitiate, 1 + 1 + 8 + 16))
            {
                packet.WriteP8String("Patch" ?? "Survey"); // file type
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
