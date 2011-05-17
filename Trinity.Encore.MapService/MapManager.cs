using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Trinity.Core.Collections;
using Trinity.Encore.Game.Identification;

namespace Trinity.Encore.MapService
{
    public sealed class MapManager
    {
        /// <summary>
        /// Update all maps registered to the service.
        /// </summary>
        /// <param name="timeDiff">Time in ms since last call to this function.</param>
        public void Update(int timeDiff)
        {
            // Call Map::Update on all registered continent maps.
            foreach (var map in _continentMaps)
                map.Value.Update(timeDiff);

            // Call Map::Update on all registered instance maps.
            foreach (var instanceMap in _instanceMaps)
                instanceMap.Value.Update(timeDiff);
        }

        #region Map generation
        /// <summary>
        /// Generate new Map object with given mapId.
        /// </summary>
        /// <param name="mapId">Entry in Map.dbc</param>
        /// <returns></returns>
        public Map GenerateContinentMap(int mapId)
        {
            Contract.Requires(mapId >= 0);

            var m = new Map(mapId);
            _continentMaps.Add(mapId, m);

            return m;
        }

        public InstanceMap GenerateInstanceMap(int mapId)
        {
            Contract.Requires(mapId >= 0);

            var instanceId = (long)_instanceIdGenerator.GenerateId();   // TODO: Proper typing and initialization of IDGenerator
            var im = new InstanceMap(mapId, instanceId);
            _instanceMaps.Add(instanceId, im);

            return im;
        }

        public BattlegroundMap GenerateBattlegroundMap(int mapId)
        {
            Contract.Requires(mapId >= 0);

            var instanceId = (long)_instanceIdGenerator.GenerateId();   // TODO: Proper typing and initialization of IDGenerator
            var bgm = new BattlegroundMap(mapId, instanceId);
            _instanceMaps.Add(instanceId, bgm);

            return bgm;
        }
        #endregion

        #region Map accessors
        /// <summary>
        /// Returns InstanceMap object with specified instanceId
        /// </summary>
        /// <param name="instanceId">The unique Id of the instance map</param>
        /// <returns></returns>
        public InstanceMap GetInstanceMap(long instanceId)
        {
            Contract.Requires(instanceId >= 0);

            return _instanceMaps.TryGet(instanceId);
        }

        public BattlegroundMap GetBattlegroundMap(long instanceId)
        {
            Contract.Requires(instanceId >= 0);

            return (BattlegroundMap)_instanceMaps.TryGet(instanceId);
        }

        public Map GetContinentMap(int mapId)
        {
            Contract.Requires(mapId >= 0);

            return _continentMaps.TryGet(mapId);
        }
        #endregion

        private IdGenerator _instanceIdGenerator;

        /// <summary>
        /// Holds instanceable maps, keyed by instanceId
        /// </summary>
        private readonly Dictionary<long, InstanceMap> _instanceMaps = new Dictionary<long, InstanceMap>();

        /// <summary>
        /// Holds continent maps and transports, which are unique per Map ID
        /// </summary>
        private readonly Dictionary<int, Map> _continentMaps = new Dictionary<int, Map>();
    }
}
    