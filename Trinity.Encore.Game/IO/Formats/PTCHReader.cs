using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using Trinity.Core.IO;

namespace Trinity.Encore.Game.IO.Formats
{
    public sealed class PTCHReader : BinaryFileReader
    {
        public const string BSD0ChunkName = "BSD0";

        public const string COPYChunkName = "COPY";

        public PTCHChunk PTCH { get; private set; }

        public MD5Chunk MD5 { get; private set; }

        public XFRMChunk XFRM { get; private set; }

        public BSDIFF40Chunk BSDIFF40 { get; private set; }

        public BSD0Chunk BSD0 { get; private set; }

        public COPYChunk COPY { get; private set; }

        [ContractInvariantMethod]
        private void Invariant()
        {
            Contract.Invariant(!string.IsNullOrEmpty(FileName));
        }

        public PTCHReader(string fileName)
            : base(fileName, System.Text.Encoding.ASCII)
        {
            Contract.Requires(!string.IsNullOrEmpty(fileName));

            ReadFile();
        }

        protected override void Read(BinaryReader reader)
        {
            PTCH = new PTCHChunk(reader);
            MD5 = new MD5Chunk(reader);
            XFRM = new XFRMChunk(reader);

            switch (XFRM.Type)
            {
                case BSD0ChunkName:
                    BSD0 = new BSD0Chunk(reader);
                    BSDIFF40 = BSD0.Unpack();
                    break;
                case COPYChunkName:
                    COPY = new COPYChunk(reader);
                    break;
            }
        }

        public void Apply(string newFileName)
        {
            Contract.Requires(!string.IsNullOrEmpty(newFileName));

            if (XFRM.Type == COPYChunkName)
            {
                File.WriteAllBytes(newFileName, COPY.FullData);
                return;
            }

            var controlBlock = BSDIFF40.ControlBlock.GetBinaryReader();
            var diffBlock = BSDIFF40.DiffBlock.GetBinaryReader();
            var extraBlock = BSDIFF40.ExtraBlock.GetBinaryReader();

            var sizeAfter = PTCH.NewSize;
            var oldFile = File.ReadAllBytes(newFileName);
            var newFile = new byte[sizeAfter];

            var oldFileOffset = 0;
            var newFileOffset = 0;

            while (newFileOffset < sizeAfter)
            {
                var diffChunkSize = controlBlock.ReadInt32();

                if (diffChunkSize < 0)
                    throw new InvalidDataException("Negative diff chunk size encountered.");

                var extraChunkSize = controlBlock.ReadInt32();

                if (extraChunkSize < 0)
                    throw new InvalidDataException("Negative extra chunk size encountered.");

                var extraOffset = controlBlock.ReadInt32();

                var diffChunk = diffBlock.ReadBytes(diffChunkSize);
                Buffer.BlockCopy(diffChunk, 0, newFile, newFileOffset, diffChunkSize);

                for (var i = 0; i < diffChunkSize; i++)
                {
                    var oldPlusI = oldFileOffset + i;
                    if (oldPlusI < 0 || oldPlusI >= PTCH.OldSize)
                        continue;

                    var newPlusI = newFileOffset + i;
                    var nb = newFile[newPlusI];
                    var ob = oldFile[oldPlusI];

                    newFile[newPlusI] = (byte)((nb + ob) % 256);
                }

                newFileOffset += diffChunkSize;
                oldFileOffset += diffChunkSize;

                var extraChunk = extraBlock.ReadBytes(extraChunkSize);
                Buffer.BlockCopy(extraChunk, 0, newFile, newFileOffset, extraChunkSize);

                newFileOffset += extraChunkSize;
                oldFileOffset += ((extraOffset & 0x80000000) != 0) ? (int)(0x80000000 - extraOffset) : extraOffset;
            }

            controlBlock.Dispose();
            diffBlock.Dispose();
            extraBlock.Dispose();

            File.WriteAllBytes(newFileName, newFile);
        }

        #region Chunks

        public sealed class PTCHChunk
        {
            public PTCHChunk(BinaryReader reader)
            {
                Contract.Requires(reader != null);

                Magic = reader.ReadFourCC(); // TODO: Magic check.
                PatchSize = reader.ReadInt32();

                if (PatchSize < 0)
                    throw new InvalidDataException("Negative patch size encountered.");

                OldSize = reader.ReadInt32();

                if (OldSize < 0)
                    throw new InvalidDataException("Negative old size encountered.");

                NewSize = reader.ReadInt32();

                if (NewSize < 0)
                    throw new InvalidDataException("Negative new size encountered.");
            }

            [ContractInvariantMethod]
            private void Invariant()
            {
                Contract.Invariant(Magic != null);
                Contract.Invariant(Magic.Length == 4);
                Contract.Invariant(PatchSize >= 0);
                Contract.Invariant(OldSize >= 0);
                Contract.Invariant(NewSize >= 0);
            }

            public string Magic { get; private set; }

            public int PatchSize { get; private set; }

            public int OldSize { get; private set; }

            public int NewSize { get; private set; }
        }

        public sealed class MD5Chunk
        {
            public const int HashLength = 16; // Size of MD5 digest. No framework constant...

            public MD5Chunk(BinaryReader reader)
            {
                Contract.Requires(reader != null);

                Magic = reader.ReadFourCC(); // TODO: Magic check.
                BlockSize = reader.ReadInt32();

                if (BlockSize < 0)
                    throw new InvalidDataException("Negative block size encountered.");

                OldHash = reader.ReadBytes(HashLength);
                NewHash = reader.ReadBytes(HashLength);
            }

            [ContractInvariantMethod]
            private void Invariant()
            {
                Contract.Invariant(Magic != null);
                Contract.Invariant(Magic.Length == 4);
                Contract.Invariant(BlockSize >= 0);
                Contract.Invariant(OldHash != null);
                Contract.Invariant(OldHash.Length == HashLength);
                Contract.Invariant(NewHash != null);
                Contract.Invariant(NewHash.Length == HashLength);
            }

            public string Magic { get; private set; }

            public int BlockSize { get; private set; }

            public byte[] OldHash { get; private set; }

            public byte[] NewHash { get; private set; }
        }

        public sealed class XFRMChunk
        {
            public XFRMChunk(BinaryReader reader)
            {
                Contract.Requires(reader != null);

                Magic = reader.ReadFourCC(); // TODO: Magic check.
                BlockSize = reader.ReadInt32();

                if (BlockSize < 0)
                    throw new InvalidDataException("Negative block size encountered.");

                Type = reader.ReadFourCC();
            }

            [ContractInvariantMethod]
            private void Invariant()
            {
                Contract.Invariant(Magic != null);
                Contract.Invariant(Magic.Length == 4);
                Contract.Invariant(BlockSize >= 0);
                Contract.Invariant(!string.IsNullOrEmpty(Type));
                Contract.Invariant(Type.Length == 4);
            }

            public string Magic { get; private set; }

            public int BlockSize { get; private set; }

            public string Type { get; private set; }
        }

        public sealed class BSDIFF40Chunk
        {
            public BSDIFF40Chunk(byte[] data)
            {
                Contract.Requires(data != null);

                using (var reader = data.GetBinaryReader())
                {
                    Magic = reader.ReadFourCC() + reader.ReadFourCC(); // TODO: Magic check.
                    ControlBlockSize = reader.ReadInt64();

                    if (ControlBlockSize < 0)
                        throw new InvalidDataException("Negative control block size encountered.");

                    DiffBlockSize = reader.ReadInt64();

                    if (DiffBlockSize < 0)
                        throw new InvalidDataException("Negative diff block size encountered.");

                    NewSize = reader.ReadInt64();

                    if (NewSize < 0)
                        throw new InvalidDataException("Negative new size encountered.");

                    ControlBlock = reader.ReadBytes((int)ControlBlockSize);
                    DiffBlock = reader.ReadBytes((int)DiffBlockSize);
                    var stream = reader.BaseStream;
                    ExtraBlock = reader.ReadBytes((int)(stream.Length - stream.Position));
                }
            }

            [ContractInvariantMethod]
            private void Invariant()
            {
                Contract.Invariant(Magic != null);
                Contract.Invariant(Magic.Length == 8);
                Contract.Invariant(ControlBlockSize >= 0);
                Contract.Invariant(DiffBlockSize >= 0);
                Contract.Invariant(NewSize >= 0);
                Contract.Invariant(ControlBlock != null);
                Contract.Invariant(DiffBlock != null);
                Contract.Invariant(ExtraBlock != null);
            }

            public string Magic { get; private set; }

            public long ControlBlockSize { get; private set; }

            public long DiffBlockSize { get; private set; }

            public long NewSize { get; private set; }

            public byte[] ControlBlock { get; private set; }

            public byte[] DiffBlock { get; private set; }

            public byte[] ExtraBlock { get; private set; }
        }

        public sealed class BSD0Chunk
        {
            public BSD0Chunk(BinaryReader reader)
            {
                Contract.Requires(reader != null);

                UnpackedSize = reader.ReadInt32();

                if (UnpackedSize < 0)
                    throw new InvalidDataException("Negative unpacked size encountered.");

                var stream = reader.BaseStream;
                var count = (int)(stream.Length - stream.Position);
                Contract.Assume(count >= 0);

                CompressedDiff = reader.ReadBytes(count);
            }

            [ContractInvariantMethod]
            private void Invariant()
            {
                Contract.Invariant(UnpackedSize >= 0);
                Contract.Invariant(CompressedDiff != null);
            }

            public BSDIFF40Chunk Unpack()
            {
                using (var reader = CompressedDiff.GetBinaryReader())
                {
                    var list = new List<byte>();

                    while (!reader.BaseStream.IsRead())
                    {
                        var b = reader.ReadByte();
                        list.AddRange((b & 0x80) != 0 ? reader.ReadBytes((b & 0x7f) + 1) : new byte[b + 1]);
                    }

                    return new BSDIFF40Chunk(list.ToArray());
                }
            }

            public int UnpackedSize { get; private set; }

            public byte[] CompressedDiff { get; private set; }
        }

        public sealed class COPYChunk
        {
            public COPYChunk(BinaryReader reader)
            {
                Contract.Requires(reader != null);

                var stream = reader.BaseStream;
                var count = (int)(stream.Length - stream.Position);
                Contract.Assume(count >= 0);

                FullData = reader.ReadBytes(count);
            }

            [ContractInvariantMethod]
            private void Invariant()
            {
                Contract.Invariant(FullData != null);
            }

            public byte[] FullData { get; private set; }
        }

        #endregion
    }
}
