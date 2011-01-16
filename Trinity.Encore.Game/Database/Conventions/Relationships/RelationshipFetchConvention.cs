using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace Trinity.Encore.Game.Database.Conventions.Relationships
{
    internal sealed class RelationshipFetchConvention : IHasManyConvention, IHasManyConventionAcceptance, IHasManyToManyConvention,
        IHasManyToManyConventionAcceptance, IHasOneConvention, IHasOneConventionAcceptance, IReferenceConvention, IReferenceConventionAcceptance
    {
        public void Apply(IOneToManyCollectionInstance instance)
        {
            instance.Fetch.Select();
        }

        public void Accept(IAcceptanceCriteria<IOneToManyCollectionInspector> criteria)
        {
            criteria.Expect(x => x.Fetch, Is.Not.Set);
        }

        public void Apply(IManyToManyCollectionInstance instance)
        {
            instance.Fetch.Select();
        }

        public void Accept(IAcceptanceCriteria<IManyToManyCollectionInspector> criteria)
        {
            criteria.Expect(x => x.Fetch, Is.Not.Set);
        }

        public void Apply(IOneToOneInstance instance)
        {
            instance.Fetch.Select();
        }

        public void Accept(IAcceptanceCriteria<IOneToOneInspector> criteria)
        {
            criteria.Expect(x => x.Fetch, Is.Not.Set);
        }

        public void Apply(IManyToOneInstance instance)
        {
            instance.Fetch.Select();
        }

        public void Accept(IAcceptanceCriteria<IManyToOneInspector> criteria)
        {
            criteria.Expect(x => x.Fetch, Is.Not.Set);
        }
    }
}
