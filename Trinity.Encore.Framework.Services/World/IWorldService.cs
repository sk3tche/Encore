using System.Net.Security;
using System.ServiceModel;

namespace Trinity.Encore.Framework.Services.World
{
    [ServiceContract(ProtectionLevel = ProtectionLevel.None, SessionMode = SessionMode.Required)]
    public interface IWorldService
    {
    }
}
