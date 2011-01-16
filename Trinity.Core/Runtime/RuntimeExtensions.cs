using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml.Serialization;

namespace Trinity.Core.Runtime
{
    public static class RuntimeExtensions
    {
        public static void ThrowIfDisposed(this IDisposableResource resource)
        {
            Contract.Requires(resource != null);

            if (resource.IsDisposed)
                throw new ObjectDisposedException(resource.ToString(), "An attempt was made to use a disposed object.");
        }

        public static byte[] ToBinary(this object obj)
        {
            Contract.Requires(obj != null);
            Contract.Ensures(Contract.Result<byte[]>() != null);

            var formatter = new BinaryFormatter();

            using (var stream = new MemoryStream())
            {
                formatter.Serialize(stream, obj);

                var length = (int)stream.Length;
                var bytes = new byte[length];

                stream.Position = 0;
                stream.Write(bytes, 0, length);

                return bytes;
            }
        }

        public static byte[] ToXml(this object obj)
        {
            Contract.Requires(obj != null);
            Contract.Ensures(Contract.Result<byte[]>() != null);

            var serializer = new XmlSerializer(obj.GetType());

            using (var stream = new MemoryStream())
            {
                serializer.Serialize(stream, obj);

                var length = (int)stream.Length;
                var bytes = new byte[length];

                stream.Position = 0;
                stream.Write(bytes, 0, length);

                return bytes;
            }
        }

        public static string ToXmlString(this object obj)
        {
            Contract.Requires(obj != null);
            Contract.Ensures(Contract.Result<string>() != null);

            return Encoding.UTF8.GetString(obj.ToXml());
        }
    }
}
