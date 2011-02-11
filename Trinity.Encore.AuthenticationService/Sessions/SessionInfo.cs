using System;
using System.Diagnostics.Contracts;
using Trinity.Core.Cryptography.SRP;
using Trinity.Core.Runtime.Serialization;
using Trinity.Encore.Services.Account;
using Trinity.Encore.Services.Authentication;

namespace Trinity.Encore.AuthenticationService.Sessions
{
    public sealed class SessionInfo : IMemberwiseSerializable<AuthenticationData>
    {
        public bool Active { get; set; }

        public SRPServer SRP { get; set; }

        [ContractInvariantMethod]
        private void Invariant()
        {
            Contract.Invariant(SRP != null);
        }

        public SessionInfo(SRPServer srp)
        {
            Contract.Requires(srp != null);

            SRP = srp;
        }

        public AuthenticationData Serialize()
        {
            Contract.Ensures(Contract.Result<AuthenticationData>() != null);

            return new AuthenticationData
            {
                SessionKey = SRP.SessionKey,
                Salt = SRP.Salt,
                Verifier = SRP.Verifier,
            };
        }
    }
}
