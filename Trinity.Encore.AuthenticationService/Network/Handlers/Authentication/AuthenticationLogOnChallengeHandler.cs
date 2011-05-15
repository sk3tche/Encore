using System;
using System.Reflection;
using Trinity.Encore.Game;
using Trinity.Encore.Game.Network;
using Trinity.Encore.Game.Network.Handling;
using Trinity.Encore.Game.Network.Transmission;
using Trinity.Network;
using Trinity.Network.Connectivity;

namespace Trinity.Encore.AuthenticationService.Network.Handlers.Authentication
{
    [AuthenticationPacketHandler(GruntOpCode.AuthenticationLogOnChallenge)]
    public sealed class AuthenticationLogOnChallengeHandler : AuthenticationPacketHandler
    {
        public ClientType GameType { get; private set; }

        public Version ClientVersion { get; private set; }

        public ProcessorArchitecture Processor { get; private set; }

        public PlatformID OperatingSystem { get; private set; }

        public ClientLocale Locale { get; private set; }

        public int TimeZone { get; private set; }

        public string AccountName { get; private set; }

        public override bool Read(IClient client, IncomingAuthenticationPacket packet)
        {
            packet.ReadByteField("Protocol Version?"); // Always 8
            var packetSize = packet.ReadInt16Field("Packet Size");
            var clientType = packet.ReadFourCCField("Client Type");
            var clientVersion = new Version(
                packet.ReadByteField("Major"),
                packet.ReadByteField("Minor"),
                packet.ReadByteField("Revision"),
                packet.ReadInt16Field("Build"));
            var processor = packet.ReadFourCCField("Processor");
            var operatingSystem = packet.ReadFourCCField("Operating System");
            var locale = packet.ReadFourCCField("Locale");
            var timeZone = packet.ReadInt32Field("Time Zone");
            var clientAddress = packet.ReadIPAddressField("Client Address", false);
            var accountName = packet.ReadP8StringField("Account Name");

            var expectedSize = (short)(packet.Length - sizeof(byte) - sizeof(short));
            if (packetSize != expectedSize)
                return InvalidValue(client, packetSize, expectedSize);

            var clientTypeEnum = GameUtility.GetClientTypeFromFourCC(clientType);
            if (clientTypeEnum == null)
                return InvalidValue(client, clientType);

            var processorEnum = GameUtility.GetProcessorFromFourCC(processor);
            if (processorEnum == null)
                return InvalidValue(client, processor);

            var operatingSystemEnum = GameUtility.GetPlatformFromFourCC(operatingSystem);
            if (operatingSystemEnum == null)
                return InvalidValue(client, operatingSystem);

            var localeEnum = GameUtility.GetClientLocaleFromFourCC(locale);
            if (localeEnum == null)
                return InvalidValue(client, locale);

            var expectedAddress = client.EndPoint.ToIPEndPoint().Address;
            if (clientAddress.Value.Equals(expectedAddress))
                return InvalidValue(client, clientAddress, expectedAddress);

            GameType = (ClientType)clientTypeEnum;
            ClientVersion = clientVersion;
            Processor = (ProcessorArchitecture)processorEnum;
            OperatingSystem = (PlatformID)operatingSystemEnum;
            Locale = (ClientLocale)localeEnum;
            TimeZone = timeZone;
            AccountName = accountName;

            return true;
        }

        public override void Handle(IClient client)
        {
        }
    }
}
