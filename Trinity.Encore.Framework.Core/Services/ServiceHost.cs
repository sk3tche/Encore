using System;
using System.Diagnostics.Contracts;
using System.ServiceModel;

namespace Trinity.Encore.Framework.Core.Services
{
    public sealed class ServiceHost<TService> : ServiceHost
        where TService : class
    {
        public ServiceHost(TService instance, string uri)
            : base(instance, new Uri(uri))
        {
            Contract.Requires(instance != null);
            Contract.Requires(!string.IsNullOrEmpty(uri));
        }

        public TService Channel
        {
            get { return (TService)SingletonInstance; }
        }
    }
}
