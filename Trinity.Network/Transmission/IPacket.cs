using System;
using System.Diagnostics.Contracts;
using System.IO;

namespace Trinity.Network.Transmission
{
    [ContractClass(typeof(PacketContracts))]
    public interface IPacket
    {
        Stream BaseStream { get; }

        Enum OpCode { get; }

        int Length { get; }

        int Position { get; set; }
    }

    [ContractClassFor(typeof(IPacket))]
    public abstract class PacketContracts : IPacket
    {
        public Stream BaseStream
        {
            get
            {
                Contract.Ensures(Contract.Result<Stream>() != null);

                return null;
            }
        }

        public Enum OpCode
        {
            get { throw new NotImplementedException(); }
        }

        public int Length
        {
            get
            {
                Contract.Ensures(Contract.Result<int>() >= 0);

                return 0;
            }
        }

        public int Position
        {
            get
            {
                Contract.Ensures(Contract.Result<int>() >= 0);

                return 0;
            }
            set { Contract.Requires(value >= 0); }
        }
    }
}
