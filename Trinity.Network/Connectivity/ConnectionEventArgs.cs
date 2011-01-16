using System;
using System.Diagnostics.Contracts;

namespace Trinity.Network.Connectivity
{
    public sealed class ConnectionEventArgs : EventArgs
    {
        public ConnectionEventArgs(IClient client)
        {
            Contract.Requires(client != null);

            Client = client;
        }

        [ContractInvariantMethod]
        private void Invariant()
        {
            Contract.Invariant(Client != null);
        }

        public IClient Client { get; private set; }
    }
}
