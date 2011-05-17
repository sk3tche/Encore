
namespace Trinity.Encore.MapService
{
    public class InstanceMap : Map
    {
        public InstanceMap(int mapId, long instanceId) : base(mapId)
        {
            _instanceId = instanceId;
        }

        private long _instanceId;
        
    }
}
