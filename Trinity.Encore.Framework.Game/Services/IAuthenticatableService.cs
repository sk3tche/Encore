using System;
using System.Collections.ObjectModel;
using System.Net.Security;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using Trinity.Encore.Framework.Core.Cryptography;

namespace Trinity.Encore.Framework.Game.Services
{
    /// <summary>
    /// Base interface for services that require session authentication.
    /// 
    /// You must set IsInitiating to false on all other methods in your services when inheriting
    /// this interface.
    /// </summary>
    [ServiceContract(ProtectionLevel = ProtectionLevel.None, SessionMode = SessionMode.Required)]
    public interface IAuthenticatableService
    {
        [OperationContract(IsInitiating = true)]
        void Authenticate(string username, BigInteger password);
    }
}
