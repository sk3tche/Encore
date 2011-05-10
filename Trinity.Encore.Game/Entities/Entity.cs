using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trinity.Core.Security;
using Trinity.Core.Threading;
using Trinity.Core.Threading.Actors;

namespace Trinity.Encore.Game.Entities
{
    public abstract class Entity : Actor<Entity>, IEntity
    {
        /// <summary>
        /// Called from Map::Update, updates the unit.
        /// </summary>
        /// <param name="diff">The time in ms since the last call to this function</param>
        public void Update(int diff)
        {
            
        }

        public EntityGuid Guid { get; private set; }
    }
}
