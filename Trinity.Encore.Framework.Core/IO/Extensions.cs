using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Text;

namespace Trinity.Encore.Framework.Core.IO
{
    public static class Extensions
    {
        public static void WriteCString(this BinaryWriter writer, string str, Encoding encoding = null)
        {
            Contract.Requires(writer != null);
            Contract.Requires(str != null);

            writer.Write(encoding != null ? encoding.GetBytes(str) : Encoding.ASCII.GetBytes(str));
            writer.Write('\0');
        }

        public static string ReadCString(this BinaryReader reader, Encoding encoding = null)
        {
            Contract.Requires(reader != null);
            Contract.Ensures(Contract.Result<string>() != null);

            var list = new List<byte>();

            while (true)
            {
                var chr = reader.ReadByte();

                if (chr == '\0')
                    break;

                list.Add(chr);
            }

            var arr = list.ToArray();
            return encoding != null ? encoding.GetString(arr) : Encoding.ASCII.GetString(arr);
        }

        public static ulong ReadPackedGuid(this Stream stream)
        {
            Contract.Requires(stream != null);
            Contract.Requires(stream.Position + 9 <= stream.Length);
            Contract.Ensures(Contract.Result<ulong>() != 0);
            ulong guid = 0;
            var guidmark = stream.ReadByte();

            for (int i = 0; i < 8; ++i)
            {
                if((guidmark & ((byte)1 << i)) != 0)
                {
                    var bit = stream.ReadByte();
                    guid |= (ulong)bit << (i * 8);
                }
            }
            return guid;
        }

        public static void WritePackedGuid(this BinaryWriter writer, ulong guid)
        {
            Contract.Requires(writer != null);
            byte[] packGUID = new byte[8+1];
            packGUID[0] = 0;
            ulong size = 1;
            for(byte i = 0; guid != 0; ++i)
            {
                if((guid & 0xFF) != 0)
                {
                    packGUID[0] |= (byte)(1 << i);
                    packGUID[size] =  (byte)(guid & 0xFF);
                    ++size;
                }

                guid >>= 8;
            }
            writer.Write(packGUID);
        }

        public static bool IsRead(this Stream stream)
        {
            Contract.Requires(stream != null);

            return stream.Position == stream.Length;
        }
    }
}
