using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace Trinity.Encore.Framework.Game.Database.Conventions.Properties
{
    internal sealed class PropertyUpdateConvention : IPropertyConvention, IPropertyConventionAcceptance
    {
        public void Apply(IPropertyInstance instance)
        {
            instance.Update();
        }

        public void Accept(IAcceptanceCriteria<IPropertyInspector> criteria)
        {
            criteria.Expect(x => !x.ReadOnly);
        }
    }
}
