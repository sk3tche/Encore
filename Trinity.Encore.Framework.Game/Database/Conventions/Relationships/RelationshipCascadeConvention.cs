using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace Trinity.Encore.Framework.Game.Database.Conventions.Relationships
{
    internal sealed class RelationshipCascadeConvention : IHasManyConvention, IHasManyConventionAcceptance, IHasManyToManyConvention,
        IHasManyToManyConventionAcceptance, IHasOneConvention, IHasOneConventionAcceptance, IReferenceConvention, IReferenceConventionAcceptance
    {
        public void Apply(IOneToManyCollectionInstance instance)
        {
            instance.Cascade.SaveUpdate();
        }

        public void Accept(IAcceptanceCriteria<IOneToManyCollectionInspector> criteria)
        {
            criteria.Expect(x => x.Cascade, Is.Not.Set);
        }

        public void Apply(IManyToManyCollectionInstance instance)
        {
            instance.Cascade.SaveUpdate();
        }

        public void Accept(IAcceptanceCriteria<IManyToManyCollectionInspector> criteria)
        {
            criteria.Expect(x => x.Cascade, Is.Not.Set);
        }

        public void Apply(IOneToOneInstance instance)
        {
            instance.Cascade.SaveUpdate();
        }

        public void Accept(IAcceptanceCriteria<IOneToOneInspector> criteria)
        {
            criteria.Expect(x => x.Cascade, Is.Not.Set);
        }

        public void Apply(IManyToOneInstance instance)
        {
            instance.Cascade.SaveUpdate();
        }

        public void Accept(IAcceptanceCriteria<IManyToOneInspector> criteria)
        {
            criteria.Expect(x => x.Cascade, Is.Not.Set);
        }
    }
}
