using System.Security.Cryptography;

namespace Trinity.Encore.Framework.Core.Cryptography.SRP.Parameters
{
    public sealed class SHA1Parameters : SRPParameters
    {
        public const int KeySize = 32;

        public SHA1Parameters(SRPVersion version, bool caseSensitive)
            : base(version, caseSensitive)
        {
        }

        protected override void SetupParameters()
        {
            Hash = new SHA1Managed();
            Generator = new BigInteger(7);
            Modulus = new BigInteger("B79B3E2A87823CAB8F5EBFBF8EB10108535006298B5BADBD5B53E1895E644B89");
        }

        public static SHA1Parameters Default
        {
            get { return new SHA1Parameters(SRPVersion.SRP6A, true); }
        }

        public override int KeyLength
        {
            get { return KeySize; }
        }
    }
}
