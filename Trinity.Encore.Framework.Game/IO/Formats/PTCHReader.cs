using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using Trinity.Encore.Framework.Core.IO;

namespace Trinity.Encore.Framework.Game.IO.Formats
{
    public sealed class PTCHReader
    {
        public const string BSD0ChunkName = "BSD0";

        public const string COPYChunkName = "COPY";

        public string FileName { get; private set; }

        public PTCHChunk PTCH { get; private set; }

        public MD5Chunk MD5 { get; private set; }

        public XFRMChunk XFRM { get; private set; }

        public BSDIFF40Chunk BSDIFF40 { get; private set; }

        public BSD0Chunk BSD0 { get; private set; }

        public COPYChunk COPY { get; private set; }

        public string Type { get; private set; }

        [ContractInvariantMethod]
        private void Invariant()
        {
            Contract.Invariant(!string.IsNullOrEmpty(FileName));
        }

        public PTCHReader(string fileName)
        {
            Contract.Requires(!string.IsNullOrEmpty(fileName));

            FileName = fileName;

            ReadFile();
        }

        private void ReadFile()
        {
            Contract.Ensures(PTCH != null);
            Contract.Ensures(MD5 != null);
            Contract.Ensures(XFRM != null);
            Contract.Ensures(!string.IsNullOrEmpty(Type));
            Contract.Ensures(Type.Length == 4);

            var stream = File.Open(FileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            using (var reader = new BinaryReader(stream))
            {
                PTCH = new PTCHChunk(reader);
                MD5 = new MD5Chunk(reader);
                XFRM = new XFRMChunk(reader);
                Type = reader.ReadFourCC();

                switch (Type)
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

            Contract.Assume(PTCH != null);
            Contract.Assume(MD5 != null);
            Contract.Assume(XFRM != null);
            Contract.Assume(!string.IsNullOrEmpty(Type));
            Contract.Assume(Type.Length == 4);
        }

        public void Apply(string newFileName)
        {
            if (Type == COPYChunkName)
            {
                File.WriteAllBytes(newFileName, COPY.FullData);
                return;
            }

            var controlBlock = BSDIFF40.ControlBlock.GetBinaryReader();
            var diffBlock = BSDIFF40.DiffBlock.GetBinaryReader();
            var extraBlock = BSDIFF40.ExtraBlock.GetBinaryReader();

            var sizeAfter = PTCH.SizeAfter;
            var oldFile = File.ReadAllBytes(newFileName);
            var newFile = new byte[sizeAfter];

            var oldFileOffset = 0;
            var newFileOffset = 0;

            while (newFileOffset < sizeAfter)
            {
                var diffChunkSize = controlBlock.ReadInt32();
                var extraChunkSize = controlBlock.ReadInt32();
                var extraOffset = controlBlock.ReadInt32();

                Contract.Assume(diffChunkSize >= 0);
                var diffChunk = diffBlock.ReadBytes(diffChunkSize);
                Buffer.BlockCopy(diffChunk, 0, newFile, newFileOffset, diffChunkSize);

                for (var i = 0; i < diffChunkSize; i++)
                {
                    var oldPlusI = oldFileOffset + i;
                    if (oldPlusI < 0 || oldPlusI >= PTCH.SizeBefore)
                        continue;

                    var newPlusI = newFileOffset + i;
                    var nb = newFile[newPlusI];
                    var ob = oldFile[oldPlusI];

                    newFile[newPlusI] = (byte)((nb + ob) % 256);
                }

                newFileOffset += diffChunkSize;
                oldFileOffset += diffChunkSize;

                Contract.Assume(extraChunkSize >= 0);
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

                Magic = reader.ReadFourCC();
                PatchSize = reader.ReadInt32();
                SizeBefore = reader.ReadInt32();
                SizeAfter = reader.ReadInt32();

                Contract.Assert(PatchSize >= 0);
                Contract.Assert(SizeBefore >= 0);
                Contract.Assert(SizeAfter >= 0);
            }

            [ContractInvariantMethod]
            private void Invariant()
            {
                Contract.Invariant(Magic != null);
                Contract.Invariant(Magic.Length == 4);
                Contract.Invariant(PatchSize >= 0);
                Contract.Invariant(SizeBefore >= 0);
                Contract.Invariant(SizeAfter >= 0);
            }

            public string Magic { get; private set; }

            public int PatchSize { get; private set; }

            public int SizeBefore { get; private set; }

            public int SizeAfter { get; private set; }
        }

        public sealed class MD5Chunk
        {
            public const int HashLength = 16;

            public MD5Chunk(BinaryReader reader)
            {
                Contract.Requires(reader != null);

                Magic = reader.ReadFourCC();
                BlockSize = reader.ReadInt32();
                HashBefore = reader.ReadBytes(HashLength);
                HashAfter = reader.ReadBytes(HashLength);

                Contract.Assert(BlockSize >= 0);
            }

            [ContractInvariantMethod]
            private void Invariant()
            {
                Contract.Invariant(Magic != null);
                Contract.Invariant(Magic.Length == 4);
                Contract.Invariant(BlockSize >= 0);
                Contract.Invariant(HashBefore != null);
                Contract.Invariant(HashBefore.Length == HashLength);
                Contract.Invariant(HashAfter != null);
                Contract.Invariant(HashAfter.Length == HashLength);
            }

            public string Magic { get; private set; }

            public int BlockSize { get; private set; }

            public byte[] HashBefore { get; private set; }

            public byte[] HashAfter { get; private set; }
        }

        public sealed class XFRMChunk
        {
            public XFRMChunk(BinaryReader reader)
            {
                Contract.Requires(reader != null);

                Magic = reader.ReadFourCC();
                BlockSize = reader.ReadInt32();

                Contract.Assert(BlockSize >= 0);
            }

            [ContractInvariantMethod]
            private void Invariant()
            {
                Contract.Invariant(Magic != null);
                Contract.Invariant(Magic.Length == 4);
                Contract.Invariant(BlockSize >= 0);
            }

            public string Magic { get; private set; }

            public int BlockSize { get; private set; }
        }

        public sealed class BSDIFF40Chunk
        {
            public BSDIFF40Chunk(byte[] data)
            {
                Contract.Requires(data != null);

                using (var reader = new BinaryReader(new MemoryStream(data)))
                {
                    Magic = reader.ReadFourCC() + reader.ReadFourCC();
                    ControlBlockSize = reader.ReadInt64();
                    DiffBlockSize = reader.ReadInt64();
                    SizeAfter = reader.ReadInt64();

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
                Contract.Invariant(SizeAfter >= 0);
                Contract.Invariant(ControlBlock != null);
                Contract.Invariant(DiffBlock != null);
                Contract.Invariant(ExtraBlock != null);
            }

            public string Magic { get; private set; }

            public long ControlBlockSize { get; private set; }

            public long DiffBlockSize { get; private set; }

            public long SizeAfter { get; private set; }

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
                var stream = reader.BaseStream;
                var count = (int)(stream.Length - stream.Position);
                Contract.Assume(count >= 0);
                CompressedDiff = reader.ReadBytes(count);

                Contract.Assert(UnpackedSize >= 0);
            }

            [ContractInvariantMethod]
            private void Invariant()
            {
                Contract.Invariant(UnpackedSize >= 0);
                Contract.Invariant(CompressedDiff != null);
            }

            public BSDIFF40Chunk Unpack()
            {
                using (var reader = new BinaryReader(new MemoryStream(CompressedDiff)))
                {
                    var list = new List<byte>();

                    byte b;
                    while ((b = reader.ReadByte()) != 0)
                        list.AddRange((b & 0x80) != 0 ? reader.ReadBytes((b & 0x7f) + 1) : new byte[b + 1]);

                    list.Add(0);
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
