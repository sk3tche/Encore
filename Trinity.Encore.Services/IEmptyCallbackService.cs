using System.ServiceModel;

namespace Trinity.Encore.Services
{
    public interface IEmptyCallbackService
    {
        [OperationContract]
        void Ping();
    }
}
