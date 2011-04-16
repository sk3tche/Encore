using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using Trinity.Encore.Game.Partitioning;
using Trinity.Encore.Game.Entities;
using Mono.GameMath;

namespace Trinity.Encore.MapService
{
    /// <summary>
    /// This class is instantiated for every "game-instance" of a map.
    /// All objects on the map are stored in this class.
    /// DBC-related data is referenced for fast lookup.
    /// </summary>
    public class Map
    {
        /// <summary>
        /// Constructs a new instance of this map.
        /// TODO: Construct based on DBC template
        /// </summary>
        public Map(int mapId, long instanceId)
        {
            MapId = mapId;
            InstanceId = instanceId;
        }

        /// <summary>
        /// Updates active units and processes events.
        /// </summary>
        /// <param name="timeDiff">Time in ms since last call to this function</param>
        public void Update(int timeDiff)
        {
            // Execute scheduled update routines
        }

        /// <summary>
        /// Registers an entity on this instance of a map
        /// </summary>
        /// <param name="entity"></param>
        public void AddEntity(IWorldEntity entity)
        {
            Contract.Requires(entity != null);

            _entityQuadTree.AddEntity(entity);
            _entityLookup.Add(entity.Guid, entity);
        }

        /// <summary>
        /// Unregisters an entity on this instance of a map
        /// </summary>
        /// <param name="entity"></param>
        public void RemoveEntity(IWorldEntity entity)
        {
            Contract.Requires(entity != null);

            _entityQuadTree.RemoveEntity(entity);
            _entityLookup.Remove(entity.Guid);
        }

        /// <summary>
        /// MapId from Map.dbc
        /// </summary>
        public int MapId { get; private set; }

        /// <summary>
        /// Unique identifier for the instance
        /// </summary>
        public long InstanceId { get; private set; }
        
        /// <summary>
        /// QuadTree-based storage of Entities
        /// TODO: Proper bounds
        /// </summary>
        private readonly QuadTree _entityQuadTree = new QuadTree(new BoundingBox(new Vector3(-17066, -17066, -17066), new Vector3(17066, 17066, 17066)));

        /// <summary>
        /// Dictionary-based storage of Entities, for fast guid-based lookup
        /// </summary>
        private readonly Dictionary<EntityGuid, IWorldEntity> _entityLookup = new Dictionary<EntityGuid, IWorldEntity>();
    }
}
