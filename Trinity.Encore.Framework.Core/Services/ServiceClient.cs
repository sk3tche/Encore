using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.ServiceModel;

namespace Trinity.Encore.Framework.Core.Services
{
    public class ServiceClient<TService> : ClientBase<TService>
        where TService : class
    {
        [SuppressMessage("Microsoft.Design", "CA1057", Justification = "The tools fail to see the light.")]
        public ServiceClient(string uri)
            : this(new Uri(uri))
        {
            Contract.Requires(!string.IsNullOrEmpty(uri));
        }

        public ServiceClient(Uri uri)
            : base(new NetTcpBinding(SecurityMode.None, true), new EndpointAddress(uri))
        {
            Contract.Requires(uri != null);
        }

        public TService ServiceChannel
        {
            get { return Channel; }
        }
    }
}
