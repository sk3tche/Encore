using System;
using System.Diagnostics.Contracts;
using System.IO;
using Trinity.Encore.Framework.Core.Cryptography;
using Trinity.Encore.Framework.Game.Entities;

namespace Trinity.Encore.Framework.Game.IO
{
    public static class IOExtensions
    {
        public static EntityGuid ReadPackedGuid(this BinaryReader reader)
        {
            Contract.Requires(reader != null);

            return new EntityGuid(ReadPackedUInt64(reader));
        }

        public static void WritePackedGuid(this BinaryWriter writer, EntityGuid guid)
        {
            Contract.Requires(writer != null);

            WritePackedUInt64(writer, guid.Full);
        }

        [CLSCompliant(false)]
        public static ulong ReadPackedUInt64(this BinaryReader reader)
        {
            Contract.Requires(reader != null);

            ulong guid = 0;
            var guidMark = reader.ReadByte();

            for (var i = 0; i < 8; ++i)
            {
                if ((guidMark & (1 << i)) == 0)
                    continue;

                var bit = reader.ReadByte();
                guid |= (ulong)bit << (i * 8);
            }

            return guid;
        }

        [CLSCompliant(false)]
        public static void WritePackedUInt64(this BinaryWriter writer, ulong value)
        {
            Contract.Requires(writer != null);

            var packedGuid = new byte[8 + 1];
            ulong size = 1;

            for (var i = 0; value != 0; ++i)
            {
                var guidPart = value & 0xff;
                if (guidPart != 0)
                {
                    packedGuid[0] |= (byte)(1 << i);
                    packedGuid[size] = (byte)guidPart;

                    ++size;
                }

                value >>= 8;
            }

            writer.Write(packedGuid);
        }

        public static void Write(this BinaryWriter writer, BigInteger bigInt, int numBytes, bool prefix = false)
        {
            Contract.Requires(writer != null);
            Contract.Requires(bigInt != null);
            Contract.Requires(numBytes >= 0);

            var data = bigInt.GetBytes(numBytes);

            if (prefix)
                writer.Write((byte)numBytes);

            writer.Write(data);
        }

        public static BigInteger ReadBigInteger(this BinaryReader reader)
        {
            Contract.Requires(reader != null);

            var length = reader.ReadByte();
            Contract.Assume(length >= 0);
            var data = reader.ReadBytes(length);

            return new BigInteger(data);
        }

        public static BigInteger ReadBigInteger(this BinaryReader reader, int length)
        {
            Contract.Requires(reader != null);
            Contract.Requires(length >= 0);

            var data = reader.ReadBytes(length);

            return new BigInteger(data);
        }
    }
}
