using Trinity.Encore.Game.Network;
using Trinity.Encore.Game.Network.Handling;
using Trinity.Encore.Game.Network.Transmission;
using Trinity.Network.Connectivity;

namespace Trinity.Encore.AuthenticationService.Network.Handlers.Survey
{
    [AuthenticationPacketHandler(GruntOpCode.SurveyResult)]
    public sealed class SurveyResultHandler : AuthenticationPacketHandler
    {
        public override bool Read(IClient client, IncomingAuthenticationPacket packet)
        {
            packet.ReadInt32Field("Survey Id");
            packet.ReadBooleanField("Success");
            var dataSize = packet.ReadInt16Field("Data Size");
            packet.ReadBytesField("Survey Data", dataSize.Value);

            return true;
        }

        public override void Handle(IClient client)
        {
        }
    }
}
