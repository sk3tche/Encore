using Trinity.Encore.Framework.Game.Network;
using Trinity.Encore.Framework.Game.Network.Handling;
using Trinity.Encore.Framework.Game.Network.Transmission;
using Trinity.Encore.Framework.Network.Connectivity;

namespace Trinity.Encore.Services.World.Handlers
{
    public static class ConnectionHandler
    {
        [WorldPacketHandler(WorldServerOpCodes.ClientConnectionPing)]
        public static void HandlePing(IClient client, IncomingWorldPacket packet)
        {
            var latency = packet.ReadInt32();
            var sequence = packet.ReadInt32();

            SendPong(client, sequence);
        }

        public static void SendPong(IClient client, int sequence)
        {
            using (var packet = new OutgoingWorldPacket(WorldServerOpCodes.ServerConnectionPong, 4))
            {
                packet.Write(sequence);

                client.Send(packet);
            }
        }
    }
}
