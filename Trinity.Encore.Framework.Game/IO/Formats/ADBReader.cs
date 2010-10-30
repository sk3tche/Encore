using System.Diagnostics.Contracts;
using System.IO;

namespace Trinity.Encore.Framework.Game.IO.Formats
{
    public sealed class ADBReader<T> : ClientDbReader<T>
        where T : class, IClientDbRecord, new()
    {
        /// <summary>
        /// ADB file header magic number (WCH2).
        /// </summary>
        public const int HeaderMagicNumber = 0x32484357;

        public int FieldCount { get; private set; }

        public int RecordSize { get; private set; }

        public int TableHash { get; private set; }

        public int Build { get; private set; }

        public int Unknown1 { get; private set; }

        public int Unknown2 { get; private set; }

        public int Unknown3 { get; private set; }

        public ClientLocale Locale { get; private set; }

        public int Unknown4 { get; private set; }

        public ADBReader(string fileName)
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
            // TODO: It really looks like ADB and DB2 files are the exact same structure. We might
            // want to just merge ADBReader and DB2Reader into one.
            RecordCount = reader.ReadInt32();
            FieldCount = reader.ReadInt32();
            RecordSize = reader.ReadInt32();
            StringTableSize = reader.ReadInt32();
            TableHash = reader.ReadInt32();
            Build = reader.ReadInt32();
            Unknown1 = reader.ReadInt32();
            Unknown2 = reader.ReadInt32();
            Unknown3 = reader.ReadInt32();
            Locale = (ClientLocale)reader.ReadInt32();
            Unknown4 = reader.ReadInt32();

            // No idea what these are...
            if (Unknown3 != 0)
            {
                var size = Unknown3 * 4 - 48;
                Contract.Assume(size > 0);

                reader.ReadBytes(size);
                reader.ReadBytes(size * 2);
            }

            // Read in all the records.
            var count = RecordCount * RecordSize;
            Contract.Assume(count >= 0);
            return reader.ReadBytes(count);
        }
    }
}
