using System.Diagnostics.Contracts;
using System.IO;
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
    }
}
