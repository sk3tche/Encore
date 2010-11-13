using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Text;

namespace Trinity.Encore.Framework.Network.Transmission
{
    [ContractClass(typeof(OutgoingPacketContracts))]
    public abstract class OutgoingPacket : BinaryWriter, IPacket
    {
        protected OutgoingPacket(Enum opCode, Encoding encoding, int capacity = 0)
            : base(new MemoryStream(capacity), encoding)
        {
            Contract.Requires(opCode != null);
            Contract.Requires(encoding != null);
            Contract.Requires(capacity >= 0);

            OpCode = opCode;
        }

        [ContractInvariantMethod]
        private void Invariant()
        {
            Contract.Invariant(OpCode != null);
            Contract.Invariant(Length >= 0);
            Contract.Invariant(Position >= 0);
            Contract.Invariant(HeaderLength > 0);
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

        public abstract int HeaderLength { get; }

        public abstract void WriteHeader(byte[] buffer);
    }

    [ContractClassFor(typeof(OutgoingPacket))]
    public abstract class OutgoingPacketContracts : OutgoingPacket
    {
        protected OutgoingPacketContracts(Enum opCode, Encoding encoding, int capacity)
            : base(opCode, encoding, capacity)
        {
        }

        public override int HeaderLength
        {
            get
            {
                Contract.Ensures(Contract.Result<int>() > 0);

                return 0;
            }
        }

        public override void WriteHeader(byte[] buffer)
        {
            Contract.Requires(buffer != null);
            Contract.Requires(buffer.Length >= HeaderLength);
        }
    }
}
