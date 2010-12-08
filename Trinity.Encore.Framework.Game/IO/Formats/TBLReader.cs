using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Text;
using Trinity.Encore.Framework.Core.IO;

namespace Trinity.Encore.Framework.Game.IO.Formats
{
    public sealed class TBLReader : BinaryFileReader
    {
        public TBLReader(string fileName)
            : base(fileName, Encoding.ASCII)
        {
            Contract.Requires(!string.IsNullOrEmpty(fileName));

            Records = new List<TBLRecord>();
        }

        [ContractInvariantMethod]
        private void Invariant()
        {
            Contract.Invariant(Records != null);
        }

        protected override void Read(BinaryReader reader)
        {
            Magic = reader.ReadFourCC();
            Load = reader.ReadInt32();
            LastModified = reader.ReadInt32();
            Build = reader.ReadInt32();

            while (!reader.BaseStream.IsRead())
                Records.Add(new TBLRecord(reader));
        }

        public string Magic { get; private set; }

        public int Load { get; private set; }

        public int LastModified { get; private set; }

        public int Build { get; private set; }

        public List<TBLRecord> Records { get; private set; }

        public sealed class TBLRecord
        {
            public TBLRecord(BinaryReader reader)
            {
                Contract.Requires(reader != null);

                TableHash = reader.ReadInt32();
                Unknown1 = reader.ReadInt32();
                Unknown2 = reader.ReadInt32();
            }

            public int TableHash { get; private set; }

            public int Unknown1 { get; private set; }

            public int Unknown2 { get; private set; }
        }
    }
}
