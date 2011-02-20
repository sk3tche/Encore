using System.Diagnostics.Contracts;
using System.IO;

namespace Trinity.Encore.Game.IO.Formats.Databases
{
    public sealed class DBCReader<T> : ClientDbReader<T>
        where T : class, IClientDbRecord, new()
    {
        /// <summary>
        /// DBC file header magic number (WDBC).
        /// </summary>
        public const int HeaderMagicNumber = 0x43424457;

        public int FieldCount { get; private set; }

        public int RecordSize { get; private set; }

        public DBCReader(string fileName)
            : base(fileName)
        {
            Contract.Requires(!string.IsNullOrEmpty(fileName));
        }

        public override int? HeaderMagic
        {
            get { return HeaderMagicNumber; }
        }

        protected override byte[] ReadData(BinaryReader reader)
        {
            RecordCount = reader.ReadInt32();

            if (RecordCount < 0)
                throw new InvalidDataException("Negative record count was encountered.");

            FieldCount = reader.ReadInt32();

            if (FieldCount < 0)
                throw new InvalidDataException("Negative field count was encountered.");

            RecordSize = reader.ReadInt32();

            if (RecordSize < 0)
                throw new InvalidDataException("Negative record size was encountered.");

            StringTableSize = reader.ReadInt32();

            if (StringTableSize < 0)
                throw new InvalidDataException("Negative string table size was encountered.");

            // Read in all the records.
            var count = RecordCount * RecordSize;
            return reader.ReadBytes(count);
        }
    }
}
