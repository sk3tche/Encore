using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Text;

namespace Trinity.Encore.Framework.Network.Transmission
{
    public abstract class IncomingPacket : BinaryReader, IPacket
    {
        protected IncomingPacket(Enum opCode, byte[] data, int length, Encoding encoding)
            : base(new MemoryStream(data, 0, length, false, false), encoding)
        {
            Contract.Requires(opCode != null);
            Contract.Requires(data != null);
            Contract.Requires(length >= 0);
            Contract.Requires(length < data.Length);
            Contract.Requires(encoding != null);

            OpCode = opCode;
        }

        [ContractInvariantMethod]
        private void Invariant()
        {
            Contract.Invariant(OpCode != null);
            Contract.Invariant(Length >= 0);
            Contract.Invariant(Position >= 0);
        }

        public Enum OpCode { get; private set; }

        public int Length
        {
            get { return (int)BaseStream.Length; }
        }

        public int Position
        {
            get { return (int)BaseStream.Position; }
            set { BaseStream.Position = value; }
        }
    }
}
