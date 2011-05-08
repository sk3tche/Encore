using System.Diagnostics.Contracts;
using System.Net;
using Trinity.Core;
using Trinity.Core.Logging;
using Trinity.Encore.Game.Network.Transmission;
using Trinity.Network.Connectivity;
using Trinity.Network.Transmission;

namespace Trinity.Encore.Game.Network.Handling
{
    public abstract class PacketHandlerBase<TPacket>
        where TPacket : IncomingPacket
    {
        private static readonly LogProxy _log = new LogProxy("PacketHandlerBase");

        private static void LogError(string message, IClient client, bool disconnect)
        {
            Contract.Requires(client != null);
            Contract.Requires(!string.IsNullOrEmpty(message));

            // If we have no client, we're running in library mode, and just throw an exception.
            if (client == null)
                throw new ProtocolViolationException("Protocol violation: {0}".Interpolate(message));

            // Otherwise, report an error, as we're serving actual clients.
            _log.Warn("Protocol violation by client {0}: {1}".Interpolate(client, message));

            if (disconnect)
                client.Disconnect();
        }

        protected static bool ProtocolViolation(IClient client, string message, bool disconnect = true)
        {
            Contract.Requires(client != null);
            Contract.Requires(!string.IsNullOrEmpty(message));

            LogError("{0}".Interpolate(message), client, disconnect);
            return false;
        }

        protected static bool InvalidValue<T>(IClient client, PacketField<T> field, bool disconnect = true)
        {
            Contract.Requires(client != null);

            LogError("{0} was invalid (was {1})".Interpolate(field.Name, field.Value), client, disconnect);
            return false;
        }

        protected static bool InvalidValue<T>(IClient client, PacketField<T> field, T expected, bool disconnect = true)
        {
            Contract.Requires(client != null);

            LogError("{0} was expected to be {1} (was {2})".Interpolate(field.Name, expected, field.Value), client, disconnect);
            return false;
        }

        protected static bool InvalidValueRange<T>(IClient client, PacketField<T> field, T expectedLower, T expectedUpper,
            bool disconnect = true)
        {
            Contract.Requires(client != null);

            LogError("{0} was expected to be between {1} and {2} (was {3})".Interpolate(field.Name, expectedLower, expectedUpper, field.Value),
                client, disconnect);
            return false;
        }

        public virtual bool Read(IClient client, TPacket packet)
        {
            Contract.Requires(client != null);
            Contract.Requires(packet != null);

            return true;
        }

        public virtual void Handle(IClient client)
        {
            Contract.Requires(client != null);
        }
    }
}
