using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.ServiceModel;

namespace Trinity.Encore.Framework.Core.Services
{
    public class DuplexServiceClient<TService, TCallback> : DuplexClientBase<TService>
        where TService : class
        where TCallback : class, new()
    {
        [SuppressMessage("Microsoft.Design", "CA1057", Justification = "The tools fail to see the light.")]
        public DuplexServiceClient(TCallback callback, string uri)
            : this(callback, new Uri(uri))
        {
            Contract.Requires(callback != null);
            Contract.Requires(!string.IsNullOrEmpty(uri));
        }

        public DuplexServiceClient(TCallback callback, Uri uri)
            : base(callback, new NetTcpBinding(SecurityMode.None, true), new EndpointAddress(uri))
        {
            Contract.Requires(callback != null);
            Contract.Requires(uri != null);

            CallbackChannel = callback;
        }

        public TService ServiceChannel
        {
            get { return Channel; }
        }

        public TCallback CallbackChannel { get; private set; }
    }
}
