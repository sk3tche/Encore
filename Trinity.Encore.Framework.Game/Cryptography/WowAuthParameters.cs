using System.Security.Cryptography;
using Trinity.Encore.Framework.Core.Cryptography;
using Trinity.Encore.Framework.Core.Cryptography.SRP;

namespace Trinity.Encore.Framework.Game.Cryptography
{
    public sealed class WowAuthParameters : SRPParameters
    {
        public const int KeySize = 32;

        public WowAuthParameters(SRPVersion version, bool caseSensitive)
            : base(version, caseSensitive)
        {
        }

        protected override void SetupParameters()
        {
            Hash = new SHA1Managed();
            Generator = new BigInteger(7);
            Modulus = new BigInteger("B79B3E2A87823CAB8F5EBFBF8EB10108535006298B5BADBD5B53E1895E644B89");
        }

        public static WowAuthParameters Default
        {
            get { return new WowAuthParameters(SRPVersion.SRP6, true); }
        }

        public override int KeyLength
        {
            get { return KeySize; }
        }
    }
}
