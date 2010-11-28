using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Net;
using System.Text;
using Trinity.Encore.Framework.Core.Cryptography;

namespace Trinity.Encore.Framework.Core.IO
{
    public static class IOExtensions
    {
        public static void WriteCString(this BinaryWriter writer, string str, Encoding encoding = null)
        {
            Contract.Requires(writer != null);
            Contract.Requires(str != null);

            writer.Write((encoding ?? Encoding.ASCII).GetBytes(str));
            writer.Write((byte)0);
        }

        public static string ReadCString(this BinaryReader reader, Encoding encoding = null)
        {
            Contract.Requires(reader != null);
            Contract.Ensures(Contract.Result<string>() != null);

            var list = new List<byte>();

            while (true)
            {
                var chr = reader.ReadByte();

                if (chr == 0)
                    break;

                list.Add(chr);
            }

            var arr = list.ToArray();
            return (encoding ?? Encoding.ASCII).GetString(arr);
        }

        public static void WriteFourCC(this BinaryWriter writer, string value)
        {
            Contract.Requires(writer != null);
            Contract.Requires(value != null);
            Contract.Requires(value.Length == 4);

            writer.Write(Encoding.ASCII.GetBytes(value));
        }

        public static string ReadFourCC(this BinaryReader reader)
        {
            Contract.Requires(reader != null);
            Contract.Ensures(Contract.Result<string>() != null);
            Contract.Ensures(Contract.Result<string>().Length == 4);

            var fourCC = Encoding.ASCII.GetString(reader.ReadBytes(4));
            Contract.Assume(fourCC.Length == 4);
            return fourCC;
        }

        public static void WriteBigEndian(this BinaryWriter writer, short value)
        {
            unchecked
            {
                writer.Write(IPAddress.HostToNetworkOrder(value));
            }
        }

        public static short ReadInt16BigEndian(this BinaryReader reader)
        {
            unchecked
            {
                return IPAddress.NetworkToHostOrder(reader.ReadInt16());
            }
        }

        public static void WriteBigEndian(this BinaryWriter writer, ushort value)
        {
            unchecked
            {
                writer.Write((ushort)IPAddress.HostToNetworkOrder((short)value));
            }
        }

        public static ushort ReadUInt16BigEndian(this BinaryReader reader)
        {
            unchecked
            {
                return (ushort)IPAddress.NetworkToHostOrder((short)reader.ReadUInt16());
            }
        }

        public static void WriteBigEndian(this BinaryWriter writer, int value)
        {
            unchecked
            {
                writer.Write(IPAddress.HostToNetworkOrder(value));
            }
        }

        public static int ReadInt32BigEndian(this BinaryReader reader)
        {
            unchecked
            {
                return IPAddress.NetworkToHostOrder(reader.ReadInt32());
            }
        }

        public static void WriteBigEndian(this BinaryWriter writer, uint value)
        {
            unchecked
            {
                writer.Write((uint)IPAddress.HostToNetworkOrder((int)value));
            }
        }

        public static uint ReadUInt32BigEndian(this BinaryReader reader)
        {
            unchecked
            {
                return (uint)IPAddress.NetworkToHostOrder((int)reader.ReadUInt32());
            }
        }

        public static void WriteBigEndian(this BinaryWriter writer, long value)
        {
            unchecked
            {
                writer.Write(IPAddress.HostToNetworkOrder(value));
            }
        }

        public static long ReadInt64BigEndian(this BinaryReader reader)
        {
            unchecked
            {
                return IPAddress.NetworkToHostOrder(reader.ReadInt64());
            }
        }

        public static void WriteBigEndian(this BinaryWriter writer, ulong value)
        {
            unchecked
            {
                writer.Write((ulong)IPAddress.HostToNetworkOrder((long)value));
            }
        }

        public static ulong ReadUInt64BigEndian(this BinaryReader reader)
        {
            unchecked
            {
                return (ulong)IPAddress.NetworkToHostOrder((long)reader.ReadUInt64());
            }
        }

        public static bool IsRead(this Stream stream)
        {
            Contract.Requires(stream != null);

            return stream.Position == stream.Length;
        }

        public static BinaryReader GetBinaryReader(this byte[] data, Encoding encoding = null)
        {
            Contract.Requires(data != null);
            Contract.Ensures(Contract.Result<BinaryReader>() != null);

            return new BinaryReader(new MemoryStream(data), encoding ?? Encoding.UTF8);
        }

        public static BinaryWriter GetBinaryWriter(this byte[] data, Encoding encoding = null)
        {
            Contract.Requires(data != null);
            Contract.Ensures(Contract.Result<BinaryWriter>() != null);

            return new BinaryWriter(new MemoryStream(data), encoding ?? Encoding.UTF8);
        }
    }
}
