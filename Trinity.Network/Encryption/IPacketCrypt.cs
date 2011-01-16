using System.Diagnostics.Contracts;

namespace Trinity.Network.Encryption
{
    [ContractClass(typeof(PacketCryptContracts))]
    public interface IPacketCrypt
    {
        int Encrypt(byte[] buffer, int start, int count);

        int Decrypt(byte[] buffer, int start, int count);
    }

    [ContractClassFor(typeof(IPacketCrypt))]
    public abstract class PacketCryptContracts : IPacketCrypt
    {
        public int Encrypt(byte[] buffer, int start, int count)
        {
            Contract.Requires(buffer != null);
            Contract.Requires(start >= 0);
            Contract.Requires(count >= 0);
            Contract.Requires(start <= buffer.Length + count);

            return 0;
        }

        public int Decrypt(byte[] buffer, int start, int count)
        {
            Contract.Requires(buffer != null);
            Contract.Requires(start >= 0);
            Contract.Requires(count >= 0);
            Contract.Requires(start <= buffer.Length + count);

            return 0;
        }
    }
}
