using System.Diagnostics.Contracts;
using Trinity.Core.IO;
using Trinity.Encore.Game.Network;
using Trinity.Encore.Game.Network.Transmission;

namespace Trinity.Encore.AuthenticationService.Network.Packets
{
    public static class TransferPackets
    {
        public static OutgoingAuthenticationPacket BuildTransferInitiate(long fileLength, byte[] fileHash, bool patch = true)
        {
            Contract.Requires(fileLength >= 0);
            Contract.Requires(fileHash != null);
            Contract.Requires(fileHash.Length == 16);
            Contract.Ensures(Contract.Result<OutgoingAuthenticationPacket>() != null);

            var packet = new OutgoingAuthenticationPacket(GruntOpCode.TransferInitiate);

            packet.WriteP8String(patch ? "Patch" : "Survey");
            packet.Write(fileLength);
            packet.Write(fileHash);

            return packet;
        }

        public static OutgoingAuthenticationPacket BuildTransferData(short chunkLength, byte[] data)
        {
            Contract.Requires(chunkLength >= 0);
            Contract.Requires(data != null);
            Contract.Requires(data.Length == chunkLength);
            Contract.Ensures(Contract.Result<OutgoingAuthenticationPacket>() != null);

            var packet = new OutgoingAuthenticationPacket(GruntOpCode.TransferData);

            packet.Write(chunkLength);
            packet.Write(data);

            return packet;
        }
    }
}
