using System.Diagnostics.Contracts;
using System.ServiceModel;

namespace Trinity.Encore.Framework.Core.Services
{
    public abstract class ServiceClient<TService> : ClientBase<TService>
        where TService : class
    {
        protected ServiceClient(string uri)
            : base(new NetTcpBinding(SecurityMode.None, true), new EndpointAddress(uri))
        {
            Contract.Requires(!string.IsNullOrEmpty(uri));
        }

        public TService ServiceChannel
        {
            get { return Channel; }
        }
    }
}
