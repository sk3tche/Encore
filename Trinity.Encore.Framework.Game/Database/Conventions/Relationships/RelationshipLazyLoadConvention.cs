using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Mapping;

namespace Trinity.Encore.Framework.Game.Database.Conventions.Relationships
{
    internal sealed class RelationshipLazyLoadConvention : IHasManyConvention, IHasManyConventionAcceptance, IHasManyToManyConvention,
        IHasManyToManyConventionAcceptance, IHasOneConvention, IHasOneConventionAcceptance, IReferenceConvention, IReferenceConventionAcceptance
    {
        public void Apply(IOneToManyCollectionInstance instance)
        {
            instance.LazyLoad(); // TODO: Do we need ExtraLazyLoad?
        }

        public void Accept(IAcceptanceCriteria<IOneToManyCollectionInspector> criteria)
        {
        }

        public void Apply(IManyToManyCollectionInstance instance)
        {
            instance.LazyLoad(); // TODO: Do we need ExtraLazyLoad?
        }

        public void Accept(IAcceptanceCriteria<IManyToManyCollectionInspector> criteria)
        {
        }

        public void Apply(IOneToOneInstance instance)
        {
            instance.LazyLoad(Laziness.Proxy);
        }

        public void Accept(IAcceptanceCriteria<IOneToOneInspector> criteria)
        {
        }

        public void Apply(IManyToOneInstance instance)
        {
            instance.LazyLoad(Laziness.Proxy);
        }

        public void Accept(IAcceptanceCriteria<IManyToOneInspector> criteria)
        {
        }
    }
}
