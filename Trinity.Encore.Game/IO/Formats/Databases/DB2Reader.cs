using System.Diagnostics.Contracts;
using System.IO;

namespace Trinity.Encore.Game.IO.Formats.Databases
{
    public class DB2Reader<T> : ClientDbReader<T>
        where T : class, IClientDbRecord, new()
    {
        /// <summary>
        /// DB2 file header magic number (WDB2).
        /// </summary>
        public const int HeaderMagicNumber = 0x32424457;

        public const int NewHeaderBuild = 12880;

        public int FieldCount { get; private set; }

        public int RecordSize { get; private set; }

        public int TableHash { get; private set; }

        public int Build { get; private set; }

        public int LastUpdated { get; private set; }

        public int MinId { get; private set; }

        public int MaxId { get; private set; }

        public ClientLocale Locale { get; private set; }

        public int Unknown4 { get; private set; }

        public override StringReadMode StringReadMode
        {
            get { return StringReadMode.StringTable2; }
        }

        public DB2Reader(string fileName)
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

            TableHash = reader.ReadInt32();
            Build = reader.ReadInt32();

            if (Build < 0)
                throw new InvalidDataException("Negative build was encountered.");

            LastUpdated = reader.ReadInt32();

            if (Build > NewHeaderBuild)
            {
                MinId = reader.ReadInt32();

                if (MinId < 0)
                    throw new InvalidDataException("Negative minimum ID was encountered.");

                MaxId = reader.ReadInt32();

                if (MaxId < 0)
                    throw new InvalidDataException("Negative maximum ID was encountered.");

                Locale = (ClientLocale)reader.ReadInt32();
                Unknown4 = reader.ReadInt32();

                // No idea what these are...
                // TODO: Unhackify this.
                if (MaxId > 0)
                {
                    if (MaxId < 12)
                        throw new InvalidDataException("Invalid maximum ID value was encountered.");

                    var size = MaxId * 4 - 48;
                    reader.ReadBytes(size);
                    reader.ReadBytes(size * 2);
                }
            }

            // Read in all the records.
            var count = RecordCount * RecordSize;
            return reader.ReadBytes(count);
        }
    }
}
