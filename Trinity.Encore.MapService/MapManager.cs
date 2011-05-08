using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using Trinity.Encore.Game.Identification;

namespace Trinity.Encore.MapService
{
    public class MapManager
    {
        /// <summary>
        /// Update all maps registered to the service.
        /// </summary>
        /// <param name="timeDiff">Time in ms since last call to this function.</param>
        public void Update(int timeDiff)
        {
            // Call Map::Update on all registered map.
            foreach (var m in _maps)
                m.Value.Update(timeDiff);
        }

        /// <summary>
        /// Returns Map object with specified instanceId
        /// </summary>
        /// <param name="instanceId"></param>
        /// <returns></returns>
        public Map GetMap(long instanceId)
        {
            Contract.Requires(instanceId >= 0);
            return _maps[instanceId];
        }

        /// <summary>
        /// Generate new Map object with given mapId.
        /// Will generate new instanceId.
        /// </summary>
        /// <param name="mapId">Entry in Map.dbc</param>
        /// <returns></returns>
        public Map GenerateMap(int mapId)
        {
            Contract.Requires(mapId >= 0);
            return GenerateMap(mapId, (long)_idGen.GenerateId());
        }

        /// <summary>
        /// Generate new Map object with given mapId and instanceId.
        /// </summary>
        /// <param name="mapId">Entry in Map.dbc</param>
        /// <param name="instanceId">Assigned unique instance identifier.</param>
        /// <returns></returns>
        public Map GenerateMap(int mapId, long instanceId)
        {
            Contract.Requires(mapId >= 0);
            Contract.Requires(instanceId >= 0);
            Map m = new Map(mapId, instanceId);
            _maps.Add(instanceId, m);
            return m;
        }

        private IdGenerator _idGen;
        private Dictionary<long, Map> _maps;
    }
}
