using System;
using System.Diagnostics.Contracts;
using System.ServiceModel;
using Trinity.Encore.Framework.Core.Exceptions;
using Trinity.Encore.Framework.Core.Services;
using Trinity.Encore.Framework.Core.Threading.Actors;

namespace Trinity.Encore.Framework.Game.Services
{
    public sealed class IpcDevice<TService, TCallback> : Actor
        where TService : class
        where TCallback : class, new()
    {
        private DuplexServiceClient<TService, TCallback> _client;

        private readonly Func<DuplexServiceClient<TService, TCallback>> _creator;

        [ContractInvariantMethod]
        private void Invariant()
        {
            Contract.Invariant(_creator != null);
        }

        public IpcDevice(Func<DuplexServiceClient<TService, TCallback>> clientCreator)
        {
            Contract.Requires(clientCreator != null);

            _creator = clientCreator;
            _client = clientCreator();
            _client.Open();
        }

        public void Call(Action<TService> call)
        {
            Contract.Requires(call != null);

            try
            {
                call(_client.ServiceChannel);
            }
            catch (Exception ex)
            {
                if (ex is CommunicationException)
                    Post(Reconnect);

                // Register, but ignore the exception.
                ExceptionManager.RegisterException(ex);
            }
        }

        private void Connect()
        {
            _client = _creator();
            _client.Open();
        }

        private void Disconnect()
        {
            var state = _client.State;
            if (state != CommunicationState.Closing && state != CommunicationState.Closed)
                _client.Close();

            _client = null;
        }

        private void Reconnect()
        {
            var state = _client.State;
            if (state == CommunicationState.Opening || state == CommunicationState.Opened)
                return;

            Disconnect();
            Connect();
        }

        protected override void Dispose(bool disposing)
        {
            if (IsDisposed)
                return;

            if (_client != null)
                Disconnect();

            base.Dispose(disposing);
        }
    }
}
