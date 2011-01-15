using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Security.Cryptography;

namespace Trinity.Encore.Framework.Core.Cryptography.SRP
{
    /// <summary>
    /// Parameters for the Secure Remote Password (SRP) protocol.
    /// </summary>
    [ContractClass(typeof(SRPParametersContracts))]
    public abstract class SRPParameters
    {
        /// <summary>
        /// Length of calculated session keys.
        /// </summary>
        public abstract int KeyLength { get; }

        /// <summary>
        /// Gets the size of the backing hash function, in bytes.
        /// </summary>
        public int HashLength
        {
            get
            {
                Contract.Ensures(Contract.Result<int>() > 0);

                var hashSize = Hash.HashSize / 8;
                Contract.Assume(hashSize > 0);
                return hashSize;
            }
        }

        /// <summary>
        /// Random number generator for this instance.
        /// </summary>
        public RandomNumberGenerator RandomGenerator { get; private set; }

        protected SRPParameters(SRPVersion version, bool caseSensitive)
        {
            AlgorithmVersion = version;
            CaseSensitive = caseSensitive;

            SetupParameters();

            RandomGenerator = new RNGCryptoServiceProvider();
            Multiplier = version == SRPVersion.SRP6 ? (BigInteger)3 : Hash.FinalizeHash(Modulus, Generator);
        }

        [ContractInvariantMethod]
        private void Invariant()
        {
            Contract.Invariant(RandomGenerator != null);
            Contract.Invariant(Hash != null);
            Contract.Invariant(Modulus != null);
            Contract.Invariant(Generator != null);
            Contract.Invariant(Multiplier != null);
        }

        protected abstract void SetupParameters();

        /// <summary>
        /// Version of this instance.
        /// </summary>
        public SRPVersion AlgorithmVersion { get; private set; }

        /// <summary>
        /// Gets a value indicating whether or not usernames and passwords are case-sensitive.
        /// </summary>
        public bool CaseSensitive { get; private set; }

        /// <summary>
        /// H in the specification. Hashing function for the instance.
        /// </summary>
        public HashAlgorithm Hash { get; protected set; }

        /// <summary>
        /// This is N in the specification.
        /// 
        /// All arithmetic is modulo this number. It should be a large prime.
        /// </summary>
        public BigInteger Modulus { get; protected set; }

        /// <summary>
        /// This is g in the specification.
        /// 
        /// This number must be a generator in the finite field of Modulus (N).
        /// </summary>
        public BigInteger Generator { get; protected set; }

        /// <summary>
        /// This is k in the specification. In SRP 6a, k = H(N, g). Older versions have k = 3.
        /// </summary>
        public BigInteger Multiplier { get; private set; }

        /// <summary>
        /// Generate a random number of a specified size.
        /// </summary>
        /// <param name="size">Maximum size in bytes of the random number.</param>
        public BigInteger RandomNumber(int size)
        {
            Contract.Ensures(Contract.Result<BigInteger>() != null);

            var buffer = new byte[size];

            RandomGenerator.GetBytes(buffer);

            if (buffer[0] == 0)
                buffer[0] = 1;

            return new BigInteger(buffer);
        }
    }

    [ContractClassFor(typeof(SRPParameters))]
    public abstract class SRPParametersContracts : SRPParameters
    {
        protected SRPParametersContracts(SRPVersion version, bool caseSensitive)
            : base(version, caseSensitive)
        {
        }

        public override int KeyLength
        {
            get
            {
                Contract.Ensures(Contract.Result<int>() > 0);

                return 0;
            }
        }

        protected override void SetupParameters()
        {
            Contract.Ensures(Hash != null);
            Contract.Ensures(Modulus != null);
            Contract.Ensures(Generator != null);
        }
    }
}
