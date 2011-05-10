using Microsoft.VisualStudio.TestTools.UnitTesting;
using Trinity.Core.Cryptography.SRP;
using Trinity.Encore.Game.Cryptography;

namespace Trinity.Encore.Tests.Core.Cryptography
{
    [TestClass]
    public sealed class SRPTest
    {
        private void TestSRP(SRPParameters srpParams)
        {
            var password = Password.GenerateCredentialsHash(srpParams.Hash, "TEST", "TESTPW");
            var server = new SRPServer("TEST", password, srpParams);
            var client = new SRPClient("TEST", password, srpParams);

            // Client sends A to the server.
            server.PublicEphemeralValueA = client.PublicEphemeralValueA;

            // Server sends s and B to the client.
            client.Salt = server.Salt;
            client.PublicEphemeralValueB = server.PublicEphemeralValueB;

            Assert.IsTrue(client.SessionKey == server.SessionKey);
            Assert.IsTrue(server.Validator.IsClientProofValid(client.Validator.ClientSessionKeyProof));
            Assert.IsTrue(client.Validator.IsServerProofValid(server.Validator.ServerSessionKeyProof));
        }

        [TestMethod]
        public void TestVersion6()
        {
            TestSRP(new WowAuthenticationParameters(caseSensitive: true));
            TestSRP(new WowAuthenticationParameters());
        }

        [TestMethod]
        public void TestVersion6A()
        {
            TestSRP(new SC2AuthenticationParameters(caseSensitive: true));
            TestSRP(new SC2AuthenticationParameters());
        }
    }
}
