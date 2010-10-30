using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Text;

namespace Trinity.Encore.Framework.Core.IO
{
    public static class IOExtensions
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
