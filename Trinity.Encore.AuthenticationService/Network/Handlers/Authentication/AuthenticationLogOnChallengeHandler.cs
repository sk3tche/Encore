using Trinity.Encore.Game.Network;
using Trinity.Encore.Game.Network.Handling;
using Trinity.Encore.Game.Network.Transmission;
using Trinity.Network.Connectivity;

namespace Trinity.Encore.AuthenticationService.Network.Handlers.Authentication
{
    [AuthenticationPacketHandler(GruntOpCode.AuthenticationLogOnChallenge)]
    public sealed class AuthenticationLogOnChallengeHandler : AuthenticationPacketHandler
    {
        public override bool Read(IClient client, IncomingAuthenticationPacket packet)
        {
            packet.ReadByteField("Unknown"); // Always 8
            packet.ReadInt16Field("Packet Size");
            packet.ReadFourCCField("Game Name");
            packet.ReadByteField("Major");
            packet.ReadByteField("Minor");
            packet.ReadByteField("Revision");
            packet.ReadInt16Field("Build");
            packet.ReadFourCCField("Platform");
            packet.ReadFourCCField("Operating System");
            packet.ReadFourCCField("Locale");
            packet.ReadInt32Field("Time Zone");
            packet.ReadIPAddressField("Client Address", false);
            packet.ReadP8StringField("Account Name");

            return true;
        }

        public override void Handle(IClient client)
        {
        }
    }
}
