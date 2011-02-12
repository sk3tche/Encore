using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.ServiceModel;

namespace Trinity.Core.Services
{
    public sealed class ServiceHost<TInterface, TService> : ServiceHost
        where TInterface : class
        where TService : class
    {
        public ServiceHost(TService instance, Uri uri)
            : base(instance)
        {
            Contract.Requires(instance != null);
            Contract.Requires(uri != null);

            AddServiceEndpoint(typeof(TInterface), new NetTcpBinding(SecurityMode.None, true), uri);
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
