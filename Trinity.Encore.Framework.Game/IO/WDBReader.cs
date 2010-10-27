using System;
using System.Diagnostics.Contracts;
using System.IO;

namespace Trinity.Encore.Framework.Game.IO
{
    public sealed class WDBReader<T> : ClientDbReader<T>
        where T : class, IClientDbRecord, new()
    {
        /// <summary>
        /// WDB file header magic number (WIDB).
        /// </summary>
        public const int HeaderMagicNumber = 0x57494442;

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

        public override int HeaderMagic
        {
            get { return HeaderMagicNumber; }
        }

        protected override byte[] ReadData(BinaryReader reader)
        {
            if (reader.ReadInt32() != HeaderMagic)
                throw new IOException("Invalid WDB file.");

            Build = reader.ReadInt32();
            Locale = (ClientLocale)reader.ReadInt32();
            Unknown1 = reader.ReadInt32();
            Unknown2 = reader.ReadInt32();
            Version = reader.ReadInt32();

            // TODO: Reading rows isn't as simple as with other formats...
            throw new NotImplementedException();
        }
    }
}
