using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using Trinity.Encore.Framework.Core.IO;

namespace Trinity.Encore.Framework.Game.IO
{
    public sealed class DBCReader<T> : ClientDbReader<T>
        where T : class, IClientDbRecord, new()
    {
        /// <summary>
        /// DBC file header magic number (WDBC).
        /// </summary>
        public const int HeaderMagicNumber = 0x43424457;

        public int RecordCount { get; private set; }

        public int FieldCount { get; private set; }

        public int RecordSize { get; private set; }

        public DBCReader(string fileName)
            : base(fileName)
        {
            Contract.Requires(!string.IsNullOrEmpty(fileName));
        }

        public override int HeaderMagic
        {
            get { return HeaderMagicNumber; }
        }

        protected override byte[] ReadData(BinaryReader reader)
        {
            RecordCount = reader.ReadInt32();
            FieldCount = reader.ReadInt32();
            RecordSize = reader.ReadInt32();
            StringTableSize = reader.ReadInt32();

            // Read in all the records.
            var count = RecordCount * RecordSize;
            Contract.Assume(count >= 0);
            return reader.ReadBytes(count);
        }
    }
}
