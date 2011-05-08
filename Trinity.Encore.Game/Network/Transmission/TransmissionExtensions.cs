using System;
using System.Diagnostics.Contracts;
using System.Net;
using System.Text;
using Trinity.Core.Cryptography;
using Trinity.Core.IO;
using Trinity.Encore.Game.Entities;
using Trinity.Encore.Game.IO;
using Trinity.Network.Transmission;

namespace Trinity.Encore.Game.Network.Transmission
{
    public static class TransmissionExtensions
    {
        #region Basic Types

        public static PacketField<byte> ReadByteField(this IncomingPacket packet, string name)
        {
            Contract.Requires(packet != null);
            Contract.Requires(name != null);

            return new PacketField<byte>(PacketFieldType.Byte, packet.ReadByte(), name);
        }

        [CLSCompliant(false)]
        public static PacketField<sbyte> ReadSByteField(this IncomingPacket packet, string name)
        {
            Contract.Requires(packet != null);
            Contract.Requires(name != null);

            return new PacketField<sbyte>(PacketFieldType.SByte, packet.ReadSByte(), name);
        }

        [CLSCompliant(false)]
        public static PacketField<ushort> ReadUInt16Field(this IncomingPacket packet, string name)
        {
            Contract.Requires(packet != null);
            Contract.Requires(name != null);

            return new PacketField<ushort>(PacketFieldType.UInt16, packet.ReadUInt16(), name);
        }

        public static PacketField<short> ReadInt16Field(this IncomingPacket packet, string name)
        {
            Contract.Requires(packet != null);
            Contract.Requires(name != null);

            return new PacketField<short>(PacketFieldType.Int16, packet.ReadInt16(), name);
        }

        [CLSCompliant(false)]
        public static PacketField<uint> ReadUInt32Field(this IncomingPacket packet, string name)
        {
            Contract.Requires(packet != null);
            Contract.Requires(name != null);

            return new PacketField<uint>(PacketFieldType.UInt32, packet.ReadUInt32(), name);
        }

        public static PacketField<int> ReadInt32Field(this IncomingPacket packet, string name)
        {
            Contract.Requires(packet != null);
            Contract.Requires(name != null);

            return new PacketField<int>(PacketFieldType.Int32, packet.ReadInt32(), name);
        }

        [CLSCompliant(false)]
        public static PacketField<ulong> ReadUInt64Field(this IncomingPacket packet, string name)
        {
            Contract.Requires(packet != null);
            Contract.Requires(name != null);

            return new PacketField<ulong>(PacketFieldType.UInt64, packet.ReadUInt64(), name);
        }

        public static PacketField<long> ReadInt64Field(this IncomingPacket packet, string name)
        {
            Contract.Requires(packet != null);
            Contract.Requires(name != null);

            return new PacketField<long>(PacketFieldType.Int64, packet.ReadInt64(), name);
        }

        public static PacketField<float> ReadSingleField(this IncomingPacket packet, string name)
        {
            Contract.Requires(packet != null);
            Contract.Requires(name != null);

            return new PacketField<float>(PacketFieldType.Single, packet.ReadSingle(), name);
        }

        public static PacketField<double> ReadDoubleField(this IncomingPacket packet, string name)
        {
            Contract.Requires(packet != null);
            Contract.Requires(name != null);

            return new PacketField<double>(PacketFieldType.Double, packet.ReadDouble(), name);
        }

        public static PacketField<decimal> ReadDecimalField(this IncomingPacket packet, string name)
        {
            Contract.Requires(packet != null);
            Contract.Requires(name != null);

            return new PacketField<decimal>(PacketFieldType.Decimal, packet.ReadDecimal(), name);
        }

        public static PacketField<char> ReadCharField(this IncomingPacket packet, string name)
        {
            Contract.Requires(packet != null);
            Contract.Requires(name != null);

            return new PacketField<char>(PacketFieldType.Char, packet.ReadChar(), name);
        }

        public static PacketField<string> ReadStringField(this IncomingPacket packet, string name)
        {
            Contract.Requires(packet != null);
            Contract.Requires(name != null);

            return new PacketField<string>(PacketFieldType.String, packet.ReadString(), name);
        }

        public static PacketField<bool> ReadBooleanField(this IncomingPacket packet, string name)
        {
            Contract.Requires(packet != null);
            Contract.Requires(name != null);

            return new PacketField<bool>(PacketFieldType.Boolean, packet.ReadBoolean(), name);
        }

        public static PacketField<char[]> ReadCharsField(this IncomingPacket packet, string name, int count)
        {
            Contract.Requires(packet != null);
            Contract.Requires(name != null);
            Contract.Requires(count >= 0);

            return new PacketField<char[]>(PacketFieldType.Chars, packet.ReadChars(count), name);
        }

        public static PacketField<byte[]> ReadBytesField(this IncomingPacket packet, string name, int count)
        {
            Contract.Requires(packet != null);
            Contract.Requires(name != null);
            Contract.Requires(count >= 0);

            return new PacketField<byte[]>(PacketFieldType.Bytes, packet.ReadBytes(count), name);
        }

        #endregion

        #region Core Types

        public static PacketField<string> ReadCStringField(this IncomingPacket packet, string name, Encoding encoding = null)
        {
            Contract.Requires(packet != null);
            Contract.Requires(name != null);

            return new PacketField<string>(PacketFieldType.CString, packet.ReadCString(encoding), name);
        }

        public static PacketField<string> ReadP8StringField(this IncomingPacket packet, string name, Encoding encoding = null)
        {
            Contract.Requires(packet != null);
            Contract.Requires(name != null);

            return new PacketField<string>(PacketFieldType.P8String, packet.ReadP8String(encoding), name);
        }

        public static PacketField<string> ReadP16StringField(this IncomingPacket packet, string name, Encoding encoding = null)
        {
            Contract.Requires(packet != null);
            Contract.Requires(name != null);

            return new PacketField<string>(PacketFieldType.P16String, packet.ReadP16String(encoding), name);
        }

        public static PacketField<string> ReadP32StringField(this IncomingPacket packet, string name, Encoding encoding = null)
        {
            Contract.Requires(packet != null);
            Contract.Requires(name != null);

            return new PacketField<string>(PacketFieldType.P32String, packet.ReadP32String(encoding), name);
        }

        public static PacketField<string> ReadFourCCField(this IncomingPacket packet, string name)
        {
            Contract.Requires(packet != null);
            Contract.Requires(name != null);

            return new PacketField<string>(PacketFieldType.FourCC, packet.ReadFourCC(), name);
        }

        public static PacketField<BigInteger> ReadBigIntegerField(this IncomingPacket packet, string name)
        {
            Contract.Requires(packet != null);
            Contract.Requires(name != null);

            return new PacketField<BigInteger>(PacketFieldType.BigInteger, packet.ReadBigInteger(), name);
        }

        public static PacketField<BigInteger> ReadBigIntegerField(this IncomingPacket packet, string name, int length)
        {
            Contract.Requires(packet != null);
            Contract.Requires(name != null);
            Contract.Requires(length >= 0);

            return new PacketField<BigInteger>(PacketFieldType.FixedBigInteger, packet.ReadBigInteger(length), name);
        }

        public static PacketField<IPAddress> ReadIPAddressField(this IncomingPacket packet, string name, bool ipv6)
        {
            Contract.Requires(packet != null);
            Contract.Requires(name != null);

            return new PacketField<IPAddress>(PacketFieldType.IPAddress, packet.ReadIPAddress(ipv6), name);
        }

        #endregion

        #region Game Types

        [CLSCompliant(false)]
        public static PacketField<ulong> ReadPackedUInt64Field(this IncomingPacket packet, string name)
        {
            Contract.Requires(packet != null);
            Contract.Requires(name != null);

            return new PacketField<ulong>(PacketFieldType.PackedUInt64, packet.ReadPackedUInt64(), name);
        }

        public static PacketField<EntityGuid> ReadGuidField(this IncomingPacket packet, string name)
        {
            Contract.Requires(packet != null);
            Contract.Requires(name != null);

            return new PacketField<EntityGuid>(PacketFieldType.Guid, packet.ReadGuid(), name);
        }

        public static PacketField<EntityGuid> ReadSmartGuidField(this IncomingPacket packet, string name)
        {
            Contract.Requires(packet != null);
            Contract.Requires(name != null);

            return new PacketField<EntityGuid>(PacketFieldType.SmartGuid, packet.ReadSmartGuid(), name);
        }

        #endregion
    }
}
