using System.Diagnostics.Contracts;
using System.ServiceModel;

namespace Trinity.Encore.Framework.Core.Services
{
    public abstract class DuplexServiceClient<TService, TCallback> : DuplexClientBase<TService>
        where TService : class
        where TCallback : class, new()
    {
        protected DuplexServiceClient(TCallback callback, string uri)
            : base(callback, new NetTcpBinding(SecurityMode.None, true), new EndpointAddress(uri))
        {
            Contract.Requires(callback != null);
            Contract.Requires(!string.IsNullOrEmpty(uri));

            CallbackChannel = callback;
        }

        public TService ServiceChannel
        {
            get { return Channel; }
        }

        public TCallback CallbackChannel { get; private set; }
    }
}
