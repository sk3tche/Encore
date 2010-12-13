using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace Trinity.Encore.Framework.Game.Database.Conventions.Relationships
{
    internal sealed class ReferenceConvention : IReferenceConvention
    {
        public void Apply(IManyToOneInstance instance)
        {
            instance.NotFound.Exception();
        }
    }
}
