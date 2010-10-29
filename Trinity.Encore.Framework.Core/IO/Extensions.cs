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

        public static bool IsRead(this Stream stream)
        {
            Contract.Requires(stream != null);

            return stream.Position == stream.Length;
        }
    }
}
