using System;
using FluentNHibernate;

namespace Trinity.Encore.Framework.Game.Database.Conventions.Naming
{
    public sealed class ForeignKeyNameConvention : FluentNHibernate.Conventions.ForeignKeyConvention
    {
        public const string Id = "Id";

        protected override string GetKeyName(Member property, Type type)
        {
            return (property != null ? property.Name : type.Name) + Id;
        }
    }
}
