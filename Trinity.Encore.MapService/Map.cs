using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Trinity.Core.Collections;
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
        public Map(int mapId)
        {
            MapId = mapId;
        }

        /// <summary>
        /// Updates active units and processes events.
        /// </summary>
        /// <param name="timeDiff">Time in ms since last call to this function</param>
        public void Update(int timeDiff)
        {
            // Execute scheduled update routines
            foreach (var worldEntity in _entityQuadTree.FindEntities(x => !x.Node.IsEmpty).Cast<WorldEntity>())
            {
                Contract.Assume(worldEntity != null);
                worldEntity.PostAsync(() => worldEntity.Update(timeDiff));
            }
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
        /// Gets an entity based on GUID in this map.
        /// </summary>
        /// <param name="guid">EntityGuid of unit to look for</param>
        /// <returns>IWorldEntity object</returns>
        public IWorldEntity GetEntityInMap(EntityGuid guid)
        {
            Contract.Requires(guid != EntityGuid.Zero);

            return _entityLookup.TryGet(guid);
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
        /// These coordinates represent the boundaries of the QuadTree.
        /// TODO: Proper extraction of these coordinates based on client data.
        /// </summary>
        private const float MinX = -17066;
        private const float MinY = -17066;
        private const float MinZ = -17066;
        private const float MaxX = 17066;
        private const float MaxY = 17066;
        private const float MaxZ = 17066;

        /// <summary>
        /// QuadTree-based storage of Entities
        /// TODO: Proper bounds
        /// </summary>
        private readonly QuadTree _entityQuadTree = new QuadTree(new BoundingBox(new Vector3(MinX, MinY, MinZ), new Vector3(MaxX, MaxY, MaxZ)));

        /// <summary>
        /// Dictionary-based storage of Entities, for fast guid-based lookup
        /// </summary>
        private readonly Dictionary<EntityGuid, IWorldEntity> _entityLookup = new Dictionary<EntityGuid, IWorldEntity>();
    }
}
