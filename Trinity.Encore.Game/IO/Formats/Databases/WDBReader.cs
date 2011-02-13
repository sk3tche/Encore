using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using Trinity.Core.IO;

namespace Trinity.Encore.Game.IO.Formats.Databases
{
    public sealed class WDBReader<T> : ClientDbReader<T>
        where T : class, IClientDbRecord, new()
    {
        public int Magic { get; private set; }

        public int Build { get; private set; }

        public ClientLocale Locale { get; private set; }

        public int Unknown1 { get; private set; }

        public int Unknown2 { get; private set; }

        public int Version { get; private set; }

        public WDBReader(string fileName)
            : base(fileName)
        {
            Contract.Requires(!string.IsNullOrEmpty(fileName));
        }

        public override StringReadMode StringReadMode
        {
            get { return StringReadMode.Direct; }
        }

        protected override byte[] ReadData(BinaryReader reader)
        {
            Magic = reader.ReadInt32();
            Build = reader.ReadInt32();
            Locale = (ClientLocale)reader.ReadInt32();
            Unknown1 = reader.ReadInt32();
            Unknown2 = reader.ReadInt32();
            Version = reader.ReadInt32();

            var data = new List<byte>();
            var count = 0;

            while (!reader.BaseStream.IsRead())
            {
                var entry = reader.ReadInt32();
                var size = reader.ReadInt32();

                if (entry == 0 && size == 0)
                    continue; // End of file.

                Contract.Assume(size > 0);
                var row = reader.ReadBytes(size);
                data.AddRange(BitConverter.GetBytes(entry));
                data.AddRange(row);
                count++;
            }

            RecordCount = count;
            return data.ToArray();
        }
    }
}
