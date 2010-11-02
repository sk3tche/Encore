using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.ServiceModel;
using System.Threading.Tasks.Dataflow;
using Trinity.Encore.Framework.Core.Collections;
using Trinity.Encore.Framework.Core.Configuration;
using Trinity.Encore.Framework.Core.Exceptions;
using Trinity.Encore.Framework.Core.Logging;
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

        private readonly Queue<Action<TService>> _msgQueue = new Queue<Action<TService>>();

        [ContractInvariantMethod]
        private void Invariant()
        {
            Contract.Invariant(_creator != null);
            Contract.Invariant(_msgQueue != null);
        }

        public IpcDevice(Func<DuplexServiceClient<TService, TCallback>> clientCreator)
        {
            Contract.Requires(clientCreator != null);

            _creator = clientCreator;
            _client = clientCreator();
            _client.Open();

            ServiceCalls = new ActionBlock<Action<TService>>(new Action<Action<TService>>(HandleServiceCall),
                GetOptions(CancellationToken));
        }

        public ITargetBlock<Action<TService>> ServiceCalls { get; private set; }

        private void HandleServiceCall(Action<TService> call)
        {
            Contract.Requires(call != null);

            // Process any calls that previously failed.
            while (!_msgQueue.IsEmpty())
            {
                var msg = _msgQueue.Dequeue();
                msg(_client.ServiceChannel);
            }

            try
            {
                call(_client.ServiceChannel);
            }
            catch (Exception ex)
            {
                if (ex is CommunicationException)
                {
                    _msgQueue.Enqueue(call);
                    IncomingMessages.Post(Reconnect);
                }

                // Register, but ignore the exception.
                ExceptionManager.RegisterException(ex, this);
            }
        }

        private void Connect()
        {
            _client = _creator();
            _client.Open();
        }

        private void Disconnect()
        {
            if (_client.State != CommunicationState.Closed)
                _client.Close();

            _client = null;
        }

        private void Reconnect()
        {
            Disconnect();
            Connect();
        }

        protected override void Cleanup()
        {
            if (_client != null)
                Disconnect();
        }
    }
}
