using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace Trinity.Encore.Framework.Core.Services
{
    public abstract class DuplexServiceClient<TService, TCallback> : DuplexClientBase<TService>
        where TService : class
        where TCallback : class, new()
    {
        protected DuplexServiceClient(string uri)
            : base(new TCallback(), new NetTcpBinding(SecurityMode.None), new EndpointAddress(uri))
        {
            Contract.Requires(!string.IsNullOrEmpty(uri));
        }
    }
}
