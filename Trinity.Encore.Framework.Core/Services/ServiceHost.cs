using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.ServiceModel;

namespace Trinity.Encore.Framework.Core.Services
{
    public sealed class ServiceHost<TService> : ServiceHost
        where TService : class
    {
        public ServiceHost(TService instance, Uri uri)
            : base(instance, uri)
        {
            Contract.Requires(instance != null);
            Contract.Requires(uri != null);
        }

        [SuppressMessage("Microsoft.Design", "CA1057", Justification = "The tools fail to see the light.")]
        public ServiceHost(TService instance, string uri)
            : this(instance, new Uri(uri))
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
