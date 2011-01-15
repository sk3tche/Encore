using System.Diagnostics.Contracts;

namespace Trinity.Encore.Framework.Core.Cryptography.SRP
{
    public sealed class SRPValidator
    {
        internal SRPValidator(SRPBase srp)
        {
            Contract.Requires(srp != null);

            SRP = srp;
        }

        [ContractInvariantMethod]
        private void Invariant()
        {
            Contract.Invariant(SRP != null);
        }

        internal SRPBase SRP { get; private set; }

        /// <summary>
        /// Referred to as M in the specification. This is used for authentication.
        /// 
        /// The client sends this value to the server and the server calculates it locally to verify it.
        /// The same then happens with ServerSessionKeyProof. Note: ClientSessionKeyProof should come first.
        /// </summary>
        public BigInteger ClientSessionKeyProof
        {
            get
            {
                Contract.Ensures(Contract.Result<BigInteger>() != null);

                var parameters = SRP.Parameters;
                var modulusHash = SRP.Hash(parameters.Modulus);
                var generatorHash = SRP.Hash(parameters.Generator);
                var usernameHash = SRP.Hash(SRP.UserName);

                return SRP.Hash(modulusHash ^ generatorHash, usernameHash, SRP.Salt,
                    SRP.PublicEphemeralValueA, SRP.PublicEphemeralValueB, SRP.SessionKey);
            }
        }

        /// <summary>
        /// The server sends this to the client as proof that it has the same session key as the
        /// client. The client will calculate this locally to verify.
        /// </summary>
        public BigInteger ServerSessionKeyProof
        {
            get
            {
                Contract.Ensures(Contract.Result<BigInteger>() != null);

                return SRP.Hash(SRP.PublicEphemeralValueA, ClientSessionKeyProof, SRP.SessionKey);
            }
        }

        public bool IsClientProofValid(BigInteger proof)
        {
            Contract.Requires(proof != null);

            return proof == ClientSessionKeyProof;
        }

        public bool IsServerProofValid(BigInteger proof)
        {
            Contract.Requires(proof != null);

            return proof == ServerSessionKeyProof;
        }
    }
}
