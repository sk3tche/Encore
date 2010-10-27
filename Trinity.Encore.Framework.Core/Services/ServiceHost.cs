using System;
using System.Diagnostics.Contracts;
using System.ServiceModel;

namespace Trinity.Encore.Framework.Core.Services
{
    public abstract class ServiceHost<T> : ServiceHost
        where T : class
    {
        protected ServiceHost(string uri)
            : base(typeof(T), new Uri(uri))
        {
            Contract.Requires(!string.IsNullOrEmpty(uri));
        }
    }
}
