using System.Collections.Generic;
using System.Diagnostics.Contracts;
using FluentNHibernate.Conventions;
using Trinity.Encore.Game.Database.Conventions.Identity;
using Trinity.Encore.Game.Database.Conventions.Naming;
using Trinity.Encore.Game.Database.Conventions.Properties;
using Trinity.Encore.Game.Database.Conventions.Relationships;
using Trinity.Persistence;

namespace Trinity.Encore.Game.Database
{
    public abstract class GameDatabaseContext : DatabaseContext
    {
        protected GameDatabaseContext(DatabaseType type, string connString)
            : base(type, connString)
        {
            Contract.Requires(!string.IsNullOrEmpty(connString));
        }

        protected override sealed IEnumerable<IConvention> CreateConventions()
        {
            yield return new IdConvention();
            yield return new IdGenerationConvention();

            yield return new ClassNameConvention();
            yield return new ForeignKeyNameConvention();

            yield return new PropertyNullableConvention();
            yield return new PropertyUpdateConvention();

            yield return new ReferenceConvention();
            yield return new RelationshipCascadeConvention();
            yield return new RelationshipFetchConvention();
            yield return new RelationshipLazyLoadConvention();
        }
    }
}
