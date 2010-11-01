using System;
using System.Diagnostics.Contracts;
using System.ServiceModel;

namespace Trinity.Encore.Framework.Core.Services
{
    public abstract class ServiceHost<TService> : ServiceHost
        where TService : class
    {
        protected ServiceHost(TService instance, string uri)
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
